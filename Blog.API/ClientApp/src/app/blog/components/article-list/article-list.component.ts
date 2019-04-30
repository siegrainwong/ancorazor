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
import { SGAnimations } from "src/app/shared/animations/sg-animations";
import { Title } from "@angular/platform-browser";
import { Store } from "src/app/shared/store/store";
import { constants } from "src/app/shared/constants/siegrain.constants";
import { SGCustomizeTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import {
  SGTransitionMode,
  SGRouteTransitionCommands,
  SGCustomizeTransitionCommands,
  SGTransitionCommands
} from "src/app/shared/animations/sg-transition.model";
import { Subscription } from "rxjs";
import RouteData, { RouteKinds } from "src/app/shared/models/route-data.model";
import { SGTransitionStore } from "src/app/shared/animations/sg-transition.store";

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

  public animations = {
    articles: SGAnimations.fadeOpposite,
    pagination: SGAnimations.pageTurnButton
  };

  private _subscription = new Subscription();
  constructor(
    private _service: ArticleService,
    private _titleService: Title,
    private _util: SGUtil,
    public transitionStore: SGTransitionStore,
    public store: Store
  ) {}

  ngOnInit() {
    this._titleService.setTitle(`${constants.titlePlainText}`);
    this._subscription.add(
      this.store.routeDataChanged$.subscribe(async data => {
        this.data = data.list;
      })
    );
  }

  ngOnDestroy() {
    console.log(ArticleListComponent.name + " destroyed");
    this._subscription.unsubscribe();
  }

  transitionForComponent?(
    nextRoute: ActivatedRouteSnapshot
  ): SGTransitionCommands {
    this.animations.articles = SGAnimations.fadeOpposite;
    return new SGRouteTransitionCommands({ scrollTo: topElementId });
  }

  customizeTransitionForComponent(
    nextRoute: ActivatedRouteSnapshot
  ): SGCustomizeTransitionCommands {
    let index = parseInt(nextRoute.params.index) || 0;
    let isNextPage = index == this.data.nextPageIndex;
    this.animations.articles = isNextPage
      ? SGAnimations.pageTurnNext
      : SGAnimations.pageTurnPrevious;

    return new SGCustomizeTransitionCommands({
      crossRoute: true,
      animations: this.animations,
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
      (await this._util.confirm("Delete", TipType.Danger)) &&
      (await this._service.remove(item.id));
    if (!result) return;

    // TODO: 单动画机制
    this.data.list.splice(index, 1);
  }
}
