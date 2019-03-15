import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from "@angular/router";
import { Store } from "../store/store";
import { SGUtil } from "../utils/siegrain.utils";

@Injectable({
  providedIn: "root"
})
export class AuthGuard implements CanActivate {
  constructor(private _store: Store, private _util: SGUtil) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const canActivate = this._store.userIsAvailable;
    if (!canActivate)
      this._util.routeTo(["/"], { extras: { fragment: "sign-in" } });
    return canActivate;
  }
}
