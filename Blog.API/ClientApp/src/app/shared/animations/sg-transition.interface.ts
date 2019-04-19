import { RouterStateSnapshot } from "@angular/router";
import {
  CustomizeTransitionCommands,
  SGTransitionMode,
  RouteTransitionCommands
} from "./sg-transition.model";

/**
 * 配置组件是否可以离场
 **/
export interface CanComponentDeactivate {
  canDeactivate?: () => Promise<boolean> | boolean;
}

/**
 * 配置组件是否可以过渡离场
 **/
export interface CanComponentTransitionToLeave extends CanComponentDeactivate {
  ResolveDataBeforeLeave?();
  /**
   * 是否过渡离场
   * - 返回`false`不执行动画
   * - 不实现 或 返回`true`则执行
   */
  CanComponentTransitionToLeave?(
    component: CanComponentTransitionToLeave,
    url: string
  ): boolean;
}

/**
 * 配置路由过渡
 * 需要对路由过渡进行**额外**配置时实现该接口
 **/
export interface RouteTransitionForComponent
  extends CanComponentTransitionToLeave {
  /**
   * 路由过渡离场动画配置
   */
  RouteTransitionForComponent?(
    component: CanComponentTransitionToLeave,
    url: string
  ): RouteTransitionCommands;
}

/**
 * 配置自定义过渡动画
 * 页面需要自定义过渡动画时实现该接口
 **/
export interface CustomizeTransitionForComponent
  extends RouteTransitionForComponent {
  /** 指定自定义动画名称集合 */
  animationNames: string[];

  /**
   * 指定当前要执行的动画
   * @returns SGTransitionMode
   * - `SGTransitionMode.route` 执行路由动画
   * - `SGTransitionMode.custom` 执行自定义动画
   **/
  ModeForComponentTransition(
    component: CanComponentTransitionToLeave,
    url: string
  ): SGTransitionMode;

  /**
   * 自定义动画配置
   */
  CustomizeTransitionForComponent(
    component: CanComponentTransitionToLeave,
    url: string
  ): CustomizeTransitionCommands;
}
