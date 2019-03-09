import { Component, OnInit } from "@angular/core";
import { ArticleParameters } from "../../models/article-parameters";
import { ArticleService } from "../../services/article.service";
import ArticleModel from "../../models/article-model";
import { Pagination } from "src/app/shared/models/response-result";
import { ActivatedRoute } from "@angular/router";
import { environment } from "src/environments/environment";
import { SGUtil } from "src/app/shared/utils/siegrain.utils";
import {
  SGTransition,
  SGTransitionMode
} from "src/app/shared/utils/siegrain.animations";
import { Title } from "@angular/platform-browser";
import { Store } from 'src/app/shared/store/store';

enum ItemTransition {
  route = "articles",
  next = "page_turn_next",
  previous = "page_turn_previous"
}
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
  preloading: boolean = false;
  private _preloads: {
    articles: ArticleModel[];
    pagination: Pagination;
  };
  private _parameter = new ArticleParameters();
  // article item animation
  private _itemTransition: ItemTransition = ItemTransition.route;

  constructor(
    private service: ArticleService,
    private route: ActivatedRoute,
    public util: SGUtil,
    public transition: SGTransition,
    private titleService: Title,
    private store: Store
  ) {}

  ngOnInit() {
    this.titleService.setTitle(`${environment.titlePlainText}`);
    this.route.queryParams.subscribe(param => {
      this._parameter.pageIndex = param.index || 0;
      this.articles = [];
      this.pagination = null;
      this.getArticles();
    });
    this.transition.transitionWillBegin$.subscribe(mode => {
      if (mode == SGTransitionMode.route)
        this._itemTransition = ItemTransition.route;
    });
  }

  async readPost(model: ArticleModel){
    let res = await this.service.getArticle(model.id);
    if(!res || !res.succeed) return;
    this.store.preloadArticle = res.data as ArticleModel;
    this.util.routeTo(['/article', model.id]);
  }

  async getArticles() {
    if (this._preloads) {
      this.articles = this._preloads.articles;
      this.pagination = this._preloads.pagination;
      this._preloads = null;
      this.preloading = false;
    } else {
      let res = await this.service.getPagedArticles(this._parameter);
      if (!res || !res.succeed) return;
      this.articles = res.data as ArticleModel[];
      this.pagination = res.pagination;
    }
  }

  async preloadArticles() {
    this.preloading = true;
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
    await this.preloadArticles();
    this.turnPage(ItemTransition.previous);
  }

  async next() {
    if (!this.pagination.nextPageLink) return;
    this._parameter.pageIndex++;
    await this.preloadArticles();
    this.turnPage(ItemTransition.next);
  }

  turnPage(transition: ItemTransition) {
    this._itemTransition = transition;
    this.util.routeTo(
      ["/"],
      {
        queryParams: { index: this._parameter.pageIndex }
      },
      this._preloads ? [this._itemTransition, "page_turn_button"] : null
    );
  }
}
