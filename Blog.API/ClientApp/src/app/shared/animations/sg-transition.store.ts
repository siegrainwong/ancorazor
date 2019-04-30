import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { SGTransitionDelegate } from "./sg-transition.delegate";
import { LoggingService } from "../services/logging.service";

/** `SGTransition`过渡管道 */
export enum SGTransitionPipeline {
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
  constructor(private _logger: LoggingService) {}
  /**
   * `SGTransition`代理，用于操控离场过渡
   * @internal implementation detail, do not use!
   **/
  public _transitionDelegate: SGTransitionDelegate;
  /**
   * 下一个路由的`SGTransition`代理，用于操控入场过渡
   * @internal implementation detail, do not use!
   */
  public _nextTransitionDelegate: SGTransitionDelegate;
  /**
   * 当前组件是否支持转场
   * @internal implementation detail, do not use!
   **/
  public get _isLeaveTransitionAvailable(): boolean {
    return !!this._transitionDelegate;
  }

  /** 是否正在过渡 */
  public get isTransitioning() {
    return this._transitionStream != SGTransitionPipeline.Complete;
  }

  /** @internal */
  public _previousRouteConfig: string;
  /** @internal */
  public _nextRouteConfig: string;
  /**
   * 根据过渡的两个`RouteConfigPath`判断是否是跨路由过渡
   * @internal
   **/
  public get _isTransitionCrossedRoute(): boolean {
    return this._previousRouteConfig !== this._nextRouteConfig;
  }

  /**
   * 为其他`Resolve`计数
   * 当计数达到其他`Resolve`的数量时（意为着其他`Resolver`执行完毕）才执行转场
   **/
  private _completedResolveCount: number = 0;
  private set completedResolveCount(val: number) {
    this._completedResolveCount = val;
    this._resolveCountChanged$.next(val);
  }
  private get completedResolveCount() {
    return this._completedResolveCount;
  }
  /**
   * `setResolved`后计数变化时触发
   * 用于监听其他`Resolve`是否完成
   * @internal implementation detail, do not use!
   **/
  public _resolveCountChanged$ = new BehaviorSubject<number>(
    this.completedResolveCount
  );

  private _transitionStream: SGTransitionPipeline =
    SGTransitionPipeline.Complete;
  /**
   * 设置`transitionStream`，由模块管理外部不要设置
   * @internal implementation detail, do not use!
   */
  public _setTransitionStream(val: SGTransitionPipeline) {
    this._logger.info(
      `sg-transition stream ${val}: `,
      this.nameOfEnumMember(SGTransitionPipeline, val)
    );
    this._transitionStream = val;
    this.transitionStreamChanged$.next(val);
    if (val === SGTransitionPipeline.Complete) this._clear();
  }
  /**
   * `SGTransitionPipeline`过渡管道流变化时触发
   */
  public transitionStreamChanged$ = new BehaviorSubject<SGTransitionPipeline>(
    this._transitionStream
  );

  /**
   * 标记该`Resolve`已执行完毕
   *
   * e.g.
   * ```ts
   * async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
   *   : Promise<ArticleModel> {
   *   let id = route.paramMap.get("id");
   *   let res = await this._service.getArticle(parseInt(id));
   *   if (!res) this._router.navigate(["/"]);
   *   this._transitionStore.setResolved();  // 在这里调用
   *   return res;
   * }
   * ```
   **/
  public setResolved() {
    if (!this._isLeaveTransitionAvailable) return;
    // TODO: 这里可以内部维护一个Resolved列表，根据Caller来判断一下，避免重复调用。
    this.completedResolveCount++;
  }

  /**
   * 清理转场状态
   * @internal implementation detail, do not use!
   **/
  private _clear() {
    if (!this._isLeaveTransitionAvailable) return;
    this.completedResolveCount = 0;

    this._transitionDelegate = null;
    this._nextTransitionDelegate = null;

    this._previousRouteConfig = null;
    this._nextRouteConfig = null;

    this._logger.info("sg-transition states cleared.");
  }

  private nameOfEnumMember(source: any, member: any) {
    for (const enumMember in source) {
      const isValueProperty = parseInt(enumMember, 10) >= 0;
      if (isValueProperty && member === parseInt(enumMember)) {
        return source[enumMember];
      }
    }
  }
}
