/**
 * Mark: ng-cli 创建单ts页面
 * ng g c blog/blog-app --flat --module blog --inline-style --inline-template
 * --flat：前缀式生成
 * --module blog：指定声明模块
 * --inline-style --inline-template：不分开生成scss和html文件，只生成一个ts
 */

import { Component, OnInit, HostBinding } from "@angular/core";
import {
  ActivatedRoute,
  Router,
  NavigationEnd,
  RouterOutlet
} from "@angular/router";
import { filter } from "rxjs/operators";
import { Store } from "../shared/store/store";
import RouteData from "../shared/models/route-data.model";
import { slideInAnimation } from "../shared/utils/animations";
import {
  query,
  style,
  animate,
  trigger,
  transition,
  animateChild
} from "@angular/animations";

const fadeIn = [
  query(
    ":leave",
    style({ position: "absolute", left: 0, right: 0, opacity: 1 })
  ),
  query(
    ":enter",
    style({ position: "absolute", left: 0, right: 0, opacity: 0 })
  ),
  query(":leave", animate("1s", style({ opacity: 0 }))),
  query(":leave", animateChild()),
  query(":enter", animate("1s", style({ opacity: 1 }))),
  query(":enter", animateChild())
];

@Component({
  selector: "app-blog-app",
  templateUrl: "./blog-app.component.html",
  animations: [
    slideInAnimation
    // trigger("routeAnimations", [transition("* => *", fadeIn)])
  ]
})
export class BlogAppComponent implements OnInit {
  @HostBinding("@.disabled")
  isHomePage: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    public store: Store
  ) {}

  ngOnInit() {
    this.observeRoute();
  }

  get kind() {
    return this.store.routeData.kind && "home";
  }

  @HostBinding("@.disabled")
  public animationsDisabled = false;
  prepareRoute(outlet: RouterOutlet) {
    return (
      outlet && outlet.activatedRouteData && outlet.activatedRouteData["kind"]
    );
  }

  observeRoute() {
    this.store.routeData = this.route.firstChild.snapshot.data as RouteData;
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.store.routeData = this.route.firstChild.snapshot.data as RouteData;
      });
  }
}
