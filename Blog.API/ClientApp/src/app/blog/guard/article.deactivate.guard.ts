import { Injectable } from "@angular/core";
import {
  CanDeactivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree
} from "@angular/router";
import { Observable } from "rxjs";
import {
  LeavingAnimationGuard,
  CanComponentTransitionToLeave
} from "src/app/shared/guard/transition-leaving.deactivate.guard";
import ArticleModel from "../models/article-model";

export interface CanArticleResolvedBeforeTransition
  extends CanComponentTransitionToLeave {
  CanArticleResolvedBeforeTransition?: () =>
    | ArticleModel
    | Promise<ArticleModel>;
}

@Injectable({
  providedIn: "root"
})
export class ArticleResolveGuard extends LeavingAnimationGuard<
  CanArticleResolvedBeforeTransition
> {
  async canDeactivate(
    component: CanArticleResolvedBeforeTransition,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot
  ) {
    return await super.canDeactivate(
      component,
      currentRoute,
      currentState,
      nextState
    );
  }
}
