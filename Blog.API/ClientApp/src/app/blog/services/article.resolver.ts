import { Injectable } from "@angular/core";
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from "@angular/router";
import { ArticleService } from "./article.service";
import { SGBaseResolver } from "src/app/shared/services/base.resolver";
import { Store } from "src/app/shared/store/store";
import ArticleModel from "../models/article-model";

@Injectable({
  providedIn: "root"
})
export class ArticleResolver extends SGBaseResolver
  implements Resolve<ArticleModel> {
  constructor(private _service: ArticleService, private _router: Router) {
    super();
  }
  async resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<ArticleModel> {
    let id = route.paramMap.get("id");
    let res = await this._service.getArticle(parseInt(id));
    if (!res) this._router.navigate(["/"]);

    await super.resolve(route, state);
    return res;
  }
}
