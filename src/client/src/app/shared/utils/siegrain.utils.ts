import { Injectable } from "@angular/core";
import { MatSnackBar, MatDialog } from "@angular/material";
import { Router, NavigationExtras } from "@angular/router";
import { SGTransition } from "./siegrain.animations";
import {
  ConfirmDialog,
  ConfirmDialogData
} from "../components/confirm-dialog.component";

export const enum TipType {
  Confirm = "❔",
  Success = "✔️",
  Warn = "⚠️",
  Danger = "❌"
}

@Injectable()
export class SGUtil {
  constructor(
    private snackBar: MatSnackBar,
    private router: Router,
    private transition: SGTransition,
    private dialog: MatDialog
  ) {}

  /**##### Utilities */

  /**
   * ref: `MatSnackBar`
   * @param msg 消息内容
   * @param type 消息类型
   */
  tip(msg: string, type: TipType = TipType.Danger) {
    this.snackBar.open(`${type} ${msg}`, "Got it", {
      duration: 5000
    });
  }

  /**
   * 确认框
   * @param title 标题
   * @param type 类型
   * @param content 内容
   */
  confirm(
    title: string = "Confirm",
    type: TipType = TipType.Confirm,
    content: string = "Are you sure?"
  ): Promise<boolean> {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: new ConfirmDialogData({
        title: title,
        content: content,
        type: type
      })
    });
    return new Promise(resolve => {
      dialogRef.afterClosed().subscribe(data => {
        resolve(data);
      });
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
