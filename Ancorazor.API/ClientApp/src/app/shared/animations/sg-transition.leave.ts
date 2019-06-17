import { Injectable } from "@angular/core";
import {
  SGTransitionCommands,
  SGRouteTransitionCommands,
  SGTransitionMode
} from "./sg-transition.model";
import { first } from "rxjs/operators";
import { SGTransitionPipeline, SGTransitionStore } from "./sg-transition.store";
import { SGTransition, SGTransitionDirection } from "./sg-transition";
import { ActivatedRouteSnapshot } from "@angular/router";
import {
  SGTransitionDelegate,
  SGCustomizeTransitionDelegate
} from "./sg-transition.delegate";
import { LoggingService } from "../services/logging.service";

export declare type SGDelegates = {
  customDelegate?: SGCustomizeTransitionDelegate;
  routeDelegate?: SGTransitionDelegate;
};

/**
 * `SGTransition`离场过渡核心
 */
@Injectable({
  providedIn: "root"
})
export class SGTransitionToLeave {
  constructor(
    private _transitionStore: SGTransitionStore,
    private _logger: LoggingService,
    private _core: SGTransition
  ) {}

  /**
   * 等待其他`Resolve`执行完毕
   * @internal
   **/
  public async _waitingForResolve(resolveCount: number) {
    this.setStream(SGTransitionPipeline.ResolveStart);
    // 等待其他 resolve 执行完毕再执行过渡
    this._logger.info(
      `sg-transition module is waiting for another ${resolveCount} resolvers...`
    );
    await this._transitionStore._resolveCountChanged$
      .pipe(first(x => x == resolveCount))
      .toPromise();
    this.setStream(SGTransitionPipeline.ResolveEnd);
  }

  /**
   * 触发离场过渡
   * @internal
   **/
  public async _transitionToLeave(route: ActivatedRouteSnapshot) {
    this.setStream(SGTransitionPipeline.TransitionLeavingStart);
    // 获取对应代理
    let delegates = this.setupDelegates(
      this._transitionStore._transitionDelegate
    );

    // 获取命令
    let commands = this.commandsFromDelegates(delegates, route);

    // 执行过渡
    this.scrollTo(commands.scrollTo);
    await this._core._triggerAnimations(SGTransitionDirection.leave, commands);

    this.setStream(SGTransitionPipeline.TransitionLeavingEnd);
    return commands;
  }

  private commandsFromDelegates(
    delegates: SGDelegates,
    route: ActivatedRouteSnapshot
  ): SGTransitionCommands {
    let customDelegate = delegates.customDelegate;
    let routeDelegate = delegates.routeDelegate;

    const routeCommandsFromDelegate = delegate => {
      return (
        (delegate.transitionForComponent &&
          delegate.transitionForComponent(route)) ||
        new SGRouteTransitionCommands()
      );
    };

    if (customDelegate) {
      const mode = customDelegate.modeForComponentTransition(route);
      return mode == SGTransitionMode.route
        ? routeCommandsFromDelegate(customDelegate)
        : customDelegate.customizeTransitionForComponent(route);
    } else {
      return routeCommandsFromDelegate(routeDelegate);
    }
  }

  /** 根据当前页面实现的接口返回对应的代理 */
  private setupDelegates(component: SGTransitionDelegate): SGDelegates {
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

  /**
   * 滚动到元素
   * @param elementId
   */
  private scrollTo(elementId: string) {
    elementId &&
      document.querySelector(elementId).scrollIntoView({
        behavior: "smooth",
        block: "start",
        inline: "nearest"
      });
  }

  private setStream(stream: SGTransitionPipeline) {
    this._transitionStore._setTransitionStream(stream);
  }
}
