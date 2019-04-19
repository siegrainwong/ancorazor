import { ScrollDispatcher } from "@angular/cdk/scrolling";
import { Component, OnInit, OnDestroy } from "@angular/core";
import {
  ActivatedRoute,
  Router,
  NavigationEnd,
  NavigationStart
} from "@angular/router";
import { filter } from "rxjs/operators";
import { Store } from "../shared/store/store";
import RouteData from "../shared/models/route-data.model";
import { SGTransition } from "../shared/animations/sg-transition";
import { SGUtil } from "../shared/utils/siegrain.utils";
import { externalScripts } from "../shared/constants/siegrain.constants";
import { onScroll } from "../shared/utils/scroll-listener";
import { Subscription } from "rxjs";
import { UserService } from "./services/user.service";

@Component({
  selector: "app-blog-app",
  templateUrl: "./blog-app.component.html"
})
export class BlogAppComponent implements OnInit, OnDestroy {
  /**
   * Mark: 取消订阅以释放内存
   * https://blog.angularindepth.com/the-best-way-to-unsubscribe-rxjs-observable-in-the-angular-applications-d8f9aa42f6a0
   */
  private _subscription = new Subscription();

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _util: SGUtil,
    private _scrollDispatcher: ScrollDispatcher,
    private _userService: UserService,
    public store: Store,
    public transition: SGTransition
  ) {}

  ngOnInit() {
    this.subscribeRoute();
    if (!this.store.renderFromClient) return;
    this.setupUser();
    this._scrollDispatcher.scrolled().subscribe(onScroll);
    this.loadExternalResources();
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  private setupUser() {
    this.store.setupUser();
    this._subscription.add(
      this.store.userChanged$.subscribe(() => {
        this._userService.getXSRFToken();
      })
    );
  }

  private loadExternalResources() {
    this._util.loadExternalScripts(Object.values(externalScripts));
  }

  private subscribeRoute() {
    this.store.routeData = this._route.firstChild.snapshot.data as RouteData;
    this._subscription.add(
      this._router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe(async () => {
          this.store.routeData = this._route.firstChild.snapshot
            .data as RouteData;
        })
    );
    this._subscription.add(
      this._router.events
        .pipe(filter(event => event instanceof NavigationStart))
        .subscribe(event => this.store.routeWillBegin$.next())
    );
  }
}
