import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from "@angular/router";
import { Observable } from "rxjs";
import { OpenIdConnectService } from "./open-id-connect.service";
import { Store } from "../store/store";
import { LoggingService } from "../services/logging.service";

/**
 * 鉴权模块，相当于.NET中用来鉴权的Attribute
 * 鉴权成功则继续访问
 * 失败就跳转登录
 */
@Injectable({
  providedIn: "root"
})
export class RequireAuthenticatedUserRouteGuard implements CanActivate {
  constructor(
    private openIdConnectService: OpenIdConnectService,
    private store: Store,
    private router: Router,
    private logger: LoggingService
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    if (this.store.renderFromServer) {
      /**
       * 这个地方不能 navigate，会直接跳到 server 渲染的内容，angular 都不会加载
       * resolve false，return false 都会导致 prerender timeout
       * 所以只能 resolve true 或者 return true
       */
      this.logger.info("authentication not support for SSR.");
      return true;
    }

    // 如果 guarded 的页面是首屏加载，这里第一时间就没有加载到用户
    this.logger.info("did user load in guard: ", this.store.userLoaded);
    if (this.store.userLoaded) {
      if (this.openIdConnectService.userIsAvailable) {
        return true;
      } else {
        this.openIdConnectService.triggerSignIn();
        return false;
      }
    } else {
      return new Promise((rsv, rjc) => {
        this.openIdConnectService.getUser().then(user => {
          if (!user) {
            this.openIdConnectService.triggerSignIn();
            this.logger.info("authn guard: rejected");
            rjc(false);
          }
          this.logger.info("authn guard: resolved");
          rsv(true);
        });
      });
    }
  }
}
