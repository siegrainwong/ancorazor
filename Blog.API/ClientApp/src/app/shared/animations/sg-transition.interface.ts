import { RouterStateSnapshot } from "@angular/router";
import {
  CustomizeTransitionCommands,
  SGTransitionMode,
  RouteTransitionCommands
} from "./sg-transition.model";

/**
 * 过渡动画代理，配置路由过渡
 * 需要对路由过渡进行**额外**配置时实现该接口
 **/
export interface SGTransitionDelegate {
  /**
   * 路由过渡离场动画配置
   */
  RouteTransitionForComponent?(
    component: SGTransitionDelegate,
    url: string
  ): RouteTransitionCommands;
}

export interface SGCustomTransitionDelegate extends SGTransitionDelegate {
  /**
   * 指定当前要执行的动画
   * @returns SGTransitionMode
   * - `SGTransitionMode.route` 执行路由动画
   * - `SGTransitionMode.custom` 执行自定义动画
   **/
  ModeForComponentTransition(
    component: SGTransitionDelegate,
    url: string
  ): SGTransitionMode;

  /**
   * 自定义动画配置
   */
  CustomizeTransitionForComponent(
    component: SGTransitionDelegate,
    url: string
  ): CustomizeTransitionCommands;
}

/**
 * 配置组件是否可以离场
 **/
// export interface CanComponentDeactivate {
//   canDeactivate?: () => Promise<boolean> | boolean;
// }

/**
 * 配置路由过渡
 * 需要对路由过渡进行**额外**配置时实现该接口
 **/
// export interface RouteTransitionForComponent
//   extends CanComponentTransitionToLeave {
//   /**
//    * 路由过渡离场动画配置
//    */
//   RouteTransitionForComponent?(
//     component: CanComponentTransitionToLeave,
//     url: string
//   ): RouteTransitionCommands;
// }

/**
 * 配置自定义过渡动画
 * 页面需要自定义过渡动画时实现该接口
 **/
// export interface CustomizeTransitionForComponent
//   extends RouteTransitionForComponent {
//   /** 指定自定义动画名称集合 */
//   animationNames: string[];

//   /**
//    * 指定当前要执行的动画
//    * @returns SGTransitionMode
//    * - `SGTransitionMode.route` 执行路由动画
//    * - `SGTransitionMode.custom` 执行自定义动画
//    **/
//   ModeForComponentTransition(
//     component: CanComponentTransitionToLeave,
//     url: string
//   ): SGTransitionMode;

//   /**
//    * 自定义动画配置
//    */
//   CustomizeTransitionForComponent(
//     component: CanComponentTransitionToLeave,
//     url: string
//   ): CustomizeTransitionCommands;
// }
