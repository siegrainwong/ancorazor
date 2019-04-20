import { Injectable, OnDestroy } from "@angular/core";
import { BehaviorSubject, Subscription, Observable } from "rxjs";
import { SGTransitionMode, SGAnimation } from "./sg-transition.model";
import { SGTransitionDelegate } from "./sg-transition.interface";

@Injectable({
  providedIn: "root"
})
export class SGTransitionStore {
  public transitionDelegate: SGTransitionDelegate;

  public transitionWillBegin$ = new BehaviorSubject<{
    mode: SGTransitionMode;
    animations: SGAnimation[];
  }>({ mode: SGTransitionMode.route, animations: [] });

  /**
   * 为其他`Resolve`计数
   * 当计数达到其他`Resolve`的数量时才执行转场
   **/
  private _completedResolveCount: number = 0;
  public resolveCompletedChanged$ = new BehaviorSubject<number>(
    this._completedResolveCount
  );

  /** 标记该`Resolve`已执行完毕 */
  public setResolved() {
    this._completedResolveCount++;
    this.resolveCompletedChanged$.next(this._completedResolveCount);
  }

  /** 清理转场状态 */
  public setTransitioned() {
    this.transitionDelegate = null;
    this._completedResolveCount = 0;
  }
}
