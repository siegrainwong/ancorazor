import { Component, OnInit } from "@angular/core";
import { ArticleParameters } from "../../models/article-parameters";
import { ArticleService } from "../../services/article.service";
import ArticleModel from "../../models/article-model";
import { PagedResult } from "src/app/shared/models/response-result";
import { ActivatedRoute, RouterStateSnapshot } from "@angular/router";
import {
  SGUtil,
  TipType,
  topElementId
} from "src/app/shared/utils/siegrain.utils";
import {
  SGTransition,
  SGTransitionMode,
  SGAnimation
} from "src/app/shared/utils/siegrain.animations";
import { Title } from "@angular/platform-browser";
import { Store } from "src/app/shared/store/store";
import { constants } from "src/app/shared/constants/siegrain.constants";
import {
  CanComponentTransitionToLeave,
  TransitionToLeaveCommands
} from "src/app/shared/guard/transition-leaving.deactivate.guard";

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
export class ArticleListComponent
  implements OnInit, CanComponentTransitionToLeave {
  // component
  headerModel: ArticleModel = new ArticleModel({
    title: constants.title,
    cover: constants.homeCoverUrl
  });
  // request
  data: PagedResult<ArticleModel>;
  preloading: boolean = false;
  private _preloads: PagedResult<ArticleModel>;
  parameter = new ArticleParameters();
  // article item animation
  private _itemAnimationName: ItemAnimationName;

  constructor(
    private _service: ArticleService,
    private _route: ActivatedRoute,
    private _titleService: Title,
    public util: SGUtil,
    public transition: SGTransition,
    public store: Store
  ) {}

  ngOnInit() {
    this._titleService.setTitle(`${constants.titlePlainText}`);
    this._route.queryParams.subscribe(param => {
      this.parameter.pageIndex = param.index || 0;
      this.data = null;
      this.getArticles();
    });
    this.transition.transitionWillBegin$.subscribe(data => {
      data &&
        data.mode == SGTransitionMode.route &&
        this.setupTransitions(ItemAnimationName.route);
    });
  }

  // TODO: 搞一个返回 CustomTransitionCommands 的接口，用于调用自定义动画

  CanComponentTransitionToLeaving?(
    nextState: RouterStateSnapshot
  ): boolean | TransitionToLeaveCommands {
    if (nextState.url.startsWith("/article"))
      return new TransitionToLeaveCommands({ scrollTo: topElementId });

    return true;
  }

  public async read(model: ArticleModel) {
    (await this.preloadArticle(model)) &&
      this.util.routeTo(["/article", model.id], {
        scrollToElementId: topElementId
      });
  }

  public async edit(model: ArticleModel) {
    (await this.preloadArticle(model)) &&
      this.util.routeTo(["/edit", model.id], {
        scrollToElementId: topElementId
      });
  }

  public async delete(item: ArticleModel, index: number) {
    let result =
      (await this.util.confirm("Delete", TipType.Danger)) &&
      (await this._service.remove(item.id));
    if (!result) return;

    // 创建一个新动画对象单独执行动画
    item.animation = new SGAnimation(item.animation);
    await this.transition.triggerAnimations([item.animation]);
    this.data.list.splice(index, 1);
  }

  public async previous() {
    if (!this.data.hasPrevious) return;
    this.parameter.pageIndex = this.data.previousPageIndex;
    (await this.preloadArticles()) && this.turnPage(ItemAnimationName.previous);
  }

  public async next() {
    if (!this.data.hasNext) return;
    this.parameter.pageIndex = this.data.nextPageIndex;
    (await this.preloadArticles()) && this.turnPage(ItemAnimationName.next);
  }

  private async getArticles() {
    if (this._preloads) {
      this.data = this._preloads;
      this._preloads = null;
      this.preloading = false;
    } else {
      let res = await this._service.getPagedArticles(this.parameter);
      if (!res) return;
      this.data = res;
    }
    this.setupTransitions(this._itemAnimationName);
  }

  private async preloadArticle(model: ArticleModel): Promise<boolean> {
    let res = await this._service.getArticle(model.id);
    if (!res) return Promise.resolve(false);
    this.store.preloadArticle = res;
    return Promise.resolve(true);
  }

  private async preloadArticles(): Promise<boolean> {
    this.preloading = true;
    let res = await this._service.getPagedArticles(this.parameter);
    if (!res) {
      this.preloading = false;
      return Promise.resolve(false);
    }
    this._preloads = res;
    return Promise.resolve(true);
  }

  /** 设置 article item 的动画 */
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
        queryParams: { index: this.parameter.pageIndex }
      },
      names: this._preloads
        ? [this._itemAnimationName, "page-turn-button"]
        : null,
      extraDuration: StaggerDuration,
      scrollToElementId: topElementId
    });
  }
}
