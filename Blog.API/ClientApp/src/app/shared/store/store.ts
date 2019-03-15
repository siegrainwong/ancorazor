import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import RouteData from "../models/route-data.model";
import { LoggingService } from "../services/logging.service";
import ArticleModel from "src/app/blog/models/article-model";
import { UserModel } from "src/app/blog/models/user-model";

const UserStoreKey = "sg:user";

/**
 * 状态管理
 */
@Injectable({
  providedIn: "root"
})
export class Store {
  constructor(private logger: LoggingService) {}

  /**##### Methods */
  setupUser() {
    if (!this.renderFromClient) return;
    let userJson = window.localStorage.getItem(UserStoreKey);
    if (!userJson) return;
    this.user = JSON.parse(userJson);
  }

  /**##### Variables */
  renderFromClient: Boolean = false;
  userLoaded: Boolean = false;
  preloadArticle: ArticleModel;

  /**##### Observables */
  private _user: UserModel;
  userChanged$ = new BehaviorSubject<UserModel>(this._user);

  get user() {
    return this._user;
  }

  set user(value: UserModel) {
    this._user = value;
    if (value && value.token) {
      window.localStorage.setItem(UserStoreKey, JSON.stringify(value));
    } else {
      window.localStorage.setItem(UserStoreKey, null);
    }

    this.userChanged$.next(value);
    this.logger.info("user data changed", value);
  }

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
