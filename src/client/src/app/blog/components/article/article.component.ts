import { Component, OnInit } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { ArticleService } from "../../services/article.service";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-article",
  templateUrl: "./article.component.html",
  styleUrls: ["./article.component.scss"]
})
export class ArticleComponent implements OnInit {
  model: ArticleModel;
  constructor(
    private articleService: ArticleService,
    private route: ActivatedRoute
  ) {}
  ngOnInit() {
    this.getArticle();
  }
  async getArticle() {
    // let params = await this.route.params.toPromise();
    // console.log(params);

    let res = await this.articleService.getArticle(1);
    if (!res || !res.succeed) return;
    this.model = res.data as ArticleModel;
  }
}
