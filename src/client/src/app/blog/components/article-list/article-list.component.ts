import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { ArticleParameters } from "../../models/article-parameters";
import { ArticleService } from "../../services/article.service";
import ArticleModel from "../../models/article-model";
import { Pagination } from "src/app/shared/models/response-result";
import { Router, ActivatedRoute } from "@angular/router";
import { environment } from "src/environments/environment";
import { SGUtil } from "src/app/shared/utils/siegrain.utils";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";

@Component({
  selector: "app-article-list",
  templateUrl: "./article-list.component.html",
  styleUrls: ["./article-list.component.scss"]
})
export class ArticleListComponent implements OnInit {
  headerModel: ArticleModel = new ArticleModel({
    title: environment.title,
    cover: environment.homeCoverUrl
  });
  articles: ArticleModel[];
  pagination: Pagination;
  parameter = new ArticleParameters();
  private transitionName = "articles";
  @Output() articlePressed = new EventEmitter<ArticleModel>();

  constructor(
    private service: ArticleService,
    private router: Router,
    private route: ActivatedRoute,
    public util: SGUtil,
    private transition: SGTransition
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

  get transitionClass() {
    return this.transition.apply(this.transitionName);
  }

  previous() {
    if (!this.pagination.previousPageLink) return;
    this.transitionName = "page_turn_previous";
    this.parameter.pageIndex--;
    this.query();
  }

  next() {
    if (!this.pagination.nextPageLink) return;
    this.transitionName = "page_turn_next";
    this.parameter.pageIndex++;
    this.query();
  }

  query() {
    this.util.routeTo(["/"], {
      queryParams: { index: this.parameter.pageIndex }
    });
  }
}
