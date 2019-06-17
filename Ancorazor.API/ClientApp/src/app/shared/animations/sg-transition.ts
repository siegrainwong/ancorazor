import { Injectable, OnDestroy } from "@angular/core";
import { SGTransitionCommands } from "./sg-transition.model";
import { SGTransitionUtil } from "./sg-transition.util";
import { timeout } from "../utils/promise-delay";
import { SGAnimations } from "./sg-animations";

export enum SGTransitionDirection {
  enter,
  leave
}

/**
 * 模块核心
 */
@Injectable({
  providedIn: "root"
})
export class SGTransition {
  constructor(private _util: SGTransitionUtil) {}
  /**
   * 触发动画
   * @param animations `SGAnimation` 对象集合
   * @internal implementation details, do not use!
   */
  public async _triggerAnimations(
    direction: SGTransitionDirection,
    commands: SGTransitionCommands
  ) {
    let res = this._util.resolveCommands(commands);
    let duration = this._util.getDuration(res.animations) + res.extraDuration;
    let anims = Object.values(res.animations);
    anims.map(x => {
      x.leaving = direction == SGTransitionDirection.leave;
      x.animated = true;
    });
    await timeout(duration);
    anims.map(x => (x.animated = false));
  }
}
