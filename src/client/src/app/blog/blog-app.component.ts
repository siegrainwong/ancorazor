/**
 * Mark: ng-cli 创建单ts页面
 * ng g c blog/blog-app --flat --module blog --inline-style --inline-template
 * --flat：前缀式生成
 * --module blog：指定声明模块
 * --inline-style --inline-template：不分开生成scss和html文件，只生成一个ts
 */

import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router, NavigationEnd } from "@angular/router";
import { filter } from "rxjs/operators";
import { Variables } from "../shared/variables";
import RouteData from "../shared/models/route-data.model";

@Component({
  selector: "app-blog-app",
  templateUrl: "./blog-app.component.html",
  styles: []
})
export class BlogAppComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private variables: Variables
  ) {}

  ngOnInit() {
    this.observeRoute();
  }

  observeRoute() {
    this.variables.currentRouteData = this.route.firstChild.snapshot
      .data as RouteData;
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.variables.currentRouteData = this.route.firstChild.snapshot
          .data as RouteData;
      });
  }
}
