import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { Store } from "../store/store";
import { timeout } from "./promise-delay";
import { Router } from "@angular/router";
import { environment } from "src/environments/environment";
import { animations } from "./animations";

export const enum TipType {
  Info,
  Warn,
  Error
}

@Injectable()
export class SGUtil {
  constructor(
    private snackBar: MatSnackBar,
    private store: Store,
    private router: Router
  ) {}

  tip(msg: string, type: TipType = TipType.Error) {
    this.snackBar.open(`❌ ${msg}`, null, {
      duration: 5000
    });
  }

  async routeTo(url: string) {
    this.store.isLeaving = true;
    await timeout(environment.routeAnimationDuration.duration);
    this.store.isLeaving = false;
    this.router.navigate([url]);
  }

  animationClass(type) {
    let animation = {};
    animation[animations[type].enter] = !this.store.isLeaving;
    animation[animations[type].leave] = this.store.isLeaving;
    animation[environment.routeAnimationDuration.name] = true;
    return animation;
  }
}

export { timeFormat, dateFormat, getKeyByValue } from "./time-format";
export { timeout } from "./promise-delay";
export { random } from "./random";
export { animations } from "./animations";
