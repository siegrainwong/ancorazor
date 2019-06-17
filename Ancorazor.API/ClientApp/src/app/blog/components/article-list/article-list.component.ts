import {
  Component,
  OnInit,
  OnDestroy,
  AfterContentChecked,
  AfterViewInit
} from "@angular/core";
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
import { Store } from "src/app/shared/store/store";
import { SGCustomizeTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import {
  SGTransitionMode,
  SGRouteTransitionCommands,
  SGCustomizeTransitionCommands,
  SGTransitionCommands,
  SGAnimation
} from "src/app/shared/animations/sg-transition.model";
import RouteData, { RouteKinds } from "src/app/shared/models/route-data.model";
import { SGTransitionStore } from "src/app/shared/animations/sg-transition.store";
import { ObservedComponentBase } from "src/app/shared/components/observed.base";
import { AutoUnsubscribe } from "src/app/shared/utils/auto-unsubscribe.decorator";

const StaggerDuration = 200; // 列表总动画时长 = transition duration + stagger duration

@Component({
  selector: "app-article-list",
  templateUrl: "./article-list.component.html",
  styleUrls: ["./article-list.component.scss"]
})
@AutoUnsubscribe()
export class ArticleListComponent extends ObservedComponentBase
  implements OnInit, OnDestroy, SGCustomizeTransitionDelegate {
  // header
  public headerModel: ArticleModel;
  // request
  public data: PagedResult<ArticleModel>;
  public preloading: boolean = false;
  // observables
  private _routeChanged$;
  private _settingChanged$;

  public animations = {
    articles: SGAnimations.fadeOpposite,
    pagination: SGAnimations.pageTurnButton
  };

  constructor(
    private _service: ArticleService,
    private _util: SGUtil,
    public transitionStore: SGTransitionStore,
    public store: Store
  ) {
    super();
  }

  ngOnInit() {
    this._routeChanged$ = this.store.routeDataChanged$.subscribe(data => {
      this.data = data.list;
    });
    this._settingChanged$ = this.store.siteSettingChanged$.subscribe(data => {
      this.headerModel = new ArticleModel({
        title: data.title,
        cover: data.coverUrl
      });
    });
  }

  transitionForComponent?(
    nextRoute: ActivatedRouteSnapshot
  ): SGTransitionCommands {
    this.animations.articles = SGAnimations.fadeOpposite;
    this.animations.pagination = SGAnimations.fadeOpposite;
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
    this.animations.pagination = SGAnimations.pageTurnButton;

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
