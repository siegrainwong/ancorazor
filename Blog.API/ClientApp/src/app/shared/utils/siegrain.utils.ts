import { Injectable } from "@angular/core";
import { MatSnackBar, MatDialog } from "@angular/material";
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

export const topElementId = "#content";

@Injectable({ providedIn: "root" })
export class SGUtil {
  constructor(private _snackBar: MatSnackBar, private _dialog: MatDialog) {}

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

  /**
   * === Lazy loading ===
   **/

  /**
   * 懒加载 JS
   *
   * Mark: https://codinglatte.com/posts/angular/lazy-loading-scripts-and-styles-angular/
   */
  public loadExternalScripts(urls: string[]) {
    let promises: Promise<any>[] = [];
    urls.forEach(url => {
      promises.push(
        new Promise(resolve => {
          const scriptElement = document.createElement("script");
          scriptElement.src = url;
          scriptElement.onload = resolve;
          document.body.appendChild(scriptElement);
        })
      );
    });
    return Promise.all(promises);
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
