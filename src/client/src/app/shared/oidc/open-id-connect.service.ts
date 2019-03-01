import { Injectable } from "@angular/core";
import { UserManager, User } from "src/libraries/oidc-client-js-dev";
// import { UserManager, User } from "oidc-client";
import { environment } from "src/environments/environment";
import { ReplaySubject } from "rxjs";
import { Store } from "../store/store";

/**
 * oidc连接模块，负责管理用户的登录注销状态
 */
@Injectable({
  providedIn: "root"
})
export class OpenIdConnectService {
  private userManager: UserManager = null;

  private currentUser: User;

  // TODO: 这个是rxjs的广播者，但这个1是几个意思要自己下来了解一下
  userLoaded$ = new ReplaySubject<boolean>(1);

  get userIsAvailable(): boolean {
    return this.currentUser != null;
  }

  get user(): User {
    return this.currentUser;
  }

  constructor(private store: Store) {
    console.log("oidc ctor.");

    // SSR 时不允许创建 userManager 实例
    if (store.renderFromServer) return;

    this.userManager = new UserManager(environment.openIdConnectSettings);

    /**
     * TODO: 之前有请求在过程中没有执行完毕，需要调用这个方法清除一下之前的执行状态
     * 不是很懂，需要测试
     */
    this.userManager.clearStaleState();

    /**
     * Mark: 实现用户的登录和注销事件
     */
    this.userManager.events.addUserLoaded(user => {
      if (!environment.production) {
        console.log("User loaded.", user);
      }
      this.currentUser = user;
      // 广播给订阅者 用户登录的状态
      this.userLoaded$.next(true);
    });

    this.userManager.events.addUserUnloaded(e => {
      if (!environment.production) {
        console.log("User unloaded");
      }
      this.currentUser = null;
      // 广播给订阅者 用户注销的状态
      this.userLoaded$.next(false);
    });

    /**
     * 加载用户状态
     */
    this.userManager
      .getUser()
      .then(user => {
        this.currentUser = user;
      })
      .finally(() => {
        this.store.userLoaded = true;
      });
  }

  getUser(): Promise<User> {
    return this.userManager.getUser();
  }

  async triggerSignIn() {
    let user = await this.userManager.getUser();
    if (!user) {
      await this.userManager.signinRedirect();
      if (!environment.production) console.log("Redirect to signin triggered.");
    }
  }

  async triggerSignOut() {
    let resp = await this.userManager.signoutRedirect();
    if (!environment.production)
      console.log("Redirect to sign out triggered.", resp);
  }

  /**
   * 在登录操作结束后，不论结果
   */
  async handleCallback() {
    let user = await this.userManager.signinRedirectCallback();
    if (!environment.production)
      console.log("Callback after signin handled.", user);
  }

  /**
   * 在静默登录操作结束后
   */
  async handleSilentCallback() {
    let user = await this.userManager.signinSilentCallback();
    this.currentUser = user;
    if (!environment.production)
      // TODO: 很奇怪这个地方user为undefined。。。
      console.log("Silent renew handled.", user);
  }
}
