import { Injectable, OnDestroy } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import RouteData, { RouteKinds } from "../models/route-data.model";
import { LoggingService } from "../services/logging.service";
import { UserModel } from "src/app/blog/models/user-model";
import SiteSettingModel from "src/app/blog/models/site-setting.model";

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
    this.siteSettingChanged$.unsubscribe();
  }

  /**
   * === Variables ===
   **/

  /** 是否是客户端渲染 */
  public renderFromClient: boolean = false;
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

  public setupUser() {
    if (!this.renderFromClient) return;
    let userJson = window.localStorage.getItem(UserStoreKey);
    if (!userJson) return;
    this.user = JSON.parse(userJson);
    this._logger.info("user restored: ", this.user);
  }

  /**
   * === Observables ===
   **/

  // === user
  private _user: UserModel;
  public userChanged$ = new BehaviorSubject<UserModel>(this._user);
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
  public signIn(user: UserModel) {
    this.user = user;
    this._logger.info("user signed in", user);
  }
  public signOut() {
    this.user = null;
    this._logger.info("user signed out");
  }

  // === route data
  private _routeData: RouteData = new RouteData({ kind: RouteKinds.home });
  /** An observer for `NavigationEnd` with a `RouteData` */
  public routeDataChanged$ = new BehaviorSubject<RouteData>(this._routeData);
  get routeData(): RouteData {
    return this._routeData;
  }
  set routeData(value) {
    this._routeData = value;
    this.routeDataChanged$.next(value);
    this._logger.info("route data changed", value);
  }

  // === site setting
  private _siteSetting = new SiteSettingModel();
  public siteSettingChanged$ = new BehaviorSubject<SiteSettingModel>(
    this._siteSetting
  );
  get siteSetting() {
    return this._siteSetting;
  }
  set siteSetting(val) {
    this._siteSetting = val;
    this.siteSettingChanged$.next(val);
  }
}
