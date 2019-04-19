import { Injectable, OnDestroy } from "@angular/core";
import { timeout } from "../utils/promise-delay";
import { Store } from "../store/store";
import { BehaviorSubject, Subscription } from "rxjs";
import { SGTransitionMode, SGAnimation } from "./sg-transition.model";
import { SGAnimations as animations } from "./sg-animations";

@Injectable({
  providedIn: "root"
})
export class SGTransition implements OnDestroy {
  private _subscription = new Subscription();
  private _currentTransitionMode: SGTransitionMode = SGTransitionMode.route;
  private _routeAnimationEnableDelay = 0;

  public transitionWillBegin$ = new BehaviorSubject<{
    mode: SGTransitionMode;
    animations: SGAnimation[];
  }>({ mode: SGTransitionMode.route, animations: [] });

  constructor(private _store: Store) {
    this._subscription.add(
      this._store.routeDataChanged$.subscribe(async () => {
        if (this._store.isFirstScreen) this.disableRouteAnimation(500);

        if (this._routeAnimationEnableDelay != 0)
          await this.enableRouteAnimationAfterDelay();

        if (this._store.isFirstScreen) this._store.isFirstScreen = false;
      })
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
  public async triggerTransition(names?: string[], extraDuration: number = 0) {
    this._currentTransitionMode = names
      ? SGTransitionMode.custom
      : SGTransitionMode.route;
    let animations: SGAnimation[] = [];

    if (!names || names.length == 0) {
      animations = this.routeAnimations;
    } else {
      animations = this.getAnimations(names);
      this.disableRouteAnimation();
    }

    this.transitionWillBegin$.next({
      mode: this._currentTransitionMode,
      animations
    });

    await this.triggerAnimations(animations, extraDuration);
  }

  /**
   * 触发动画
   * @param animations `SGAnimation` 对象集合
   */
  public async triggerAnimations(
    animations: SGAnimation[],
    extraDuration: number = 0
  ) {
    let duration = this.getDuration(animations) + extraDuration;
    animations.map(x => (x.leaving = true));
    await timeout(duration);
    animations.map(x => (x.leaving = false));
  }

  /**
   * ######### Private Methods
   */

  /**
   * 在`_routeAnimationEnableDelay`后启用路由动画
   * 用于触发自定义入场动画时禁用路由动画
   */
  private async enableRouteAnimationAfterDelay() {
    await timeout(this._routeAnimationEnableDelay);
    this._routeAnimationEnableDelay = 0;
    this.routeAnimations.map(x => (x.animated = true));
  }

  /**
   * 禁用路由动画
   * @param extraDelay 额外延时，用于处理页面、动画卡顿造成路由动画禁用时间不够的问题
   */
  private disableRouteAnimation(extraDelay: number = 0) {
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
    return animations.filter(x => x.type == SGTransitionMode.route);
  }

  private getAnimations(names: string[]): Array<SGAnimation> {
    return animations.filter(x => names.indexOf(x.name) > -1);
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
