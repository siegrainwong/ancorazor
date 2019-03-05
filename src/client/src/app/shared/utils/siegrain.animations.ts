import { Injectable } from "@angular/core";
import { timeout } from "./promise-delay";

@Injectable({
  providedIn: "root"
})
export class SGTransition {
  constructor() {}

  /**
   * 返回一个 [ngClass] 对象
   */
  apply(name: string) {
    return this._animations.find(x => x.name == name).class;
  }

  /**
   * 触发转场动画
   * @param type 动画类型
   * @param name 自定义动画名称（如果是自定义动画才需要传入）
   */
  async triggerTransition(type: type, name?: string) {
    let animations: SGAnimation[] = [];
    if (!name) animations = this._animations.filter(x => x.type == type);
    else animations = [this._animations.find(x => x.name == name)];

    animations.map(x => (x.leaving = !x.leaving));

    const duration = Math.min.apply(
      null,
      animations.map(x => x.speed.duration)
    );
    await timeout(duration);

    animations.map(x => (x.leaving = !x.leaving));
  }

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
      enterClass: "fadeIn",
      leaveClass: "fadeOut"
    }),

    // custom animations
    new SGAnimation({
      name: "page_turn_next",
      enterClass: "fadeInRight",
      leaveClass: "fadeOutLeft",
      type: type.custom
    }),
    new SGAnimation({
      name: "page_turn_previous",
      enterClass: "fadeInLeft",
      leaveClass: "fadeOutRight",
      type: type.custom
    })
  ];
}

const speed = {
  slow: { name: "slow", duration: 2000 },
  slower: { name: "slower", duration: 3000 },
  fast: { name: "fast", duration: 800 },
  faster: { name: "faster", duration: 500 }
};

export enum type {
  /**
   * 转场动画：每次激活时会触发当前页面的所有转场动画
   * duration 取当前所有转场动画的最小值
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
  type: type = type.route;
  /** 是否触发离开动画 */
  leaving: boolean = false;

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
    animation["animated"] = true;
    return animation;
  }
}
