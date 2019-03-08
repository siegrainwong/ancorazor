import { Component, OnInit } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { ArticleService } from "../../services/article.service";
import { ActivatedRoute, Router } from "@angular/router";
import { Store } from "src/app/shared/store/store";

@Component({
  selector: "app-article",
  templateUrl: "./article.component.html",
  styleUrls: ["./article.component.scss"]
})
export class ArticleComponent implements OnInit {
  model: ArticleModel = new ArticleModel();
  constructor(
    private articleService: ArticleService,
    private route: ActivatedRoute,
    public store: Store
  ) {}
  ngOnInit() {
    // if (this.store.headerModel) this.model = this.store.headerModel;
    this.getArticle();
  }

  async getArticle() {
    let id = parseInt(this.route.snapshot.params.id);
    let res = await this.articleService.getArticle(id);
    if (!res || !res.succeed) return;
    this.model = res.data as ArticleModel;
    this.setupEditor();
  }

  setupEditor() {
    if (this.store.renderFromServer) return;
    let Viewer = require("tui-editor/dist/tui-editor-Viewer");
    new Viewer({
      el: document.querySelector("#viewer"),
      initialValue: this.model.content
    });
  }
}
