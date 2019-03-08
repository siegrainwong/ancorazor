import { Injectable } from "@angular/core";
import { timeout } from "./promise-delay";
import { Store } from "../store/store";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class SGTransition {
  private _routeAnimationDisabledDuration = 0;
  transitionWillBegin$ = new BehaviorSubject<SGTransitionMode>(
    SGTransitionMode.route
  );

  constructor(store: Store) {
    store.routeDataChanged$.subscribe(() => {
      this.enableRouteAnimations();
    });
  }

  /**
   * 返回一个 [ngClass] 对象
   * @param name 动画名称
   */
  apply(name: string) {
    return this.getAnimations([name])[0].class;
  }

  /**
   * 触发转场动画
   * @param names 自定义动画集合（不传则触发当前页面所有路由动画）
   * 自定义动画时间内会禁用路由动画
   */
  async triggerTransition(names?: string[]) {
    this.transitionWillBegin$.next(
      names ? SGTransitionMode.custom : SGTransitionMode.route
    );
    let animations: SGAnimation[] = [];
    let duration = 0;

    if (!names || names.length == 0) {
      animations = this.routeAnimations;
      duration = this.getDuration(animations);
    } else {
      animations = this.getAnimations(names);
      duration = this.getDuration(animations);
      this._routeAnimationDisabledDuration = duration;
      this.routeAnimations.map(x => (x.animated = false));
    }

    animations.map(x => (x.leaving = true));
    await timeout(duration);
    animations.map(x => (x.leaving = false));
  }

  async enableRouteAnimations() {
    await timeout(this._routeAnimationDisabledDuration);
    this._routeAnimationDisabledDuration = 0;
    this.routeAnimations.map(x => (x.animated = true));
  }

  /**
   * ######### Acessors
   */
  private get routeAnimations(): Array<SGAnimation> {
    return this._animations.filter(x => x.type == SGTransitionMode.route);
  }

  private getAnimations(names: string[]): Array<SGAnimation> {
    return this._animations.filter(x => names.indexOf(x.name) > -1);
  }

  private getDuration(animations: Array<SGAnimation>): number {
    const duration = Math.min.apply(
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
      name: "nav_title",
      enterClass: "fadeIn",
      leaveClass: "fadeOut"
    }),
    new SGAnimation({
      name: "title",
      enterClass: "fadeInUp",
      leaveClass: "fadeOutUp"
    }),
    new SGAnimation({
      name: "cover",
      enterClass: "fadeIn",
      leaveClass: "fadeOut"
    }),
    new SGAnimation({
      name: "articles",
      enterClass: "fadeInUp",
      leaveClass: "fadeOutDown"
    }),
    new SGAnimation({
      name: "article",
      enterClass: "fadeInUp",
      leaveClass: "fadeOutDown"
    }),

    // custom animations
    new SGAnimation({
      name: "page_turn_next",
      enterClass: "fadeInRight",
      leaveClass: "fadeOutLeft",
      type: SGTransitionMode.custom
    }),
    new SGAnimation({
      name: "page_turn_previous",
      enterClass: "fadeInLeft",
      leaveClass: "fadeOutRight",
      type: SGTransitionMode.custom
    }),
    new SGAnimation({
      name: "page_turn_button",
      enterClass: "fadeInUp",
      leaveClass: "fadeOutDown",
      type: SGTransitionMode.custom
    })
  ];
}

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

class SGAnimation {
  name: string;
  speed: { name: string; duration: number } = speed.faster;
  type: SGTransitionMode = SGTransitionMode.route;
  /** 是否触发离开动画 */
  leaving: boolean = false;
  /** 是否执行动画 */
  animated: boolean = true;

  enterClass: string;
  leaveClass: string;
  constructor(init?: Partial<SGAnimation>) {
    Object.assign(this, init);
  }

  get class() {
    let animation = {};
    animation[this.enterClass] = !this.leaving;
    animation[this.leaveClass] = this.leaving;
    animation[this.speed.name] = true;
    animation["animated"] = this.animated;
    return animation;
  }
}
