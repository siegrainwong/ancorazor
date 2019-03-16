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
    private _snackBar: MatSnackBar,
    private _router: Router,
    private _transition: SGTransition,
    private _dialog: MatDialog
  ) {}

  /** === Utilities ===*/

  /**
   * ref: `MatSnackBar`
   * @param msg 消息内容
   * @param type 消息类型
   */
  public tip(msg: string, type: TipType = TipType.Danger) {
    this._snackBar.open(`${type} ${msg}`, "Got it", {
      duration: 5000
    });
  }

  /**
   * 确认框
   * @param title 标题
   * @param type 类型
   * @param content 内容
   */
  public confirm(
    title: string = "Confirm",
    type: TipType = TipType.Confirm,
    content: string = "Are you sure?"
  ): Promise<boolean> {
    const dialogRef = this._dialog.open(ConfirmDialog, {
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

  public scrollTo(elementId: string) {
    document.querySelector(elementId).scrollIntoView({
      behavior: "smooth",
      block: "start",
      inline: "nearest"
    });
  }

  /**
   * 路由
   * 此方法会触发动画
   * @param commands `router.navigate` parameter
   * @param extras `router.navigate` parameter
   * @param names 动画名称集合
   * @param extraDuration 延长动画过渡时间
   * @param scrollToElementId 滚动到元素
   */
  public async routeTo(
    commands: any[],
    argument?: {
      extras?: NavigationExtras;
      names?: string[];
      extraDuration?: number;
      scrollToElementId?: string;
    }
  ) {
    if (!argument) argument = {};
    argument.scrollToElementId && this.scrollTo(argument.scrollToElementId);
    await this._transition.triggerTransition(
      argument.names,
      argument.extraDuration || 0
    );
    this._router.navigate(commands, argument.extras);
  }

  /** === Lazy loading ===*/
  /**
   * 懒加载 JS
   * Mark: https://codinglatte.com/posts/angular/lazy-loading-scripts-and-styles-angular/
   */
  public loadExternalScripts(url: string) {
    return new Promise(resolve => {
      const scriptElement = document.createElement("script");
      scriptElement.src = url;
      scriptElement.onload = resolve;
      document.body.appendChild(scriptElement);
    });
  }

  public loadExternalStyles(url: string) {
    return new Promise(resolve => {
      const styleElement = document.createElement("link");
      styleElement.href = url;
      styleElement.onload = resolve;
      document.head.appendChild(styleElement);
    });
  }
}

export { timeFormat, dateFormat, getKeyByValue } from "./time-format";
export { timeout } from "./promise-delay";
export { random } from "./random";
