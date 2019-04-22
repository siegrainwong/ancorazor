import { Injectable } from "@angular/core";
import { BehaviorSubject, Subject } from "rxjs";
import { SGTransitionDelegate } from "./sg-transition.delegate";

/** `SGTransition`过渡管道 */
export const enum SGTransitionPipeline {
  /** 管道流开始，当`SGTransition`模块在`Candeactivated guard`中获取到`Component`代理时 */
  Ready,
  /** 等待当前路由上其他`Resolver`解析完毕 */
  ResolveStart,
  /** 当前路由上其他`Resolver`解析完毕 */
  ResolveEnd,
  /** 开始离场过渡 */
  TransitionLeavingStart,
  /** 离场过渡完毕 */
  TransitionLeavingEnd,
  /** 等同于`Router.events`中的`NavigationEnd` */
  NavigationEnd,
  /** 开始入场过渡 */
  TransitionEnteringStart,
  /** 入场过渡完毕 */
  TransiitonEnteringEnd,
  /** 管道流结束，清理状态 */
  Complete
}

@Injectable({
  providedIn: "root"
})
/**
 * 过渡状态管理
 */
export class SGTransitionStore {
  /**
   * `SGTransition`代理
   * 代理会在`Component Candecativated`时被自动赋值，不要手动操作这个状态。
   **/
  public transitionDelegate: SGTransitionDelegate;
  /**
   * 当前组件是否支持转场
   **/
  public get isLeaveTransitionAvailable(): boolean {
    return !!this.transitionDelegate;
  }

  /**
   * 为其他`Resolve`计数
   * 当计数达到其他`Resolve`的数量时（意为着其他`Resolver`执行完毕）才执行转场
   **/
  private _completedResolveCount: number = 0;
  private set completedResolveCount(val: number) {
    this._completedResolveCount = val;
    this.resolveCountChanged$.next(val);
  }
  private get completedResolveCount() {
    return this._completedResolveCount;
  }
  /**
   * `setResolved`后计数变化时触发
   * 用于监听其他`Resolve`是否完成
   **/
  public resolveCountChanged$ = new BehaviorSubject<number>(
    this.completedResolveCount
  );

  public transitionStream: SGTransitionPipeline = SGTransitionPipeline.Complete;
  /**
   * 设置`transitionStream`，由模块管理外部不要设置。
   */
  public setTransitionStream(val: SGTransitionPipeline) {
    this.transitionStream = val;
    this.transitionStreamChanged$.next(val);
  }
  /**
   * `SGTransitionPipeline`过渡管道流变化时触发
   */
  public transitionStreamChanged$ = new BehaviorSubject<SGTransitionPipeline>(
    this.transitionStream
  );

  /** 标记该`Resolve`已执行完毕 */
  public setResolved() {
    if (!this.isLeaveTransitionAvailable) return;
    this.completedResolveCount++;
  }

  /** 清理转场状态 */
  public setTransitioned() {
    if (!this.isLeaveTransitionAvailable) return;
    this.completedResolveCount = 0;
  }
}
