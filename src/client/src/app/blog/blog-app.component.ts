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
import { Store } from "../shared/store/store";
import RouteData from "../shared/models/route-data.model";
import { slideInAnimation, headerState } from "../shared/utils/animations";
import ArticleModel from "./models/article-model";
import { environment } from "src/environments/environment";

@Component({
  selector: "app-blog-app",
  templateUrl: "./blog-app.component.html",
  styles: [],
  animations: [slideInAnimation]
})
export class BlogAppComponent implements OnInit {
  headerModel: ArticleModel = new ArticleModel();
  isHomePage: boolean = true;
  state: headerState = headerState.Prev;
  kind: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private store: Store
  ) {}

  ngOnInit() {
    this.observeRoute();
    this.onRouteChanged();
  }

  observeRoute() {
    this.store.currentRouteData = this.route.firstChild.snapshot
      .data as RouteData;
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.store.currentRouteData = this.route.firstChild.snapshot
          .data as RouteData;
      });
  }

  onRouteChanged() {
    this.store.routeDataChanged$.subscribe(data => {
      this.kind = data.kind;
      if (data.kind == "home") {
        this.state = headerState.Prev;
        this.headerModel.title = environment.title;
      } else {
        this.state = headerState.Next;
        if (this.store.headerModel) this.headerModel = this.store.headerModel;
      }
    });
  }
}
