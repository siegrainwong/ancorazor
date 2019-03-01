import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import RouteData from "../models/route-data.model";
import ArticleModel from "../../blog/models/article-model";

/**
 * 状态管理
 */
@Injectable({
  providedIn: "root"
})
export class Store {
  constructor() {}

  /**##### Variables */
  /**
   * 当前是否是服务器渲染
   */
  renderFromServer: Boolean = false;
  /**
   * 用户状态是否已加载
   */
  userLoaded: Boolean = false;
  /**
   * 首页封面
   */
  homeCoverLoaded: boolean = false;

  /**##### Observables */
  /**
   * 首页到文章跳转用
   */
  headerModel?: ArticleModel;

  private routeData: RouteData = new RouteData("home");
  routeDataChanged$ = new BehaviorSubject<RouteData>(this.routeData);

  get currentRouteData() {
    return this.routeData;
  }

  set currentRouteData(value) {
    this.routeData = value;
    this.routeDataChanged$.next(value);
    console.log(value);
  }
}
