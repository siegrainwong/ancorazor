import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { SGTransitionDelegate } from "./sg-transition.delegate";

export const enum SGTransitionPipeline {}

@Injectable({
  providedIn: "root"
})
/**
 * 过渡状态管理
 */
export class SGTransitionStore {
  public transitionDelegate: SGTransitionDelegate;
  public get isLeaveTransitionAvailable(): boolean {
    return !!this.transitionDelegate;
  }

  /**
   * 为其他`Resolve`计数
   * 当计数达到其他`Resolve`的数量时才执行转场
   **/
  private _completedResolveCount: number = 0;
  private set completedResolveCount(val: number) {
    this._completedResolveCount = val;
    this.resolveCompletedChanged$.next(val);
  }
  private get completedResolveCount() {
    return this._completedResolveCount;
  }
  public resolveCompletedChanged$ = new BehaviorSubject<number>(
    this.completedResolveCount
  );

  /** 标记该`Resolve`已执行完毕 */
  public setResolved() {
    if (!this.isLeaveTransitionAvailable) return;
    this.completedResolveCount++;
  }

  /** 清理转场状态 */
  public setTransitioned() {
    if (!this.isLeaveTransitionAvailable) return;
    this.transitionDelegate = null;
    this.completedResolveCount = 0;
  }
}
