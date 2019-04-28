import { Injectable } from "@angular/core";
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from "@angular/router";
import { SGTransitionStore } from "./sg-transition.store";
import { SGTransitionDelegate } from "./sg-transition.delegate";
import { SGTransitionCommands } from "./sg-transition.model";
import { SGTransitionToLeave } from "./sg-transition.leave";

@Injectable({
  providedIn: "root"
})
export class SGTransitionResolveGuard implements Resolve<SGTransitionCommands> {
  constructor(
    private _transitionStore: SGTransitionStore,
    private _core: SGTransitionToLeave
  ) {}

  async resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<SGTransitionCommands> {
    if (!this._transitionStore._isLeaveTransitionAvailable) return null;

    this.setupStatesForStore(route);

    const resolveCount = this.getResolveCount(route);
    resolveCount && (await this._core._waitingForResolve(resolveCount));
    return await this._core._transitionToLeave(route);
  }

  /** 获取其他Resolve的数量 */
  private getResolveCount(route: ActivatedRouteSnapshot): number {
    const resolveKey = "_resolve";
    return (
      (route[resolveKey] && Object.keys(route[resolveKey]).length - 1) || 0
    );
  }

  private setupStatesForStore(route: ActivatedRouteSnapshot) {
    this._transitionStore._nextRouteConfig = route.routeConfig.path;
    this._transitionStore._nextTransitionDelegate = <SGTransitionDelegate>(
      (<unknown>{ animations: this.animationsFromNextComponent(route) })
    );
  }

  private animationsFromNextComponent(route: ActivatedRouteSnapshot) {
    try {
      if (!route.component) return null;
      let proto = route.component["prototype"];
      // 手动调用构造器初始化字段
      proto.constructor();
      return proto.animations;
    } catch (error) {
      return null;
    }
  }
}
