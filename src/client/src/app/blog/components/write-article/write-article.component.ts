import { Component, OnInit, Input } from "@angular/core";
import { ArticleService } from "../../services/article.service";
import { Router, ActivatedRoute } from "@angular/router";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import { LoggingService } from "src/app/shared/services/logging.service";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
import { SGUtil } from "src/app/shared/utils/siegrain.utils";

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
  private editor: any;
  public isEditing: boolean = false;
  public preloading: boolean = false;

  constructor(
    private service: ArticleService,
    private router: Router,
    private store: Store,
    private logger: LoggingService,
    private route: ActivatedRoute,
    public transition: SGTransition,
    private util: SGUtil
  ) {}

  async ngOnInit() {
    await this.preloadArticle();
    this.setupEditor();
  }

  private async preloadArticle() {
    if (this.store.preloadArticle) {
      this.model = this.store.preloadArticle;
      this.store.preloadArticle = null;
    } else {
      let id = this.route.snapshot.params.id;
      if (!id) return;
      let res = await this.service.getArticle(id);
      if (!res) return;
      this.model = res;
    }
    this.isEditing = true;
  }

  private setupEditor() {
    if (this.store.renderFromServer) return;
    let Editor = require("tui-editor");
    this.editor = new Editor({
      el: document.querySelector("#editor"),
      initialEditType: "markdown",
      previewStyle: "vertical",
      height: "800px",
      initialValue: this.model.content
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
    this.model.content = this.editor.getValue();
    if (!this.model.title || !this.model.title.length)
      return this.util.tip("Title is required");
    if (!this.model.content || !this.model.content.length)
      return this.util.tip("Content is required");

    this.preloading = true;
    this.logger.info("posting: ", this.model);
    var res = this.isEditing
      ? await this.service.update(this.model)
      : await this.service.add(this.model);
    this.preloading = false;
    if (!res) return;
    this.util.routeTo([`article/${res}`]);
  }

  // TODO:
  preview() {}
}
