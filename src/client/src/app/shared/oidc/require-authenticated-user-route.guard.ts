import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { OpenIdConnectService } from './open-id-connect.service';

/**
 * 鉴权模块，相当于.NET中用来鉴权的Attribute
 * 鉴权成功则继续访问
 * 失败就跳转登录
 */
@Injectable({
  providedIn: 'root'
})
export class RequireAuthenticatedUserRouteGuard implements CanActivate {
  constructor(
    private openIdConnectService: OpenIdConnectService) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    if (this.openIdConnectService.userIsAvailable) {
      return true;
    } else {
      // trigger signin
      this.openIdConnectService.triggerSignIn();
      return false;
    }
  }
}
