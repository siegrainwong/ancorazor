import { Component, OnInit, NgModule } from "@angular/core";
import { ArticleParameters } from "../../models/article-parameters";
import { ArticleService } from "../../services/article.service";
import ArticleModel from "../../models/article-model";
import { Pagination } from "src/app/shared/models/response-result";
import { OpenIdConnectService } from "src/app/shared/oidc/open-id-connect.service";

@Component({
  selector: "app-article-list",
  templateUrl: "./article-list.component.html",
  styleUrls: ["./article-list.component.scss"]
})
export class ArticleListComponent implements OnInit {
  constructor(private postService: ArticleService) {
    console.log("article-list ctor.");
  }
  ngOnInit() {
    this.getPost();
  }

  articles: ArticleModel[];
  pagination: Pagination;
  parameter = new ArticleParameters({
    orderBy: "id desc",
    pageSize: 10,
    pageIndex: 0
  });
  async getPost() {
    let res = await this.postService.getPagedArticles(this.parameter);
    if (!res || !res.succeed) return;
    this.articles = res.data as ArticleModel[];
    this.pagination = res.pagination;
  }
  previous() {
    if (!this.pagination.previousPageLink) return;
    this.parameter.pageIndex--;
    this.getPost();
  }

  next() {
    if (!this.pagination.nextPageLink) return;
    this.parameter.pageIndex++;
    this.getPost();
  }
}
