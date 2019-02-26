import { Component, OnInit, Input } from "@angular/core";
import { ArticleService } from "../../services/article.service";
import { Router } from "@angular/router";
import ArticleModel from "../../models/article-model";
import { Variables } from "src/app/shared/variables";

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
    private variables: Variables
  ) {}

  ngOnInit() {
    this.setupEditor();
  }

  setupEditor() {
    if (this.variables.renderFromServer) return;
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
  }

  async submit() {
    this.model.content = this.editor.getValue();

    var res = await this.service.add(this.model);
    if (!res.succeed) return;

    var model = res.data as ArticleModel;
    console.log(model);
    this.router.navigate([`article/${model.id}`]);
  }

  // TODO:
  preview() {}
}
