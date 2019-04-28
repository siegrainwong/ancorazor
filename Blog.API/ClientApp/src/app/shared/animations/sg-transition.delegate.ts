import {
  SGCustomizeTransitionCommands,
  SGTransitionMode,
  SGRouteTransitionCommands,
  SGAnimation,
  SGAnimationData
} from "./sg-transition.model";
import { ActivatedRouteSnapshot } from "@angular/router";

/**
 * 过渡代理，配置路由过渡
 * @requires
 **/
export interface SGTransitionDelegate {
  /**
   * 声明当前组件的需要用到的动画
   * @requires
   *
   * ```ts
   * animations = {
   *  list: SGAnimations.fadeOpposite,
   *  header: SGAnimations.fadeUp
   * }
   * ```
   */
  animations: SGAnimationData;

  /**
   * 路由过渡离场配置
   * @optional
   */
  transitionForComponent?(
    nextRoute: ActivatedRouteSnapshot
  ): SGRouteTransitionCommands;
}

/**
 * 过渡代理，配置自定义路由过渡
 * 需要对自定义路由过渡进行配置时实现该接口
 * **在自定义过渡动画执行期间将禁用所有路由过渡动画**
 * @optional
 **/
export interface SGCustomizeTransitionDelegate extends SGTransitionDelegate {
  /**
   * 指定当前要执行的过渡模式
   * @requires
   *
   * @returns SGTransitionMode
   * - `SGTransitionMode.route` 当前执行路由动画
   * - `SGTransitionMode.custom` 当前执行自定义动画
   **/
  modeForComponentTransition(
    nextRoute: ActivatedRouteSnapshot
  ): SGTransitionMode;

  /**
   * 自定义路由过渡配置
   * @requires
   */
  customizeTransitionForComponent(
    nextRoute: ActivatedRouteSnapshot
  ): SGCustomizeTransitionCommands;
}
