import { Injectable, OnDestroy } from "@angular/core";
import { timeout } from "../utils/promise-delay";
import { Store } from "../store/store";
import { Subscription } from "rxjs";
import {
  SGAnimation,
  TransitionCommands,
  CustomizeTransitionCommands,
  RouteTransitionCommands,
  SGAnimationData
} from "./sg-transition.model";
import { filter } from "rxjs/operators";
import { SGTransitionUtil } from "./sg-transition.util";
import { SGTransitionPipeline, SGTransitionStore } from "./sg-transition.store";

export enum SGTransitionDirection {
  enter,
  leave
}

@Injectable({
  providedIn: "root"
})
export class SGTransition implements OnDestroy {
  private _subscription = new Subscription();
  private _currentTransitionsBackup: SGAnimationData;

  constructor(
    private _store: Store,
    private _util: SGTransitionUtil,
    private _transitionStore: SGTransitionStore
  ) {
    // 首屏禁用入场动画（因为SSR时已经有内容了）
    this.disableRouteTransitionAtFirstScreen();

    // `NavigationEnd`时执行入场动画
    this._subscription.add(
      this._store.routeDataChanged$
        .pipe(filter(_ => !this._store.isFirstScreen))
        .subscribe(async data => {
          this.setStream(SGTransitionPipeline.NavigationEnd);
          await this.transitionToEnter(data.sg_transition);
          this.setStream(SGTransitionPipeline.Complete);
        })
    );
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  /**
   * 触发入场过渡
   * @param previousCommands 上一个路由的过渡指令
   */
  private async transitionToEnter(previousCommands?: TransitionCommands) {
    this.setStream(SGTransitionPipeline.TransitionEnteringStart);

    const shouldCrossRoute =
      previousCommands &&
      previousCommands.crossRoute &&
      this._transitionStore._isTransitionCrossedRoute;

    let commands = null;
    if (shouldCrossRoute)
      commands = this.replaceWithPreviousTransitions(previousCommands);
    else commands = previousCommands || new RouteTransitionCommands();

    await this._triggerAnimations(SGTransitionDirection.enter, commands);

    if (shouldCrossRoute) this.restoreCurrentTransitions();
    this.setStream(SGTransitionPipeline.TransiitonEnteringEnd);
  }

  /**
   * 用上一个路由的过渡替换当前路由过渡
   * @param previousCommands 上一个路由的过渡指令
   */
  private replaceWithPreviousTransitions(
    previousCommands: TransitionCommands
  ): CustomizeTransitionCommands {
    let delegate = this._transitionStore._nextTransitionDelegate;
    let previousAnimations = previousCommands["animations"];

    this._currentTransitionsBackup = this._util.deepCopy(delegate.animations);

    this.replaceAnimations(delegate.animations, previousAnimations);

    return new CustomizeTransitionCommands({
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
   * 触发动画
   * @param animations `SGAnimation` 对象集合
   * @internal implementation details, do not use!
   */
  public async _triggerAnimations(
    direction: SGTransitionDirection,
    commands: TransitionCommands
  ) {
    let res = this._util.resolveCommands(commands);
    let duration = this.getDuration(res.animations) + res.extraDuration;
    direction == SGTransitionDirection.leave &&
      res.scrollTo &&
      this.scrollTo(res.scrollTo);
    let anims = Object.values(res.animations);
    anims.map(x => {
      x.leaving = direction == SGTransitionDirection.leave;
      x.animated = true;
    });
    await timeout(duration);
    anims.map(x => (x.animated = false));
  }

  /**
   * 滚动到元素
   * @param elementId
   */
  private scrollTo(elementId: string) {
    if (!this._store.renderFromClient) return;
    document.querySelector(elementId).scrollIntoView({
      behavior: "smooth",
      block: "start",
      inline: "nearest"
    });
  }

  /**
   * === Private Methods ===
   */

  /** 禁用首屏路由过渡 */
  private async disableRouteTransitionAtFirstScreen() {
    if (!this._store.renderFromClient) return;
    await timeout(this.getDuration(this._util.getRouteAnimations));
    this._store.isFirstScreen = false;
  }

  /** 获取一个动画集合的 duration，总是取集合中最长的 duration 值 */
  private getDuration(animations: { [name: string]: SGAnimation }): number {
    const duration = Math.max.apply(
      null,
      Object.values(animations).map(x => x.speed.duration)
    );
    return duration;
  }

  private setStream(stream: SGTransitionPipeline) {
    this._transitionStore._setTransitionStream(stream);
  }
}
