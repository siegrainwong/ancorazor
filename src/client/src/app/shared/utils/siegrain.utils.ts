import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { Router, NavigationExtras } from "@angular/router";
import { type, SGTransition } from "./siegrain.animations";

export const enum TipType {
  Info,
  Warn,
  Error
}

@Injectable()
export class SGUtil {
  constructor(
    private snackBar: MatSnackBar,
    private router: Router,
    private transition: SGTransition
  ) {}

  /**##### Utilities */
  tip(msg: string, type: TipType = TipType.Error) {
    this.snackBar.open(`❌ ${msg}`, null, {
      duration: 5000
    });
  }

  async routeTo(commands: any[], extras?: NavigationExtras) {
    await this.transition.triggerTransition(type.route);
    this.router.navigate(commands, extras);
  }
}

export { timeFormat, dateFormat, getKeyByValue } from "./time-format";
export { timeout } from "./promise-delay";
export { random } from "./random";
