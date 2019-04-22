import { Injectable, OnDestroy } from "@angular/core";
import { timeout } from "../utils/promise-delay";
import { Store } from "../store/store";
import { Subscription } from "rxjs";
import { SGAnimation, TransitionCommands } from "./sg-transition.model";
import { filter } from "rxjs/operators";
import { SGTransitionUtil } from "./sg-transition.util";

export enum SGTransitionDirection {
  enter,
  leave
}

@Injectable({
  providedIn: "root"
})
export class SGTransition implements OnDestroy {
  private _subscription = new Subscription();

  constructor(private _store: Store, private _util: SGTransitionUtil) {
    // 首屏禁用入场动画（因为SSR时已经有内容了）
    this.disableRouteTransitionAtFirstScreen();

    // `NavigationEnd`时执行入场动画
    this._subscription.add(
      this._store.routeDataChanged$
        .pipe(filter(_ => !this._store.isFirstScreen))
        .subscribe(
          async data => await this.transitionToEnter(data.sg_transition)
        )
    );
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  /**
   * 从已有动画定义中返回一个`SGAnimation`的`[ngClass]` 对象
   * @param name 动画名称
   */
  public apply(name: string) {
    return this._util.getAnimation(name).class;
  }

  /**
   * 触发离场过渡
   * @param names 自定义动画集合（不传则触发当前页面所有路由动画）
   * @param extraDuration 额外过渡时间
   * 自定义动画时间内会禁用路由动画
   */
  public async transitionToLeave(commands: TransitionCommands) {
    let res = this._util.resolveCommands(commands);
    await this.triggerAnimations(
      SGTransitionDirection.leave,
      res.animations,
      res.extraDuration,
      res.scrollTo
    );
  }

  /**
   * 触发入场过渡
   * @param commands 过渡指令
   */
  public async transitionToEnter(commands: TransitionCommands) {
    let res = this._util.resolveCommands(commands);
    await this.triggerAnimations(SGTransitionDirection.enter, res.animations);
  }

  /**
   * 触发动画
   * @param animations `SGAnimation` 对象集合
   */
  public async triggerAnimations(
    direction: SGTransitionDirection,
    animations: SGAnimation[],
    extraDuration: number = 0,
    scrollTo?: string
  ) {
    let duration = this.getDuration(animations) + extraDuration;
    scrollTo && this.scrollTo(scrollTo);
    animations.map(x => {
      x.leaving = direction == SGTransitionDirection.leave;
      x.animated = true;
    });
    await timeout(duration);
    animations.map(x => (x.animated = false));
  }

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
}
