import { Component, OnInit } from "@angular/core";
import { ArticleParameters } from "../../models/article-parameters";
import { ArticleService } from "../../services/article.service";
import ArticleModel from "../../models/article-model";
import { PagedResult } from "src/app/shared/models/response-result";
import { ActivatedRoute } from "@angular/router";
import { SGUtil, TipType } from "src/app/shared/utils/siegrain.utils";
import {
  SGTransition,
  SGTransitionMode,
  SGAnimation
} from "src/app/shared/utils/siegrain.animations";
import { Title } from "@angular/platform-browser";
import { Store } from "src/app/shared/store/store";
import { constants } from "src/app/shared/constants/siegrain.constants";

enum ItemAnimationName {
  route = "fade-opposite",
  next = "page-turn-next",
  previous = "page-turn-previous"
}
const StaggerDuration = 200; // 列表总动画时长 = transition duration + stagger duration

@Component({
  selector: "app-article-list",
  templateUrl: "./article-list.component.html",
  styleUrls: ["./article-list.component.scss"]
})
export class ArticleListComponent implements OnInit {
  // component
  headerModel: ArticleModel = new ArticleModel({
    title: constants.title,
    cover: constants.homeCoverUrl
  });
  // request
  data: PagedResult<ArticleModel>;
  preloading: boolean = false;
  private _preloads: PagedResult<ArticleModel>;
  private _parameter = new ArticleParameters();
  // article item animation
  private _itemAnimationName: ItemAnimationName;

  constructor(
    private service: ArticleService,
    private route: ActivatedRoute,
    public util: SGUtil,
    public transition: SGTransition,
    private titleService: Title,
    public store: Store
  ) {}

  ngOnInit() {
    this.titleService.setTitle(`${constants.titlePlainText}`);
    this.route.queryParams.subscribe(param => {
      this._parameter.pageIndex = param.index || 0;
      this.data = null;
      this.getArticles();
    });
    this.transition.transitionWillBegin$.subscribe(mode => {
      mode == SGTransitionMode.route &&
        this.setupTransitions(ItemAnimationName.route);
    });
  }

  public async read(model: ArticleModel) {
    (await this.preloadArticle(model)) &&
      this.util.routeTo(["/article", model.id], {
        scrollToElementId: "#content"
      });
  }

  public async edit(model: ArticleModel) {
    (await this.preloadArticle(model)) &&
      this.util.routeTo(["/edit", model.id], {
        scrollToElementId: "#content"
      });
  }

  public async delete(item: ArticleModel, index: number) {
    let result =
      (await this.util.confirm("Delete", TipType.Danger)) &&
      (await this.service.remove(item.id));
    if (!result) return;

    // 创建一个新动画对象单独执行动画
    item.animation = new SGAnimation(item.animation);
    await this.transition.triggerAnimations([item.animation]);
    this.data.list.splice(index, 1);
  }

  public async previous() {
    if (!this.data.hasPrevious) return;
    this._parameter.pageIndex--;
    (await this.preloadArticles()) && this.turnPage(ItemAnimationName.previous);
  }

  public async next() {
    if (!this.data.hasNext) return;
    this._parameter.pageIndex++;
    (await this.preloadArticles()) && this.turnPage(ItemAnimationName.next);
  }

  private async getArticles() {
    if (this._preloads) {
      this.data = this._preloads;
      this._preloads = null;
      this.preloading = false;
    } else {
      let res = await this.service.getPagedArticles(this._parameter);
      if (!res) return;
      this.data = res;
    }
    this.setupTransitions(this._itemAnimationName);
  }

  private async preloadArticle(model: ArticleModel): Promise<boolean> {
    let res = await this.service.getArticle(model.id);
    if (!res) return Promise.resolve(false);
    this.store.preloadArticle = res;
    return Promise.resolve(true);
  }

  private async preloadArticles(): Promise<boolean> {
    this.preloading = true;
    let res = await this.service.getPagedArticles(this._parameter);
    if (!res) {
      this.preloading = false;
      return Promise.resolve(false);
    }
    this._preloads = res;
    return Promise.resolve(true);
  }

  private setupTransitions(name?: ItemAnimationName) {
    this._itemAnimationName = name;
    this.data &&
      this.data.list.map(
        x =>
          (x.animation = this.transition.getAnimation(this._itemAnimationName))
      );
  }

  private turnPage(animationName: ItemAnimationName) {
    this.setupTransitions(animationName);
    this.util.routeTo(["/"], {
      extras: {
        queryParams: { index: this._parameter.pageIndex }
      },
      names: this._preloads
        ? [this._itemAnimationName, "page-turn-button"]
        : null,
      extraDuration: StaggerDuration,
      scrollToElementId: "#articleList"
    });
  }
}
