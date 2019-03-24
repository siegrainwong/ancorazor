import { Injectable, OnDestroy } from "@angular/core";
import { timeout } from "./promise-delay";
import { Store } from "../store/store";
import { BehaviorSubject, Subscription } from "rxjs";
import { constants } from "../constants/siegrain.constants";

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

  ngOnDestroy(): void {
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
   * 在`_routeAnimationEnableDelay`后启用路由动画
   */
  private async enableRouteAnimationAfterDelay() {
    await timeout(this._routeAnimationEnableDelay);
    this._routeAnimationEnableDelay = 0;
    this.routeAnimations.map(x => (x.animated = true));
  }

  /**
   * 禁用路由动画
   * @param extraDelay 额外延时
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
    return this._animations.filter(x => x.type == SGTransitionMode.route);
  }

  private getAnimations(names: string[]): Array<SGAnimation> {
    return this._animations.filter(x => names.indexOf(x.name) > -1);
  }

  /** 获取一个动画集合的 duration，按最长来取 */
  private getDuration(animations: Array<SGAnimation>): number {
    const duration = Math.max.apply(
      null,
      animations.map(x => x.speed.duration)
    );
    return duration;
  }

  /**
   * ######### Animation declarations
   */
  // animation reference: https://daneden.github.io/animate.css/
  private _animations: SGAnimation[] = [
    // router animations
    new SGAnimation({
      name: "fade",
      enterClass: "fadeIn",
      leaveClass: "fadeOut"
    }),
    new SGAnimation({
      name: "fade-up",
      enterClass: "fadeInUp",
      leaveClass: "fadeOutUp"
    }),
    new SGAnimation({
      name: "fade-opposite",
      enterClass: "fadeInUp",
      leaveClass: "fadeOutDown"
    }),

    // custom animations
    new SGAnimation({
      name: "page-turn-next",
      enterClass: "fadeInRight",
      leaveClass: "fadeOutLeft",
      type: SGTransitionMode.custom
    }),
    new SGAnimation({
      name: "page-turn-previous",
      enterClass: "fadeInLeft",
      leaveClass: "fadeOutRight",
      type: SGTransitionMode.custom
    }),
    new SGAnimation({
      name: "page-turn-button",
      enterClass: "fadeInUp",
      leaveClass: "fadeOutDown",
      type: SGTransitionMode.custom
    })
  ];
}

/**
 * animation-duration
 * 定义在`_reset.css`的`animate.css`节内
 */
const speed = {
  slow: { name: "slow", duration: 2000 },
  slower: { name: "slower", duration: 3000 },
  fast: { name: "fast", duration: 800 },
  faster: { name: "faster", duration: 500 }
};

/** 动画类型 */
export enum SGTransitionMode {
  /**
   * 路由动画：每次激活时会触发当前页面的所有路由动画
   **/
  route = "route",
  /**
   * 自定义动画：每次只激活指定名称的动画
   **/
  custom = "custom"
}

export class SGAnimation {
  name: string;
  speed: { name: string; duration: number } = speed.faster;
  type: SGTransitionMode = SGTransitionMode.route;
  /** 是否触发离开动画 */
  leaving: boolean = false;
  /** 是否执行动画 */
  animated: boolean = constants.enableAnimation;

  enterClass: string;
  leaveClass: string;
  constructor(init?: Partial<SGAnimation>) {
    Object.assign(this, init);
  }

  /**
   * 获取 `[ngClass]` 对象，通过这个对象操控`dom`元素的动画效果
   */
  get class() {
    let animation = {};
    animation[this.enterClass] = !this.leaving;
    animation["enter"] = !this.leaving;
    animation[this.leaveClass] = this.leaving;
    animation["leave"] = this.leaving;
    animation[this.speed.name] = true;
    animation["animated"] = this.animated;
    return animation;
  }
}
