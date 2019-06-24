import { Component, OnInit, Input, OnDestroy } from "@angular/core";
import { ArticleService } from "../../services/article.service";
import { Router, ActivatedRouteSnapshot } from "@angular/router";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import { LoggingService } from "src/app/shared/services/logging.service";
import {
  SGUtil,
  topElementId,
  XSRFTokenKey
} from "src/app/shared/utils/siegrain.utils";
import {
  externalScripts,
  articleDefaultContent,
  coverSize
} from "src/app/shared/constants/siegrain.constants";
import { timeFormat } from "src/app/shared/utils/time-format";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";
import { SGRouteTransitionCommands } from "src/app/shared/animations/sg-transition.model";
import { take, map } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { ObservedComponentBase } from "src/app/shared/components/observed.base";

@Component({
  selector: "app-write-article",
  templateUrl: "./write-article.component.html",
  styleUrls: ["./write-article.component.scss"]
})
export class WriteArticleComponent implements OnInit, SGTransitionDelegate {
  public animations = {
    editor: SGAnimations.fade
  };
  @Input() model = new ArticleModel({
    title: "",
    digest: ""
  });
  public isEditing: boolean = false;
  private _editor: any;
  private _lexer: any;
  private _frontMatter: any;

  constructor(
    private _service: ArticleService,
    private _logger: LoggingService,
    private _util: SGUtil,
    private _router: Router,
    public store: Store
  ) {}

  async ngOnInit() {
    if (!this.store.renderFromClient) return;
    let article = await this.store.routeDataChanged$
      .pipe(
        take(1),
        map(x => x.article)
      )
      .toPromise();
    if (article) {
      this.isEditing = true;
      this.model = article;
    }
    // this.setupNav();
    this.setupEditor();
    this.setupUploader();
  }

  transitionForComponent?(
    nextRoute: ActivatedRouteSnapshot
  ): SGRouteTransitionCommands {
    return new SGRouteTransitionCommands({ scrollTo: topElementId });
  }

  private async setupEditor() {
    await this._util.loadExternalScripts([
      externalScripts.editor,
      externalScripts.highlight
    ]);

    // PS：改了很多样式在 _reset.css 里
    var template =
      this.store.siteSetting.articleTemplate || articleDefaultContent;
    this._editor = new EasyMDE({
      element: document.querySelector("#editor"),
      initialValue: this.model.content || template,
      autoDownloadFontAwesome: false,
      spellChecker: false,
      renderingConfig: {
        codeSyntaxHighlighting: true,
        markedLexer: this.lexer
      }
    });
    this._editor.toggleSideBySide();
  }

  /**
   * 修改`markedjs`的词法分析器使其支持`yaml front matter`
   */
  private get lexer() {
    if (this._lexer) return this._lexer;
    const yamlFront = require("yaml-front-matter");

    const self = this;
    let lex = marked.Lexer.lex;
    this._lexer = function(text, options) {
      // 这里是不能抛异常的，抛了会崩掉整个编辑器
      try {
        let parsed = (self._frontMatter = yamlFront.loadFront(text));
        if (!self.hasFrontMatter) return lex(text, options);

        return lex(`# ${parsed.title} \n\n\n ${parsed.__content}`, options);
      } catch {
        return lex(text, options);
      }
    };
    return this._lexer;
  }

  private get hasFrontMatter(): boolean {
    return this._frontMatter && Object.keys(this._frontMatter).length > 2;
  }

  private async setupUploader() {
    await this._util.loadExternalScripts([
      externalScripts.filepond,
      externalScripts.filepondResize,
      externalScripts.filepondSizeValidation,
      externalScripts.filepondCrop
    ]);
    FilePond.registerPlugin(
      FilePondPluginFileValidateSize,
      FilePondPluginImageResize,
      FilePondPluginImageTransform,
      FilePondPluginImageCrop,
      FilePondPluginFileValidateType
    );
    FilePond.setOptions({
      // api
      server: {
        url: environment.apiUrlBase,
        process: {
          url: "/common/upload/cover",
          headers: {
            "X-XSRF-TOKEN": this._util.getCookie(XSRFTokenKey)
          },
          onload: res => {
            const resJson = JSON.parse(res);
            this.model.cover = resJson.data.id;
            return res.key;
          }
        }
      },
      // crop/resize/transform things
      allowImageResize: true,
      allowImageCrop: true,
      imageResizeTargetWidth: coverSize.width,
      imageResizeTargetHeight: coverSize.height,
      imageResizeMode: "cover",
      imageCropAspectRatio: coverSize.ratio,
      // type validation
      allowFileTypeValidation: true,
      acceptedFileTypes: ["image/png", "image/jpg", "image/bmp", "image/jpeg"]
    });
    const pond = FilePond.create(document.querySelector("#cover-uploader"));
    pond.labelIdle =
      'Drag & Drop your cover image here or <span class="filepond--label-action">Browse</span>';
  }

  /**
   * Events
   */

  async submit() {
    // validations
    if (!this._frontMatter.__content || !this._frontMatter.__content.length)
      return this._util.tip("The content is empty.");
    if (!this.hasFrontMatter)
      return this._util.tip("Yaml front matter (What is this?) not found."); // TODO: 添加帮助链接
    if (!this._frontMatter.title || !this._frontMatter.title.length)
      return this._util.tip("Title is required.");

    // assemble
    this.model.content = this._editor.value();
    this.model.title = this._frontMatter.title;
    this.model.createdAt = this._frontMatter.date;
    this.model.tags = this._frontMatter.tags;
    this.model.category = this._frontMatter.category;
    if (this._frontMatter.categories && !this.model.category)
      this.model.category =
        Array.isArray(this._frontMatter.categories) &&
        this._frontMatter.categories[0];
    this.model.digest = this._frontMatter.description;
    this.model.alias = this._frontMatter.alias;
    const isDraft =
      !!this._frontMatter.draft &&
      (this._frontMatter.draft as string).toLowerCase();
    this.model.isDraft = (!!isDraft && isDraft === "true") || isDraft === "yes";

    // submit
    this._logger.info("submiting: ", this.model);
    var res = this.isEditing
      ? await this._service.update(this.model)
      : await this._service.add(this.model);
    if (!res) return;
    this._router.navigate([`article/${res.path}`]);
  }
}
