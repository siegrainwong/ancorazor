import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import RouteData from "./models/route-data.model";

/**
 * Injectable root 代表一个单例
 */
@Injectable({
  providedIn: "root"
})
export class Variables {
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

  private routeData: RouteData = new RouteData("home");
  routeDataChanged$ = new BehaviorSubject<RouteData>(this.routeData);

  constructor() {}

  get currentRouteData() {
    return this.routeData;
  }

  set currentRouteData(value) {
    this.routeData = value;
    this.routeDataChanged$.next(value);
    console.log(value);
  }
}
