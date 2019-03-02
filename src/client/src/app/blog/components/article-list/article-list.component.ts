import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { ArticleParameters } from "../../models/article-parameters";
import { ArticleService } from "../../services/article.service";
import ArticleModel from "../../models/article-model";
import { Pagination } from "src/app/shared/models/response-result";
import { Router, ActivatedRoute } from "@angular/router";
import { Store } from "src/app/shared/store/store";

@Component({
  selector: "app-article-list",
  templateUrl: "./article-list.component.html",
  styleUrls: ["./article-list.component.scss"]
})
export class ArticleListComponent implements OnInit {
  articles: ArticleModel[];
  pagination: Pagination;
  parameter = new ArticleParameters();
  @Output() articlePressed = new EventEmitter<ArticleModel>();

  constructor(
    private service: ArticleService,
    private router: Router,
    private route: ActivatedRoute,
    private store: Store
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(param => {
      this.parameter.pageIndex = param.index || 0;
      this.getPosts();
    });
  }

  async getPosts() {
    let res = await this.service.getPagedArticles(this.parameter);
    if (!res || !res.succeed) return;
    this.articles = res.data as ArticleModel[];
    this.pagination = res.pagination;
  }

  readPost(model: ArticleModel) {
    this.store.headerModel = model;
    this.router.navigate([`/article/${model.id}`]);
  }

  previous() {
    if (!this.pagination.previousPageLink) return;
    this.parameter.pageIndex--;
    this.query();
  }

  next() {
    if (!this.pagination.nextPageLink) return;
    this.parameter.pageIndex++;
    this.query();
  }

  query() {
    this.router.navigate(["/"], {
      queryParams: { index: this.parameter.pageIndex }
    });
  }
}
