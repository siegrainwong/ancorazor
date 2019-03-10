import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import RouteData from "../models/route-data.model";
import { LoggingService } from "../services/logging.service";
import ArticleModel from "src/app/blog/models/article-model";

/**
 * 状态管理
 */
@Injectable({
  providedIn: "root"
})
export class Store {
  constructor(private logger: LoggingService) {}

  /**##### Variables */
  renderFromServer: Boolean = false;
  userLoaded: Boolean = false;
  preloadArticle: ArticleModel;

  /**##### Observables */
  private _routeData: RouteData = new RouteData({ kind: "home" });
  routeDataChanged$ = new BehaviorSubject<RouteData>(this._routeData);

  get routeData() {
    return this._routeData;
  }

  set routeData(value) {
    this._routeData = value;
    this.routeDataChanged$.next(value);
    this.logger.info("route data changed", value);
  }
}
