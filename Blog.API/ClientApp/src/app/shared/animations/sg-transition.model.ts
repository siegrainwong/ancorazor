/**
 * 动画速度
 * 定义在`_reset.css`的`animate.css`节内
 * 修改这里的值并不会对动画时长有实际性效果，只用于计算
 */
export const SGAnimationSpeed = {
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
  speed: { name: string; duration: number } = SGAnimationSpeed.faster;
  type: SGTransitionMode = SGTransitionMode.route;
  /** 是否触发离开动画 */
  leaving: boolean = false;
  /** 是否执行动画 */
  animated: boolean = false;

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

/**
 * === Transition Commands ===
 */

/** 过渡指令 */
export class TransitionCommands {
  /** 滚动到指定锚点 */
  scrollTo?: string;
  /**
   * 指示当前是否是跨路由过渡
   * 值为`true`时，本次命令中执行的动画会覆盖下一个组件中的同名动画
   *
   * 一般用于中途修改过渡效果后，让下一个组件继承当前的过渡效果，让过渡更自然
   **/
  crossRoute: boolean = false;
}

/** 离场过渡指令 */
export class RouteTransitionCommands extends TransitionCommands {
  constructor(obj?: Partial<RouteTransitionCommands>) {
    super();
    Object.assign(this, obj);
  }
}

/** 自定义动画过渡指令 */
export class CustomizeTransitionCommands extends TransitionCommands {
  /** 要执行的动画 */
  animations!: { [name: string]: SGAnimation };
  /** 额外动画时间 */
  extraDuration: number = 0;

  constructor(obj: CustomizeTransitionCommands) {
    super();
    Object.assign(this, obj);
  }
}
