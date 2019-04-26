import { Injectable } from "@angular/core";
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from "@angular/router";
import { SGTransitionStore, SGTransitionPipeline } from "./sg-transition.store";
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
import { SGTransition, SGTransitionDirection } from "./sg-transition";
import { LoggingService } from "../services/logging.service";

@Injectable({
  providedIn: "root"
})
export class SGTransitionResolveGuard implements Resolve<TransitionCommands> {
  constructor(
    private _transitionStore: SGTransitionStore,
    private _transition: SGTransition,
    private _logger: LoggingService
  ) {}

  async resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<TransitionCommands> {
    if (!this._transitionStore._isLeaveTransitionAvailable) return null;

    const transition = async () => {
      this.setStream(SGTransitionPipeline.TransitionLeavingStart);
      let commands = await this.transition(route);
      this.setStream(SGTransitionPipeline.TransitionLeavingEnd);
      this._transitionStore._nextTransitionDelegate = <SGTransitionDelegate>(
        (<unknown>{ animations: this.animationsFromNextRoute(route) })
      );
      return commands;
    };

    // 获取路由上其他resolve的数量
    const resolveCount = this.getResolveCount(route);

    // 没有其他 resolve 就直接执行过渡
    if (!resolveCount) {
      this._logger.info("nothing resolved before transition to ", route);
      return await transition();
    }

    this.setStream(SGTransitionPipeline.ResolveStart);
    // 等待其他 resolve 执行完毕再执行过渡
    this._logger.info(
      `sg-transition module is waiting for another ${resolveCount} resolvers...`
    );
    await this._transitionStore._resolveCountChanged$
      .pipe(first(x => x == resolveCount))
      .toPromise();
    this._logger.info("resolves completed, transition to ", route);
    this.setStream(SGTransitionPipeline.ResolveEnd);

    return await transition();
  }

  private async transition(route: ActivatedRouteSnapshot) {
    // 获取对应代理
    let delegates = this.setupDelegate(
      this._transitionStore._transitionDelegate
    );

    // 获取命令
    let commands: TransitionCommands = null;
    if (delegates.customDelegate)
      commands = this.customizeCommandsFromDelegate(
        delegates.customDelegate,
        route
      );
    else
      commands = this.routeCommandsFromDelegate(delegates.routeDelegate, route);

    // 执行过渡
    await this.transitionToLeave(commands);

    return commands;
  }

  /**
   * 触发离场过渡
   * @param names 自定义动画集合（不传则触发当前页面所有路由动画）
   * @param extraDuration 额外过渡时间
   * 自定义动画时间内会禁用路由动画
   */
  private async transitionToLeave(commands: TransitionCommands) {
    await this._transition._triggerAnimations(
      SGTransitionDirection.leave,
      commands
    );
  }

  /** 从代理获取路由过渡命令 */
  private routeCommandsFromDelegate(
    delegate: SGTransitionDelegate,
    route: ActivatedRouteSnapshot
  ) {
    return (
      (delegate.transitionForComponent &&
        delegate.transitionForComponent(route)) ||
      new RouteTransitionCommands()
    );
  }

  /** 从代理获取自定义过渡命令 */
  private customizeCommandsFromDelegate(
    delegate: SGCustomizeTransitionDelegate,
    route: ActivatedRouteSnapshot
  ) {
    const mode = delegate.modeForComponentTransition(route);
    if (mode == SGTransitionMode.route)
      return this.routeCommandsFromDelegate(delegate, route);
    return delegate.customizeTransitionForComponent(route);
  }

  /** 根据当前页面实现的接口返回对应的代理 */
  private setupDelegate(
    component: SGTransitionDelegate
  ): {
    customDelegate?: SGCustomizeTransitionDelegate;
    routeDelegate?: SGTransitionDelegate;
  } {
    let customized = <SGCustomizeTransitionDelegate>(<unknown>component);
    if (
      customized.modeForComponentTransition &&
      customized.customizeTransitionForComponent
    ) {
      return {
        customDelegate: customized
      };
    }

    return {
      routeDelegate: component
    };
  }

  /** 获取其他Resolve的数量 */
  private getResolveCount(route: ActivatedRouteSnapshot): number {
    const resolveKey = "_resolve";
    return (
      (route[resolveKey] && Object.keys(route[resolveKey]).length - 1) || 0
    );
  }

  private animationsFromNextRoute(route: ActivatedRouteSnapshot) {
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

  private setStream(stream: SGTransitionPipeline) {
    this._transitionStore._setTransitionStream(stream);
  }
}
