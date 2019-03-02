import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material";

export const enum TipType {
  Info,
  Warn,
  Error
}

@Injectable()
export class SGUtil {
  constructor(private snackBar: MatSnackBar) {}

  tip(msg: string, type: TipType = TipType.Error) {
    this.snackBar.open(`❌ ${msg}`, null, {
      duration: 5000
    });
  }
}

export { timeFormat, dateFormat, getKeyByValue } from "./time-format";
export { timeout } from "./promise-delay";
export { random } from "./random";
