import { Injectable, OnDestroy } from "@angular/core";
import { timeout } from "../utils/promise-delay";
import { Store } from "../store/store";
import {
  SGTransitionCommands,
  SGCustomizeTransitionCommands,
  SGRouteTransitionCommands,
  SGAnimationData
} from "./sg-transition.model";
import { filter } from "rxjs/operators";
import { SGTransitionUtil } from "./sg-transition.util";
import { SGTransitionPipeline, SGTransitionStore } from "./sg-transition.store";
import { SGTransition, SGTransitionDirection } from "./sg-transition";
import { ObservedServiceBase } from "../components/observed.base";
import { AutoUnsubscribe } from "../utils/auto-unsubscribe.decorator";

/**
 * `SGTransition`入场过渡核心
 */
@Injectable({
  providedIn: "root"
})
@AutoUnsubscribe()
export class SGTransitionToEnter extends ObservedServiceBase
  implements OnDestroy {
  private _currentTransitionsBackup: SGAnimationData;

  private _routeChanged$;
  constructor(
    private _store: Store,
    private _util: SGTransitionUtil,
    private _transitionStore: SGTransitionStore,
    private _core: SGTransition
  ) {
    super();
    this.disableRouteTransitionAtFirstScreen();
    this.subscribeOnComponentEntrance();
  }

  /** 在组件入口（`NavigationEnd`）时触发入场动画 */
  private subscribeOnComponentEntrance() {
    this._routeChanged$ = this._store.routeDataChanged$
      .pipe(filter(_ => !this._store.isFirstScreen))
      .subscribe(async data => {
        this.setStream(SGTransitionPipeline.NavigationEnd);
        await this.transitionToEnter(data.sg_transition);
        this.setStream(SGTransitionPipeline.Complete);
      });
  }

  /**
   * 触发入场过渡
   * @param previousCommands 上一个路由的过渡指令
   */
  private async transitionToEnter(previousCommands?: SGTransitionCommands) {
    this.setStream(SGTransitionPipeline.TransitionEnteringStart);

    const shouldCrossRoute =
      previousCommands &&
      previousCommands.crossRoute &&
      this._transitionStore._isTransitionCrossedRoute;

    let commands = null;
    if (shouldCrossRoute)
      commands = this.replaceWithPreviousTransitions(previousCommands);
    else commands = previousCommands || new SGRouteTransitionCommands();

    await this._core._triggerAnimations(SGTransitionDirection.enter, commands);

    if (shouldCrossRoute) this.restoreCurrentTransitions();
    this.setStream(SGTransitionPipeline.TransiitonEnteringEnd);
  }

  /**
   * 用上一个路由的过渡替换当前路由过渡
   * @param previousCommands 上一个路由的过渡指令
   */
  private replaceWithPreviousTransitions(
    previousCommands: SGTransitionCommands
  ): SGCustomizeTransitionCommands {
    let delegate = this._transitionStore._nextTransitionDelegate;
    let previousAnimations = previousCommands["animations"];

    this._currentTransitionsBackup = this._util.deepCopy(delegate.animations);

    this.replaceAnimations(delegate.animations, previousAnimations);

    return new SGCustomizeTransitionCommands({
      animations: delegate.animations,
      crossRoute: false,
      scrollTo: previousCommands.scrollTo,
      extraDuration: previousCommands["extraDuration"] || 0
    });
  }

  /**
   * 恢复当前路由的过渡
   */
  private restoreCurrentTransitions() {
    let delegate = this._transitionStore._nextTransitionDelegate;
    this.replaceAnimations(delegate.animations, this._currentTransitionsBackup);
    this._currentTransitionsBackup = null;
  }

  /**
   * 替换动画
   * 仅替换样式跟速度，保留对象引用
   */
  private replaceAnimations(source: SGAnimationData, target: SGAnimationData) {
    Object.entries(source).map(x => {
      const name = x[0];
      const animation = x[1];
      const replacedBy = target[name];
      if (!replacedBy) return x;
      animation.enterClass = replacedBy.enterClass;
      animation.leaveClass = replacedBy.leaveClass;
      animation.speed = replacedBy.speed;
    });
  }

  /**
   * === Private Methods ===
   */

  /** 禁用首屏路由过渡 */
  private async disableRouteTransitionAtFirstScreen() {
    if (!this._store.renderFromClient) return;
    await timeout(this._util.getDuration(this._util.getRouteAnimations));
    this._store.isFirstScreen = false;
  }

  private setStream(stream: SGTransitionPipeline) {
    this._transitionStore._setTransitionStream(stream);
  }
}
