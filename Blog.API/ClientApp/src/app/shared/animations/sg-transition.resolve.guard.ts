import { Injectable } from "@angular/core";
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from "@angular/router";
import { SGTransitionStore } from "./sg-transition.store";
import { first } from "rxjs/operators";
import {
  SGCustomizeTransitionDelegate,
  SGTransitionDelegate
} from "./sg-transition.delegate";
import {
  RouteTransitionCommands,
  SGTransitionMode,
  TransitionCommands
} from "./sg-transition.model";
import { SGTransition } from "./sg-transition";
import { SGUtil } from "../utils/siegrain.utils";
import { Store } from "../store/store";
import { LoggingService } from "../services/logging.service";

@Injectable({
  providedIn: "root"
})
export class SGTransitionResolveGuard
  implements Resolve<RouteTransitionCommands> {
  constructor(
    private _transitionStore: SGTransitionStore,
    private _transition: SGTransition,
    private _util: SGUtil,
    private _store: Store,
    private _logger: LoggingService
  ) {}

  async resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<RouteTransitionCommands> {
    // 首屏及 SSR 时不执行过渡
    if (this._store.isFirstScreen || !this._store.renderFromClient) return null;

    // 获取路由上其他resolve的数量
    const resolveCount = this.getResolveCount(route);

    // 没有其他 resolve 就直接执行过渡
    if (!resolveCount) {
      this._logger.info("nothing resolved before transition to ", route);
      return await this.transition(route);
    }

    // 等待其他 resolve 执行完毕再执行过渡
    this._logger.info(
      `sg-transition module is waiting for another ${resolveCount} resolvers...`
    );
    await this._transitionStore.resolveCompletedChanged$
      .pipe(first(x => x == resolveCount))
      .toPromise();
    this._logger.info("resolves completed, transition to ", route);
    return await this.transition(route);
  }

  private async transition(route: ActivatedRouteSnapshot) {
    let delegates = this.setupDelegate(
      this._transitionStore.transitionDelegate
    );
    let commands = null;
    if (delegates.customDelegate)
      commands = await this.transitionWithCustomMode(
        delegates.customDelegate,
        route
      );
    else
      commands = await this.transitionWithRouteMode(
        delegates.routeDelegate,
        route
      );
    this._transitionStore.setTransitioned();
    return commands;
  }

  /* 执行路由过渡 */
  private async transitionWithRouteMode(
    delegate: SGTransitionDelegate,
    route: ActivatedRouteSnapshot
  ) {
    let commands =
      (delegate.TransitionForComponent &&
        delegate.TransitionForComponent(route)) ||
      new RouteTransitionCommands();

    this.scroll(commands);
    await this._transition.triggerLeaveTransition();
    return commands;
  }

  /* 执行自定义过渡 */
  private async transitionWithCustomMode(
    delegate: SGCustomizeTransitionDelegate,
    route: ActivatedRouteSnapshot
  ) {
    const mode = delegate.ModeForComponentTransition(route);
    if (mode == SGTransitionMode.route)
      return this.transitionWithRouteMode(delegate, route);

    const commands = delegate.CustomizeTransitionForComponent(route);
    this.scroll(commands);
    await this._transition.triggerLeaveTransition(
      commands.names,
      commands.extraDuration
    );
    return commands;
  }

  private scroll(commands: TransitionCommands) {
    commands.scrollTo && this._util.scrollTo(commands.scrollTo);
  }

  /* 根据当前页面实现的接口返回对应的代理 */
  private setupDelegate(
    component: SGTransitionDelegate
  ): {
    customDelegate?: SGCustomizeTransitionDelegate;
    routeDelegate?: SGTransitionDelegate;
  } {
    let customized = <SGCustomizeTransitionDelegate>(<unknown>component);
    if (
      customized.ModeForComponentTransition &&
      customized.CustomizeTransitionForComponent
    ) {
      return {
        customDelegate: customized
      };
    }

    return {
      routeDelegate: component
    };
  }

  /* 获取其他Resolve的数量 */
  private getResolveCount(route: ActivatedRouteSnapshot): number {
    const resolveKey = "_resolve";
    return (
      (route[resolveKey] && Object.keys(route[resolveKey]).length - 1) || 0
    );
  }
}
