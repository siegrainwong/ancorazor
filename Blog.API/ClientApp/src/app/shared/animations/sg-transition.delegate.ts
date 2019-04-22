import {
  CustomizeTransitionCommands,
  SGTransitionMode,
  RouteTransitionCommands
} from "./sg-transition.model";
import { ActivatedRouteSnapshot } from "@angular/router";

/**
 * 过渡代理，配置路由过渡
 * 需要对路由过渡进行**额外**配置时实现该接口
 **/
export interface SGTransitionDelegate {
  /**
   * 路由过渡离场配置
   */
  transitionForComponent?(
    nextRoute: ActivatedRouteSnapshot
  ): RouteTransitionCommands;
}

/**
 * 过渡代理，配置自定义路由过渡
 * 需要对自定义路由过渡进行配置时实现该接口
 * **在自定义过渡动画执行期间将禁用所有路由过渡动画**
 **/
export interface SGCustomizeTransitionDelegate extends SGTransitionDelegate {
  /**
   * 指定当前要执行的过渡模式
   * @returns SGTransitionMode
   * - `SGTransitionMode.route` 执行路由动画
   * - `SGTransitionMode.custom` 执行自定义动画
   **/
  modeForComponentTransition(
    nextRoute: ActivatedRouteSnapshot
  ): SGTransitionMode;

  /**
   * 自定义路由过渡配置
   */
  customizeTransitionForComponent(
    nextRoute: ActivatedRouteSnapshot
  ): CustomizeTransitionCommands;
}
