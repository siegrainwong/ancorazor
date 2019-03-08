import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { ArticleParameters } from "../../models/article-parameters";
import { ArticleService } from "../../services/article.service";
import ArticleModel from "../../models/article-model";
import { Pagination } from "src/app/shared/models/response-result";
import { ActivatedRoute } from "@angular/router";
import { environment } from "src/environments/environment";
import { SGUtil } from "src/app/shared/utils/siegrain.utils";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";

@Component({
  selector: "app-article-list",
  templateUrl: "./article-list.component.html",
  styleUrls: ["./article-list.component.scss"]
})
export class ArticleListComponent implements OnInit {
  // component
  headerModel: ArticleModel = new ArticleModel({
    title: environment.title,
    cover: environment.homeCoverUrl
  });
  // request
  articles: ArticleModel[];
  pagination: Pagination;
  private _preloads: {
    articles: ArticleModel[];
    pagination: Pagination;
  };
  private _parameter = new ArticleParameters();
  // animate
  private _itemTransition = "articles";

  constructor(
    private service: ArticleService,
    private route: ActivatedRoute,
    public util: SGUtil,
    public transition: SGTransition
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(param => {
      this._parameter.pageIndex = param.index || 0;
      this.articles = [];
      this.pagination = null;
      this.getPosts();
    });
  }

  async getPosts() {
    if (this._preloads) {
      this.articles = this._preloads.articles;
      this.pagination = this._preloads.pagination;
      this._preloads = null;
    } else {
      let res = await this.service.getPagedArticles(this._parameter);
      if (!res || !res.succeed) return;
      this.articles = res.data as ArticleModel[];
      this.pagination = res.pagination;
    }
  }

  async preloadPosts() {
    let res = await this.service.getPagedArticles(this._parameter);
    if (!res || !res.succeed) return;
    this._preloads = {
      articles: res.data as ArticleModel[],
      pagination: res.pagination
    };
  }

  get itemTransition() {
    return this.transition.apply(this._itemTransition);
  }

  async previous() {
    if (!this.pagination.previousPageLink) return;
    this._parameter.pageIndex--;
    await this.preloadPosts();
    this.turnPage("previous");
  }

  async next() {
    if (!this.pagination.nextPageLink) return;
    this._parameter.pageIndex++;
    await this.preloadPosts();
    this.turnPage("next");
  }

  turnPage(direction: string) {
    this._itemTransition = `page_turn_${direction}`;
    this.util.routeTo(
      ["/"],
      {
        queryParams: { index: this._parameter.pageIndex },
        fragment: "nav"
      },
      this._preloads ? [this._itemTransition, "page_turn_button"] : null
    );
  }
}
