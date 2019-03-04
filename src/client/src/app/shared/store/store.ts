import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import RouteData from "../models/route-data.model";
import ArticleModel from "../../blog/models/article-model";
import { LoggingService } from "../services/logging.service";

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
  isLeaving: Boolean = false;

  /**##### Observables */
  private _headerModel: ArticleModel = new ArticleModel();
  headerModelChanged$ = new BehaviorSubject<ArticleModel>(this._headerModel);

  get headerModel() {
    return this._headerModel;
  }

  set headerModel(value) {
    this._headerModel = value;
    this.headerModelChanged$.next(value);
    this.logger.info("header changed: ", value);
  }

  private _routeData: RouteData = new RouteData("home");
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
