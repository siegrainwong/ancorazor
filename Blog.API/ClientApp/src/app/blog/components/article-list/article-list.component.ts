import { Component, OnInit, OnDestroy } from "@angular/core";
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
import { SGTransition } from "src/app/shared/animations/sg-transition";
import { Title } from "@angular/platform-browser";
import { Store } from "src/app/shared/store/store";
import { constants } from "src/app/shared/constants/siegrain.constants";
import {
  SGTransitionDelegate,
  SGCustomTransitionDelegate
} from "src/app/shared/animations/sg-transition.interface";
import {
  SGTransitionMode,
  SGAnimation,
  RouteTransitionCommands,
  CustomizeTransitionCommands,
  TransitionCommands
} from "src/app/shared/animations/sg-transition.model";
import { Subscription } from "rxjs";

const StaggerDuration = 200; // 列表总动画时长 = transition duration + stagger duration

@Component({
  selector: "app-article-list",
  templateUrl: "./article-list.component.html",
  styleUrls: ["./article-list.component.scss"]
})
export class ArticleListComponent
  implements OnInit, OnDestroy, SGCustomTransitionDelegate {
  // component
  public headerModel: ArticleModel = new ArticleModel({
    title: constants.title,
    cover: constants.homeCoverUrl
  });
  // request
  public data: PagedResult<ArticleModel>;
  public preloading: boolean = false;

  // item animation types
  public readonly itemAnimations = {
    route: "fade-opposite",
    next: "page-turn-next",
    previous: "page-turn-previous"
  };

  private _parameter = new ArticleParameters();
  private _currentItemAnimationName: string = this.itemAnimations.route;
  private _preloads: PagedResult<ArticleModel>;
  private _subscription = new Subscription();
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
    this._subscription.add(
      this._route.params.subscribe(async param => {
        this._parameter.pageIndex = param.index || 0;
        await this.getArticles();
        this.setItemTransition(this._currentItemAnimationName);
      })
    );
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  // get animationNames(): string[] {
  //   return [this._currentItemAnimationName, "page-turn-button"];
  // }

  RouteTransitionForComponent?(
    component: SGTransitionDelegate,
    url: string
  ): TransitionCommands {
    if (url.startsWith("/article") || url.startsWith("/edit"))
      return new RouteTransitionCommands({ scrollTo: topElementId });
    return null;
  }

  CustomizeTransitionForComponent(
    component: SGCustomTransitionDelegate,
    url: string
  ): CustomizeTransitionCommands {
    return new CustomizeTransitionCommands({
      names: [this._currentItemAnimationName, "page-turn-button"],
      extraDuration: StaggerDuration,
      scrollTo: topElementId
    });
  }

  ModeForComponentTransition(
    component: SGCustomTransitionDelegate,
    url: string
  ): SGTransitionMode {
    const isTurningPage =
      url.length > 0 && typeof parseInt(url.replace("/", "")) == "number";
    return isTurningPage ? SGTransitionMode.custom : SGTransitionMode.route;
  }

  // public async edit(model: ArticleModel) {
  //   (await this.preloadArticle(model)) &&
  //     this.util.routeTo(["/edit", model.id], {
  //       scrollToElementId: topElementId
  //     });
  // }

  public async delete(item: ArticleModel, index: number) {
    let result =
      (await this.util.confirm("Delete", TipType.Danger)) &&
      (await this._service.remove(item.id));
    if (!result) return;

    // 创建一个新动画对象单独执行动画
    item.animation = new SGAnimation(item.animation);
    await this.transition.triggerAnimations([item.animation]);
    this.data.list.splice(index, 1);
    this.setItemTransition(this.itemAnimations.route);
    await this.transition.triggerAnimations([item.animation]);
  }

  private async getArticles() {
    if (this._preloads) {
      this.data = this._preloads;
      this._preloads = null;
      this.preloading = false;
    } else {
      let res = await this._service.getPagedArticles(this._parameter);
      if (!res) return;
      this.data = res;
    }
  }

  // private async preloadArticle(model: ArticleModel): Promise<boolean> {
  //   let res = await this._service.getArticle(model.id);
  //   if (!res) return Promise.resolve(false);
  //   this.store.preloadArticle = res;
  //   return Promise.resolve(true);
  // }

  // private async preloadArticles(): Promise<boolean> {
  //   this.preloading = true;
  //   let res = await this._service.getPagedArticles(this.parameter);
  //   if (!res) {
  //     this.preloading = false;
  //     return Promise.resolve(false);
  //   }
  //   this._preloads = res;
  //   return Promise.resolve(true);
  // }

  /** 设置 article item 的动画 */
  private setItemTransition(name: string) {
    this._currentItemAnimationName = name;
    this.data &&
      this.data.list.map(
        x => (x.animation = this.transition.getAnimation(name))
      );
  }
}
