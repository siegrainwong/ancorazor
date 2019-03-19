import { Component, OnInit, Input } from "@angular/core";
import { ArticleService } from "../../services/article.service";
import { Router, ActivatedRoute } from "@angular/router";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import { LoggingService } from "src/app/shared/services/logging.service";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
import { SGUtil, topElementId } from "src/app/shared/utils/siegrain.utils";
import { externalScripts } from "src/app/shared/constants/siegrain.constants";

@Component({
  selector: "app-write-article",
  templateUrl: "./write-article.component.html",
  styleUrls: ["./write-article.component.scss"]
})
export class WriteArticleComponent implements OnInit {
  @Input() model = new ArticleModel({
    cover: "assets/img/write-bg.jpg",
    title: "",
    digest: ""
  });
  private _editor: any;
  public isEditing: boolean = false;
  public preloading: boolean = false;

  constructor(
    private _service: ArticleService,
    private _store: Store,
    private _logger: LoggingService,
    private _route: ActivatedRoute,
    public transition: SGTransition,
    private _util: SGUtil
  ) {}

  async ngOnInit() {
    await this.preloadArticle();
    if (this._store.renderFromClient) this.setupEditor();
  }

  private async preloadArticle() {
    if (this._store.preloadArticle) {
      this.model = this._store.preloadArticle;
      this._store.preloadArticle = null;
    } else {
      let id = this._route.snapshot.params.id;
      if (!id) return;
      let res = await this._service.getArticle(id);
      if (!res) return;
      this.model = res;
    }
    this.isEditing = true;
  }

  private async setupEditor() {
    await this._util.loadExternalScripts(externalScripts.tuiEditor);
    // await this._util.loadExternalScripts(externalScripts.tuiEditorScrollSync);
    this._editor = new tui.Editor({
      el: document.querySelector("#editor"),
      initialEditType: "markdown",
      previewStyle: "vertical",
      height: "800px",
      initialValue: this.model.content,
      exts: ["scrollSync"]
    });
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
    this.model.content = this._editor.getValue();
    if (!this.model.title || !this.model.title.length)
      return this._util.tip("Title is required");
    if (!this.model.content || !this.model.content.length)
      return this._util.tip("Content is required");

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
