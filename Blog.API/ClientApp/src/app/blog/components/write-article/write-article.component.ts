import { Component, OnInit, Input, OnDestroy, Renderer } from "@angular/core";
import { ArticleService } from "../../services/article.service";
import { ActivatedRoute } from "@angular/router";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import { LoggingService } from "src/app/shared/services/logging.service";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
import { SGUtil, topElementId } from "src/app/shared/utils/siegrain.utils";
import { externalScripts } from "src/app/shared/constants/siegrain.constants";
// import * as matter from "gray-matter";

@Component({
  selector: "app-write-article",
  templateUrl: "./write-article.component.html",
  styleUrls: ["./write-article.component.scss"]
})
export class WriteArticleComponent implements OnInit, OnDestroy {
  @Input() model = new ArticleModel({
    cover: "assets/img/write-bg.jpg",
    title: "",
    digest: ""
  });
  private _editor: any;
  public isEditing: boolean = false;
  public preloading: boolean = false;
  private _frontMatter: any;
  private _hrCount: number = 0;

  constructor(
    private _service: ArticleService,
    private _store: Store,
    private _logger: LoggingService,
    private _route: ActivatedRoute,
    private _util: SGUtil,
    public transition: SGTransition
  ) {}

  async ngOnInit() {
    await this.preloadArticle();
    if (!this._store.renderFromClient) return;
    this.setupNav();
    this.setupEditor();
  }

  ngOnDestroy() {
    this.restoreNav();
  }

  private async preloadArticle() {
    if (this._store.preloadArticle) {
      this.model = this._store.preloadArticle;
      this._store.preloadArticle = null;
    } else {
      let id = this._route.snapshot.params.id;
      let res = id && (await this._service.getArticle(id));
      if (!res) return;
      this.model = res;
    }
    this.isEditing = true;
  }

  private setupNav() {
    let nav = document.querySelector("#mainNav");
    nav.classList.add("is-visible", "is-fixed");
  }

  private restoreNav() {
    let nav = document.querySelector("#mainNav");
    nav.classList.remove("is-visible", "is-fixed");
  }

  private async setupEditor() {
    await this._util.loadExternalScripts(Object.values(externalScripts));

    // this.setupRenderer();
    // PS：改了很多样式在 _reset.css 里
    this._editor = new EasyMDE({
      element: document.querySelector("#editor"),
      initialValue: this.model.content,
      autoDownloadFontAwesome: false,
      spellChecker: false,
      renderingConfig: {
        codeSyntaxHighlighting: true,
        markedOptions: {
          renderer: this.getRenderer()
        }
      }
    });
    this._editor.toggleSideBySide();
    const self = this;
    const yamlFront = require("yaml-front-matter");
    this._editor.codemirror.on("change", function() {
      let value = self._editor.value() as string;
      self._frontMatter = yamlFront.loadFront(value);
      self._hrCount = value.split("---").length;
    });
  }

  /**
   * Mark: 修改`Marked`的词法分析器让其支持`yaml front matter`.
   */
  private getRenderer() {
    let renderer = new marked.Renderer();

    const self = this;
    let count = 0;
    // Bug: 一直加字有惊喜
    renderer.hr = function() {
      count++;
      if (count > self._hrCount) count = 1;

      if (!self.hasFrontMatter) {
        let $matter = document.querySelector(".sg-front-matter");
        $matter && $matter.remove();
        return "<hr>";
      }
      if (count > 2) return "<hr>";

      if (count == 1) {
        return "<div class='sg-front-matter'>";
      } else if (count == 2) {
        return "</div>";
      }
    };

    return renderer;
  }

  private get hasFrontMatter(): boolean {
    return Object.keys(this._frontMatter).length > 2;
  }

  /**
   * Events
   */

  onHeaderChanged(model: ArticleModel) {
    this.model.title = model.title;
    this.model.digest = model.digest;
    this.model.cover = model.cover;
  }

  async submit() {
    this.model.content = this._editor.value();
    if (!this.hasFrontMatter)
      // TODO: 添加帮助链接
      return this._util.tip("Yaml front matter not found!");
    if (!this._frontMatter.title || !this._frontMatter.title.length)
      return this._util.tip("Title is required");
    if (!this.model.content || !this.model.content.length)
      return this._util.tip("Content is required");

    this.model.title = this._frontMatter.title;

    this.preloading = true;
    this._logger.info("posting: ", this.model);
    var res = this.isEditing
      ? await this._service.update(this.model)
      : await this._service.add(this.model);
    this.preloading = false;
    if (!res) return;
    this._util.routeTo([`article/${res}`], { scrollToElementId: topElementId });
  }

  // TODO: 预览
  preview() {}
}
