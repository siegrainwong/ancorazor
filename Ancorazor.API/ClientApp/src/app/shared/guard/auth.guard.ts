import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from "@angular/router";
import { Store } from "../store/store";

@Injectable({
  providedIn: "root"
})
export class AuthGuard implements CanActivate {
  constructor(private _store: Store, private _router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const canActivate = this._store.userIsAvailable;
    if (!canActivate)
      this._router.navigate(
        ["/"],
        this._store.renderFromClient ? { fragment: "sign-in" } : {}
      );
    return canActivate;
  }
}
