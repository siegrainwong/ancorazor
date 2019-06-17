import { Injectable } from "@angular/core";
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  Router,
  Route
} from "@angular/router";
import { SGTransitionStore } from "./sg-transition.store";
import { SGTransitionDelegate } from "./sg-transition.delegate";
import { SGTransitionCommands } from "./sg-transition.model";
import { SGTransitionToLeave } from "./sg-transition.leave";
import { of, Observable } from "rxjs";
import { take } from "rxjs/operators";
import ArticleModel from "src/app/blog/models/article-model";
import { timeout } from "../utils/promise-delay";

@Injectable({
  providedIn: "root"
})
@TransitionGuard()
export class SGTransitionResolveGuard implements Resolve<SGTransitionCommands> {
  private _anotherResolveGuards = new Set<any>();
  constructor(
    private _transitionStore: SGTransitionStore,
    private _core: SGTransitionToLeave,
    private _router: Router
  ) {
    this.setupAnotherResolveGuards(this._router.config);
    this.attachNotification();
  }

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

  /**
   * 替换其他的`Resolve`方法
   *
   * 在`Resolve`后调用`_setResolved`方法
   * 以实现在其他所有`Resolve`回调完毕后执行离场过渡的效果
   */
  private attachNotification() {
    const self = this;
    this._anotherResolveGuards.forEach(guard => {
      let resolve = guard.prototype.resolve;
      // 这里不要用 lambda 方法，会丢掉 scope
      guard.prototype.resolve = async function(route, state) {
        let res = await resolve.call(this, route, state);
        if (res instanceof Observable)
          res = await (res as Observable<any>).pipe(take(1)).toPromise();

        self._transitionStore._setResolved();
        return res;
      };
    });
  }

  /**
   * 将所有包含`SGTransitionResolveGuard`的路由设置的其他`ResolveGuard`找出来
   */
  private setupAnotherResolveGuards(current: Route[]) {
    if (!current) return;
    current.forEach(x => {
      let childs =
        (x["_loadedConfig"] && x["_loadedConfig"].routes) || x.children;
      let resolves = x.resolve && Object.values(x.resolve);
      resolves &&
        resolves
          .filter(y => !y.prototype.isTransitionGuard)
          .forEach(y => this._anotherResolveGuards.add(y));

      this.setupAnotherResolveGuards(childs);
    });
  }

  /** 获取其他`Resolve`的数量 */
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

// Testing Guards
@Injectable({
  providedIn: "root"
})
export class TestObservableResolveGuard implements Resolve<any> {
  async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    await timeout(3000);
    return of(new ArticleModel());
  }
}

@Injectable({
  providedIn: "root"
})
export class TestDirectResolveGuard implements Resolve<ArticleModel> {
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return new ArticleModel();
  }
}

export function TransitionGuard(): ClassDecorator {
  return function(target: any) {
    Object.defineProperty(target.prototype, "isTransitionGuard", {
      value: () => true
    });
  };
}
