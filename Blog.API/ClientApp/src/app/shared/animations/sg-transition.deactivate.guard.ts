import { Injectable } from "@angular/core";
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanDeactivate
} from "@angular/router";
import { SGTransitionDelegate } from "./sg-transition.delegate";
import { SGTransitionStore } from "./sg-transition.store";

@Injectable({
  providedIn: "root"
})
export class SGTransitionDeactivateGuard<T extends SGTransitionDelegate>
  implements CanDeactivate<T> {
  constructor(private _transitionStore: SGTransitionStore) {}
  async canDeactivate(
    component: T,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot
  ): Promise<boolean> {
    this._transitionStore.transitionDelegate = component;
    console.log(
      "component is deactivating, delegate set with component: ",
      component
    );
    return true;
  }
}
