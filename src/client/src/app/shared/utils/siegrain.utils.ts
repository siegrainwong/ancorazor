import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { Router, NavigationExtras } from "@angular/router";
import { SGTransitionMode, SGTransition } from "./siegrain.animations";

export const enum TipType {
  Success = "✔️",
  Warn = "⚠️",
  Error = "❌"
}

@Injectable()
export class SGUtil {
  constructor(
    private snackBar: MatSnackBar,
    private router: Router,
    private transition: SGTransition
  ) {}

  /**##### Utilities */

  /**
   * ref: `MatSnackBar`
   * @param msg 消息内容
   * @param type 消息类型（未实装）
   */
  tip(msg: string, type: TipType = TipType.Error) {
    this.snackBar.open(`${type} ${msg}`, "Got it.", {
      duration: 5000
    });
  }

  /**
   * 路由
   * 此方法会触发动画
   * @param commands `router.navigate` parameter
   * @param extras `router.navigate` parameter
   * @param names 动画名称集合
   */
  async routeTo(commands: any[], extras?: NavigationExtras, names?: string[]) {
    await this.transition.triggerTransition(names);
    this.router.navigate(commands, extras);
  }
}

export { timeFormat, dateFormat, getKeyByValue } from "./time-format";
export { timeout } from "./promise-delay";
export { random } from "./random";
