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
  renderFromServer: Boolean = false;

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
