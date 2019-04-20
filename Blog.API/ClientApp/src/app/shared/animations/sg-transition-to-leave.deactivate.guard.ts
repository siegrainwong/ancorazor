import { Injectable } from "@angular/core";
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanDeactivate
} from "@angular/router";
import { SGBaseCanDeactivatedGuard } from "../guard/base.deactivate.guard";
import { SGUtil } from "../utils/siegrain.utils";
import { SGTransition } from "./sg-transition";
import {
  CanComponentTransitionToLeave,
  CustomizeTransitionForComponent,
  RouteTransitionForComponent,
  SGTransitionDelegate
} from "./sg-transition.interface";
import {
  SGTransitionMode,
  RouteTransitionCommands,
  TransitionCommands
} from "./sg-transition.model";
import { LoggingService } from "../services/logging.service";
import { BehaviorSubject } from "rxjs/internal/BehaviorSubject";
import { SGTransitionStore } from "./sg-transition.store";

@Injectable({
  providedIn: "root"
})
export class SGTransitionGuard<T extends SGTransitionDelegate>
  implements CanDeactivate<T> {
  constructor(private _transitionStore: SGTransitionStore) {}
  async canDeactivate(
    component: T,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot
  ): Promise<boolean> {
    this._transitionStore.transitionDelegate = component;
    console.log("delegate set with component: ", component);

    return true;
  }
}

/** 过渡管道 */
export const enum SGTransitionPipeline {
  CanDeactivate,
  Resolve,
  CanTransition,
  Transition
}

@Injectable({
  providedIn: "root"
})
export class SGTransitionToLeaveGuard<
  T extends CanComponentTransitionToLeave
> extends SGBaseCanDeactivatedGuard<CanComponentTransitionToLeave> {
  private _location = SGTransitionPipeline.CanDeactivate;
  public transitionPipeline$ = new BehaviorSubject<SGTransitionPipeline>(
    this._location
  );

  constructor(
    private _util: SGUtil,
    private _transition: SGTransition,
    private _logger: LoggingService
  ) {
    super();
  }

  async canDeactivate(
    component: T,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState: RouterStateSnapshot
  ) {
    this.transitionPipeline$.next(this._location);
    return await this.next(component, currentRoute, currentState, nextState);
  }

  /** 执行到管道的下一个位置 */
  private async next(
    component: T,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState: RouterStateSnapshot
  ): Promise<boolean> {
    const url = nextState.url;
    switch (this._location) {
      case SGTransitionPipeline.CanDeactivate:
        let canDeactivate = await super.canDeactivate(
          component,
          currentRoute,
          currentState,
          nextState
        );
        if (!canDeactivate) return canDeactivate;
        break;

      case SGTransitionPipeline.Resolve:
        break;

      case SGTransitionPipeline.CanTransition:
        if (!this.canTransition(component, url)) return true;
        break;

      case SGTransitionPipeline.Transition:
        let delegates = this.setupDelegate(component);
        if (delegates.customDelegate)
          await this.customTransition(delegates.customDelegate, url);
        else await this.routeTransition(delegates.routeDelegate, url);

        this._location = SGTransitionPipeline.CanDeactivate;
        return true;

      default:
        this._logger.error(`unknown state in sg-transition`, this);
        break;
    }

    this._location++;
    this.transitionPipeline$.next(this._location);
    return await this.next(component, currentRoute, currentState, nextState);
  }

  /* 执行路由过渡动画 */
  private async routeTransition(
    delegate: RouteTransitionForComponent,
    url: string
  ) {
    let commands =
      (delegate.RouteTransitionForComponent &&
        delegate.RouteTransitionForComponent(delegate, url)) ||
      new RouteTransitionCommands();

    this.scroll(commands);
    await this._transition.triggerTransition();
  }

  /* 执行自定义过渡动画 */
  private async customTransition(
    delegate: CustomizeTransitionForComponent,
    url: string
  ) {
    const mode = delegate.ModeForComponentTransition(delegate, url);
    if (mode == SGTransitionMode.route)
      return this.routeTransition(delegate, url);

    const commands = delegate.CustomizeTransitionForComponent(delegate, url);
    this.scroll(commands);
    await this._transition.triggerTransition(
      delegate.animationNames,
      commands.extraDuration
    );
  }

  private scroll(commands: TransitionCommands) {
    commands.scrollTo && this._util.scrollTo(commands.scrollTo);
  }

  private canTransition(
    delegate: CanComponentTransitionToLeave,
    url: string
  ): boolean {
    const canLeave = delegate.CanComponentTransitionToLeave;
    let result = canLeave && canLeave(delegate, url);
    return !(
      canLeave &&
      typeof delegate.CanComponentTransitionToLeave === "boolean" &&
      !delegate.CanComponentTransitionToLeave
    );
  }

  /* 根据当前页面实现的接口返回对应的代理 */
  private setupDelegate(
    component: T
  ): {
    customDelegate?: CustomizeTransitionForComponent;
    routeDelegate?: RouteTransitionForComponent;
  } {
    let customized = <CustomizeTransitionForComponent>(<unknown>component);
    if (
      customized.animationNames &&
      customized.ModeForComponentTransition &&
      customized.CustomizeTransitionForComponent
    ) {
      return {
        customDelegate: customized
      };
    }

    return {
      routeDelegate: <RouteTransitionForComponent>(<unknown>component)
    };
  }
}
