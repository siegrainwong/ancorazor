import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import {
  SGBaseCanDeactivatedGuard,
  CanComponentDeactivate
} from "./base.deactivate.guard";
import { SGUtil } from "../utils/siegrain.utils";
import { SGTransition } from "../utils/siegrain.animations";

/* 离场过渡指令 */
export class TransitionToLeaveCommands {
  /* 额外动画时间 */
  extraDuration: number = 0;
  /* 滚动到指定锚点 */
  scrollTo?: string;
  constructor(obj?: Partial<TransitionToLeaveCommands>) {
    Object.assign(this, obj);
  }
}

export interface CanComponentTransitionToLeave extends CanComponentDeactivate {
  /**
   * 是否过渡离场
   * - 不实现、返回 true、返回`TransitionCommands`都执行离场动画
   * - 返回 false 不执行离场动画
   */
  CanComponentTransitionToLeaving?(
    nextState: RouterStateSnapshot
  ): boolean | TransitionToLeaveCommands;
}

@Injectable({
  providedIn: "root"
})
export class LeavingAnimationGuard<
  T extends CanComponentTransitionToLeave
> extends SGBaseCanDeactivatedGuard<CanComponentTransitionToLeave> {
  constructor(private _util: SGUtil, private _transition: SGTransition) {
    super();
  }

  async canDeactivate(
    component: T,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState: RouterStateSnapshot
  ) {
    const superCanDeactivate = async () => {
      return await super.canDeactivate(
        component,
        currentRoute,
        currentState,
        nextState
      );
    };
    let argument = component.CanComponentTransitionToLeaving;
    if (typeof argument === "boolean" && !argument)
      return await superCanDeactivate();

    let commands =
      (typeof argument === "function" &&
        (argument(nextState) as TransitionToLeaveCommands)) ||
      new TransitionToLeaveCommands();
    commands.scrollTo && this._util.scrollTo(commands.scrollTo);
    await this._transition.triggerTransition(null, commands.extraDuration);
    return await superCanDeactivate();
  }
}
