import { Component, OnInit, OnDestroy } from "@angular/core";
import { ArticleService } from "../../services/article.service";
import ArticleModel from "../../models/article-model";
import { PagedResult } from "src/app/shared/models/response-result";
import { ActivatedRouteSnapshot } from "@angular/router";
import {
  SGUtil,
  TipType,
  topElementId
} from "src/app/shared/utils/siegrain.utils";
import { SGTransition } from "src/app/shared/animations/sg-transition";
import { Title } from "@angular/platform-browser";
import { Store } from "src/app/shared/store/store";
import { constants } from "src/app/shared/constants/siegrain.constants";
import { SGCustomizeTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import {
  SGTransitionMode,
  RouteTransitionCommands,
  CustomizeTransitionCommands,
  TransitionCommands
} from "src/app/shared/animations/sg-transition.model";
import { Subscription } from "rxjs";
import RouteData, { RouteKinds } from "src/app/shared/models/route-data.model";
import { SGTransitionUtil } from "src/app/shared/animations/sg-transition.util";

const StaggerDuration = 200; // 列表总动画时长 = transition duration + stagger duration

@Component({
  selector: "app-article-list",
  templateUrl: "./article-list.component.html",
  styleUrls: ["./article-list.component.scss"]
})
export class ArticleListComponent
  implements OnInit, OnDestroy, SGCustomizeTransitionDelegate {
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

  public currentItemAnimation: string = this.itemAnimations.route;

  private _subscription = new Subscription();
  constructor(
    private _service: ArticleService,
    private _titleService: Title,
    private _transitionUtil: SGTransitionUtil,
    public util: SGUtil,
    public transition: SGTransition,
    public store: Store
  ) {}

  ngOnInit() {
    this._titleService.setTitle(`${constants.titlePlainText}`);
    this._subscription.add(
      this.store.routeDataChanged$.subscribe(async data => {
        this.data = data.list;
        this.restoreTransitionFromLastRouteCommands(data.sg_transition);
      })
    );
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  transitionForComponent?(
    nextRoute: ActivatedRouteSnapshot
  ): TransitionCommands {
    this.setItemTransition(this.itemAnimations.route);
    let data = nextRoute.data as RouteData;
    if (data.kind == RouteKinds.home || data.kind == RouteKinds.edit)
      return new RouteTransitionCommands({ scrollTo: topElementId });
    return null;
  }

  customizeTransitionForComponent(
    nextRoute: ActivatedRouteSnapshot
  ): CustomizeTransitionCommands {
    let index = parseInt(nextRoute.params.index) || 0;
    let isNextPage = index == this.data.nextPageIndex;
    this.setItemTransition(
      isNextPage ? this.itemAnimations.next : this.itemAnimations.previous
    );
    return new CustomizeTransitionCommands({
      names: [this.currentItemAnimation, "page-turn-button"],
      extraDuration: StaggerDuration,
      scrollTo: topElementId
    });
  }

  modeForComponentTransition(
    nextRoute: ActivatedRouteSnapshot
  ): SGTransitionMode {
    let data = nextRoute.data as RouteData;
    return data.kind == RouteKinds.homePaged
      ? SGTransitionMode.custom
      : SGTransitionMode.route;
  }

  public async delete(item: ArticleModel, index: number) {
    let result =
      (await this.util.confirm("Delete", TipType.Danger)) &&
      (await this._service.remove(item.id));
    if (!result) return;

    // TODO: 单动画机制
    // item.animation = new SGAnimation(item.animation);
    // await this.transition.triggerAnimations([item.animation]);
    this.data.list.splice(index, 1);
    // this.setItemTransition(this.itemAnimations.route);
    // await this.transition.triggerAnimations([item.animation]);
  }

  private restoreTransitionFromLastRouteCommands(commands: TransitionCommands) {
    let animationName = this.currentItemAnimation;
    if (commands && this._transitionUtil.isCustomizeCommands(commands))
      animationName = (commands as CustomizeTransitionCommands).names[0];
    this.setItemTransition(animationName);
  }

  /** 设置 article item 的动画 */
  public setItemTransition(name: string) {
    this.currentItemAnimation = name;
    this.data &&
      this.data.list.map(
        x => (x.animation = this._transitionUtil.getAnimation(name))
      );
  }
}
