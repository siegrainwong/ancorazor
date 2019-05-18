import { ScrollDispatcher } from "@angular/cdk/scrolling";
import {
  Component,
  OnInit,
  OnDestroy,
  AfterContentChecked
} from "@angular/core";
import { ActivatedRoute, Router, NavigationEnd } from "@angular/router";
import { filter } from "rxjs/operators";
import { Store } from "../shared/store/store";
import RouteData from "../shared/models/route-data.model";
import { SGTransitionToEnter } from "../shared/animations/sg-transition.enter";
import { SGUtil } from "../shared/utils/siegrain.utils";
import { externalScripts } from "../shared/constants/siegrain.constants";
import { onScroll } from "../shared/utils/scroll-listener";
import { UserService } from "./services/user.service";
import { CommonService } from "./services/common.service";
import { SEOService } from "./services/seo.service";
import { AutoUnsubscribe } from "../shared/utils/auto-unsubscribe.decorator";
import { ObservedComponentBase } from "../shared/components/observed.base";

@Component({
  selector: "app-blog-app",
  templateUrl: "./blog-app.component.html"
})
@AutoUnsubscribe()
export class BlogAppComponent extends ObservedComponentBase
  implements OnInit, OnDestroy {
  private _userChanged$;
  private _routeChanged$;
  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _util: SGUtil,
    private _scrollDispatcher: ScrollDispatcher,
    private _userService: UserService,
    private _commonService: CommonService,
    private _seoService: SEOService,
    public store: Store,
    public transition: SGTransitionToEnter
  ) {
    super();
  }

  ngOnInit() {
    this.subscribeRoute();
    this.setupSetting();
    this._seoService.setup();
    if (!this.store.renderFromClient) return;
    this.setupUser();
    this._scrollDispatcher.scrolled().subscribe(onScroll);
    this.loadExternalResources();
  }

  private setupUser() {
    this.store.setupUser();
    this._userChanged$ = this.store.userChanged$.subscribe(() => {
      this._userService.getXSRFToken();
    });
  }

  async setupSetting() {
    this.store.siteSetting = await this._commonService.getSetting();
  }

  private loadExternalResources() {
    this._util.loadExternalScripts(Object.values(externalScripts));
  }

  private subscribeRoute() {
    this.store.routeData = this._route.firstChild.snapshot.data as RouteData;
    this._routeChanged$ = this._router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(async event => {
        console.log(event);

        this.store.routeData = this._route.firstChild.snapshot
          .data as RouteData;
      });
  }
}
