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

export const topElementId = "#body";
export const XSRFTokenKey = "XSRF-TOKEN";

@Injectable({ providedIn: "root" })
export class SGUtil {
  private _loadedScripts = new Set<string>();
  constructor(private _snackBar: MatSnackBar, private _dialog: MatDialog) {}

  /** === Utilities ===*/

  /**
   * ref: `MatSnackBar`
   * @param msg 消息内容
   * @param type 消息类型
   */
  public tip(msg: string, type: TipType = TipType.Danger) {
    this._snackBar.open(`${type} ${msg}`, "Got it", {
      duration: 5000 + type === TipType.Danger ? 15000 : 0
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
   * 获取 Cookie
   * @param name
   */
  public getCookie(name: string): string {
    const nameLenPlus = name.length + 1;
    return (
      document.cookie
        .split(";")
        .map(c => c.trim())
        .filter(cookie => {
          return cookie.substring(0, nameLenPlus) === `${name}=`;
        })
        .map(cookie => {
          return decodeURIComponent(cookie.substring(nameLenPlus));
        })[0] || null
    );
  }

  /**
   * Convert object key from pascal to camel case
   * @param o
   */
  public toCamel(o: object) {
    let newO, origKey, newKey, value;
    if (o instanceof Array) {
      return o.map(function(value) {
        if (typeof value === "object") {
          value = this.toCamel(value);
        }
        return value;
      });
    } else {
      newO = {};
      for (origKey in o) {
        if (o.hasOwnProperty(origKey)) {
          newKey = (
            origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey
          ).toString();
          value = o[origKey];
          if (
            value instanceof Array ||
            (value !== null && value.constructor === Object)
          ) {
            value = this.toCamel(value);
          }
          newO[newKey] = value;
        }
      }
    }
    return newO;
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
      if (this._loadedScripts.has(url)) return;
      promises.push(
        new Promise(resolve => {
          const scriptElement = document.createElement("script");
          scriptElement.src = url;
          scriptElement.onload = () => {
            resolve();
            this._loadedScripts.add(url);
          };
          document.body.appendChild(scriptElement);
        })
      );
    });
    return Promise.all(promises);
  }
}
