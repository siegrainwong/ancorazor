import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { SGTransitionToLeaveGuard } from "src/app/shared/animations/sg-transition-to-leave.deactivate.guard";
import ArticleModel from "../models/article-model";
import { CanComponentTransitionToLeave } from "src/app/shared/animations/sg-transition.interface";

export interface CanArticleResolvedBeforeTransition
  extends CanComponentTransitionToLeave {
  CanArticleResolvedBeforeTransition?: () =>
    | ArticleModel
    | Promise<ArticleModel>;
}

@Injectable({
  providedIn: "root"
})
export class ArticleResolveGuard extends SGTransitionToLeaveGuard<
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
