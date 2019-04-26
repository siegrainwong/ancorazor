import { Injectable, OnDestroy } from "@angular/core";
import { timeout } from "../utils/promise-delay";
import { Store } from "../store/store";
import { Subscription } from "rxjs";
import { SGAnimation, TransitionCommands } from "./sg-transition.model";
import { filter } from "rxjs/operators";
import { SGTransitionUtil } from "./sg-transition.util";
import { SGTransitionPipeline, SGTransitionStore } from "./sg-transition.store";
import { ActivatedRoute } from "@angular/router";
import { SGAnimations } from "./sg-animations";

export enum SGTransitionDirection {
  enter,
  leave
}

@Injectable({
  providedIn: "root"
})
export class SGTransition implements OnDestroy {
  private _subscription = new Subscription();

  constructor(
    private _store: Store,
    private _util: SGTransitionUtil,
    private _transitionStore: SGTransitionStore,
    private _route: ActivatedRoute
  ) {
    // 首屏禁用入场动画（因为SSR时已经有内容了）
    this.disableRouteTransitionAtFirstScreen();

    // `NavigationEnd`时执行入场动画
    this._subscription.add(
      this._store.routeDataChanged$
        .pipe(filter(_ => !this._store.isFirstScreen))
        .subscribe(async data => {
          this.setStream(SGTransitionPipeline.NavigationEnd);
          await this.transitionToEnter(data.sg_transition);
          this.setStream(SGTransitionPipeline.Complete);
        })
    );
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  /**
   * 触发入场过渡
   * @param commands 过渡指令
   */
  public async transitionToEnter(commands: TransitionCommands) {
    this.setStream(SGTransitionPipeline.TransitionEnteringStart);

    // 虽然这里其实每次在切换路由后都会收到上一个路由的commands，但其实。
    // triggerAnimation 的时候 trigger 不动。

    // 用上一个路由的动画声明替换当前路由同名动画声明
    // if (commands.crossRoute) {
    //   let delegate = this._transitionStore._nextTransitionDelegate;
    //   // let previousAnimations = commands["animations"];
    //   // let animations = this._util.entriesToObject(
    //   //   Object.entries(delegate.animations).map(x => {
    //   //     let sameNameAnim = previousAnimations[x[0]];
    //   //     return (sameNameAnim && [x[0], sameNameAnim]) || x;
    //   //   })
    //   // );
    //   // commands["animations"] = animations;
    //   commands["animations"] = delegate.animations;
    //   commands["animations"].articles = SGAnimations.fade;
    // }
    if (commands) commands["animations"] = this._util.getRouteAnimations;
    await this._triggerAnimations(SGTransitionDirection.enter, commands);
    this.setStream(SGTransitionPipeline.TransiitonEnteringEnd);
  }

  /**
   * 触发动画
   * @param animations `SGAnimation` 对象集合
   * @internal implementation details, do not use!
   */
  public async _triggerAnimations(
    direction: SGTransitionDirection,
    commands: TransitionCommands
  ) {
    let res = this._util.resolveCommands(commands);
    let duration = this.getDuration(res.animations) + res.extraDuration;
    this.scrollTo(res.scrollTo);
    let anims = Object.values(res.animations);
    anims.map(x => {
      x.leaving = direction == SGTransitionDirection.leave;
      x.animated = true;
    });
    await timeout(duration);
    anims.map(x => (x.animated = false));
  }

  /**
   * 滚动到元素
   * @param elementId
   */
  private scrollTo(elementId: string) {
    if (!this._store.renderFromClient) return;
    document.querySelector(elementId).scrollIntoView({
      behavior: "smooth",
      block: "start",
      inline: "nearest"
    });
  }

  /**
   * === Private Methods ===
   */

  /** 禁用首屏路由过渡 */
  private async disableRouteTransitionAtFirstScreen() {
    if (!this._store.renderFromClient) return;
    await timeout(this.getDuration(this._util.getRouteAnimations));
    this._store.isFirstScreen = false;
  }

  /** 获取一个动画集合的 duration，总是取集合中最长的 duration 值 */
  private getDuration(animations: { [name: string]: SGAnimation }): number {
    const duration = Math.max.apply(
      null,
      Object.values(animations).map(x => x.speed.duration)
    );
    return duration;
  }

  private setStream(stream: SGTransitionPipeline) {
    this._transitionStore._setTransitionStream(stream);
  }
}
