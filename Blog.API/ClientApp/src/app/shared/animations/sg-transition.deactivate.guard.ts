import { Injectable } from "@angular/core";
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanDeactivate
} from "@angular/router";
import { SGTransitionDelegate } from "./sg-transition.delegate";
import { SGTransitionStore } from "./sg-transition.store";
import { LoggingService } from "../services/logging.service";

@Injectable({
  providedIn: "root"
})
export class SGTransitionDeactivateGuard<T extends SGTransitionDelegate>
  implements CanDeactivate<T> {
  constructor(
    private _transitionStore: SGTransitionStore,
    private _logger: LoggingService
  ) {}
  async canDeactivate(
    component: T,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot
  ): Promise<boolean> {
    this._transitionStore.transitionDelegate = component;
    this._logger.info(
      "component is deactivating, delegate set with component: ",
      component
    );
    return true;
  }
}
