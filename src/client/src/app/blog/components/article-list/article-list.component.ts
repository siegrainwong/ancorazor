import { Component, OnInit } from "@angular/core";
import { ArticleParameters } from "../../models/article-parameters";
import { ArticleService } from "../../services/article.service";
import ArticleModel from "../../models/article-model";
import { Pagination } from "src/app/shared/models/response-result";
// import { OpenIdConnectService } from 'src/app/shared/oidc/open-id-connect.service';

@Component({
  selector: "app-article-list",
  templateUrl: "./article-list.component.html",
  styleUrls: ["./article-list.component.scss"]
})
export class ArticleListComponent implements OnInit {
  constructor(
    private postService: ArticleService
  ) // private userService: OpenIdConnectService
  {}
  ngOnInit() {
    this.getPost();
  }

  articles: ArticleModel[];
  pagiation: Pagination;
  parameter = new ArticleParameters({
    orderBy: "id desc",
    pageSize: 10,
    pageIndex: 0
  });
  async getPost() {
    let res = await this.postService.getPagedArticles(this.parameter);
    this.articles = res.data as ArticleModel[];
    this.pagiation = res.pagination;
  }
}
