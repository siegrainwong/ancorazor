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
import { LoggingService } from "src/app/shared/services/logging.service";
import { ArticleParameters } from "../models/article-parameters";
import { PagedResult } from "src/app/shared/models/response-result";
import { timeout } from "src/app/shared/utils/promise-delay";

@Injectable({
  providedIn: "root"
})
export class ArticleListResolveGuard
  implements Resolve<PagedResult<ArticleModel>> {
  private _parameter = new ArticleParameters();
  constructor(
    private _service: ArticleService,
    private _transitionStore: SGTransitionStore,
    private _logger: LoggingService
  ) {}
  async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let index = parseInt(route.params.index) || 0;
    if (typeof index != "number") return null;

    this._parameter.pageIndex = index;
    let res = await this._service.getPagedArticles(this._parameter);
    if (!res)
      this._logger.error("can't resolve data in ArticleListResolveGuard");

    await timeout(3000);
    this._transitionStore.setResolved();
    return res;
  }
}
