import { Component, OnInit, Input } from "@angular/core";
import { ArticleService } from "../../services/article.service";
import { Router } from "@angular/router";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import { LoggingService } from "src/app/shared/services/logging.service";

@Component({
  selector: "app-write-article",
  templateUrl: "./write-article.component.html",
  styleUrls: ["./write-article.component.scss"]
})
export class WriteArticleComponent implements OnInit {
  @Input() model = new ArticleModel();
  private editor: any;

  constructor(
    private service: ArticleService,
    private router: Router,
    private store: Store,
    private logger: LoggingService
  ) {}

  ngOnInit() {
    this.setupEditor();
  }

  setupEditor() {
    if (this.store.renderFromServer) return;
    let Editor = require("tui-editor");
    this.editor = new Editor({
      el: document.querySelector("#editor"),
      initialEditType: "markdown",
      previewStyle: "vertical",
      height: "500px",
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
    this.logger.info("posting: ", this.model);
    var res = await this.service.add(this.model);
    if (!res.succeed) return;

    var model = res.data as ArticleModel;
    this.router.navigate([`article/${model.id}`]);
  }

  // TODO:
  preview() {}
}
