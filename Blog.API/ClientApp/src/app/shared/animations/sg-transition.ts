import { Injectable, OnDestroy } from "@angular/core";
import { timeout } from "../utils/promise-delay";
import { Store } from "../store/store";
import { Subscription } from "rxjs";
import {
  SGTransitionMode,
  SGAnimation,
  TransitionCommands,
  CustomizeTransitionCommands,
  isCustomizeCommands
} from "./sg-transition.model";
import { first } from "rxjs/operators";
import { SGAnimations } from "./sg-animations";

enum SGTransitionDirection {
  enter,
  leave
}

@Injectable({
  providedIn: "root"
})
export class SGTransition implements OnDestroy {
  private _subscription = new Subscription();
  private _routeAnimationEnableDelay = 0;

  constructor(private _store: Store) {
    // 首屏禁用入场动画（因为SSR时已经有内容了）
    this._store.routeDataChanged$
      .pipe(first(_ => this._store.isFirstScreen))
      .subscribe(async () => await this.disableRouteTransitionAtFirstScreen());

    // `NavigationEnd`时执行入场动画
    this._store.routeDataChanged$.subscribe(
      async data =>
        !this._store.isFirstScreen &&
        (await this.triggerEnterTransition(data.sg_transition))
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
    return this.getAnimation(name).class;
  }

  /**
   * 触发转场动画
   * @param names 自定义动画集合（不传则触发当前页面所有路由动画）
   * 自定义动画时间内会禁用路由动画
   */
  public async triggerLeaveTransition(
    names?: string[],
    extraDuration: number = 0
  ) {
    let animations: SGAnimation[] = [];

    if (!names || names.length == 0) {
      animations = this.routeAnimations;
    } else {
      animations = this.getAnimations(names);
      this.disableRouteTransition();
    }

    await this.triggerAnimations(
      animations,
      SGTransitionDirection.leave,
      extraDuration
    );
  }

  /**
   * 触发动画
   * @param animations `SGAnimation` 对象集合
   */
  public async triggerAnimations(
    animations: SGAnimation[],
    direction: SGTransitionDirection,
    extraDuration: number = 0
  ) {
    let duration = this.getDuration(animations) + extraDuration;
    animations.map(x => (x.leaving = direction == SGTransitionDirection.leave));
    animations.map(x => (x.animated = true));
    await timeout(duration);
    animations.map(x => (x.animated = false));
  }

  /**
   * ######### Private Methods
   */
  /** 执行路由入场动画 */
  private async triggerEnterTransition(routeCommands: TransitionCommands) {
    let animations = this.routeAnimations;
    if (routeCommands && isCustomizeCommands(routeCommands)) {
      this.disableRouteTransition();
      animations = this.getAnimations(
        (routeCommands as CustomizeTransitionCommands).names
      );
    }
    await this.triggerAnimations(animations, SGTransitionDirection.enter);
  }

  /** 禁用首屏路由过渡 */
  private async disableRouteTransitionAtFirstScreen() {
    if (this._store.isFirstScreen) this.disableRouteTransition(500);

    if (this._routeAnimationEnableDelay != 0)
      await this.enableRouteTransitionAfterDelay();

    if (this._store.isFirstScreen) this._store.isFirstScreen = false;
  }

  /**
   * 在`_routeAnimationEnableDelay`后启用路由动画
   * 用于触发自定义入场动画时禁用路由动画
   */
  private async enableRouteTransitionAfterDelay() {
    await timeout(this._routeAnimationEnableDelay);
    this._routeAnimationEnableDelay = 0;
    this.routeAnimations.map(x => (x.animated = true));
  }

  /**
   * 禁用路由动画
   * @param extraDelay 额外延时，用于处理页面、动画卡顿造成路由动画禁用时间不够的问题
   */
  private disableRouteTransition(extraDelay: number = 0) {
    const animations = this.routeAnimations;
    animations.map(x => (x.animated = false));
    const duration = this.getDuration(animations);
    this._routeAnimationEnableDelay = duration + extraDelay;
  }

  /**
   * ######### Acessors
   */

  /**
   * 从已有动画定义中返回一个 `SGAnimation` 对象的一个**引用**
   * @param name
   */
  public getAnimation(name: string): SGAnimation {
    return this.getAnimations([name])[0];
  }

  private get routeAnimations(): Array<SGAnimation> {
    return SGAnimations.filter(x => x.type == SGTransitionMode.route);
  }

  private getAnimations(names: string[]): Array<SGAnimation> {
    return SGAnimations.filter(x => names.indexOf(x.name) > -1);
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
