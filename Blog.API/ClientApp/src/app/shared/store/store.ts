import { Injectable, OnDestroy } from "@angular/core";
import { BehaviorSubject, ReplaySubject } from "rxjs";
import RouteData, { RouteKinds } from "../models/route-data.model";
import { LoggingService } from "../services/logging.service";
import { UserModel } from "src/app/blog/models/user-model";

const UserStoreKey = "sg:user";

/**
 * 状态管理
 */
@Injectable({
  providedIn: "root"
})
export class Store implements OnDestroy {
  constructor(private _logger: LoggingService) {}

  ngOnDestroy(): void {
    this.userChanged$.unsubscribe();
    this.routeDataChanged$.unsubscribe();
  }

  /**
   * === Variables ===
   **/

  /** 是否是客户端渲染 */
  renderFromClient: boolean = false;
  /** 是否是首屏加载 */
  private _isFirstScreen: boolean = true;
  get isFirstScreen(): boolean {
    return this._isFirstScreen && this.renderFromClient;
  }
  set isFirstScreen(val: boolean) {
    this._isFirstScreen = val;
  }

  /** 当前是否正在进行网络请求 */
  isRequesting: boolean = false;

  /**
   * === Methods ===
   **/

  setupUser() {
    if (!this.renderFromClient) return;
    let userJson = window.localStorage.getItem(UserStoreKey);
    if (!userJson) return;
    this.user = JSON.parse(userJson);
    this._logger.info("user restored: ", this.user);
  }

  /**
   * === Observables ===
   **/

  private _user: UserModel;
  userChanged$ = new BehaviorSubject<UserModel>(this._user);

  get userIsAvailable(): boolean {
    return this._user != null;
  }

  get user(): UserModel {
    return this._user;
  }

  set user(user: UserModel) {
    this._user = user;
    if (user) {
      window.localStorage.setItem(UserStoreKey, JSON.stringify(user));
    } else {
      window.localStorage.removeItem(UserStoreKey);
    }

    this.userChanged$.next(user);
  }

  signIn(user: UserModel) {
    this.user = user;
    this._logger.info("user signed in", user);
  }

  signOut() {
    this.user = null;
    this._logger.info("user signed out");
  }

  private _routeData: RouteData = new RouteData({ kind: RouteKinds.home });
  /** An observer for `NavigationEnd` with a `RouteData` */
  routeDataChanged$ = new BehaviorSubject<RouteData>(this._routeData);

  get routeData(): RouteData {
    return this._routeData;
  }

  set routeData(value) {
    this._routeData = value;
    this.routeDataChanged$.next(value);
    this._logger.info("route data changed", value);
  }
}
