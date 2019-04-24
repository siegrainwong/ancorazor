import { Injectable, OnDestroy } from "@angular/core";
import { timeout } from "../utils/promise-delay";
import { Store } from "../store/store";
import { Subscription } from "rxjs";
import { SGAnimation, TransitionCommands } from "./sg-transition.model";
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
   * @param commands 过渡指令
   */
  public async transitionToEnter(commands: TransitionCommands) {
    this.setStream(SGTransitionPipeline.TransitionEnteringStart);
    await this._triggerAnimations(SGTransitionDirection.leave, commands);
    this.setStream(SGTransitionPipeline.TransiitonEnteringEnd);
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
    scrollTo && this.scrollTo(res.scrollTo);
    res.animations.map(x => {
      x.leaving = direction == SGTransitionDirection.leave;
      x.animated = true;
    });
    await timeout(duration);
    res.animations.map(x => (x.animated = false));
  }

  /**
   * 滚动到元素
   * @param elementId
   */
  private scrollTo(elementId: string) {
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
    await timeout(this.getDuration(this._util.getRouteAnimations));
    this._store.isFirstScreen = false;
  }

  /** 获取一个动画集合的 duration，总是取集合中最长的 duration 值 */
  private getDuration(animations: Array<SGAnimation>): number {
    const duration = Math.max.apply(
      null,
      animations.map(x => x.speed.duration)
    );
    return duration;
  }

  private setStream(stream: SGTransitionPipeline) {
    this._transitionStore._setTransitionStream(stream);
  }
}
