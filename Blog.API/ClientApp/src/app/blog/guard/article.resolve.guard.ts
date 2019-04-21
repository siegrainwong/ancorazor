import { Injectable } from "@angular/core";
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from "@angular/router";
import ArticleModel from "../models/article-model";
import { SGTransitionStore } from "src/app/shared/animations/sg-transition.store";
import { ArticleService } from "../services/article.service";
import { timeout } from "src/app/shared/utils/promise-delay";

@Injectable({
  providedIn: "root"
})
export class ArticleResolveGuard implements Resolve<ArticleModel> {
  constructor(
    private _service: ArticleService,
    private _router: Router,
    private _transitionStore: SGTransitionStore
  ) {}
  async resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<ArticleModel> {
    let id = route.paramMap.get("id");
    let res = await this._service.getArticle(parseInt(id));
    if (!res) this._router.navigate(["/"]);

    this._transitionStore.setResolved();
    return res;
  }
}
