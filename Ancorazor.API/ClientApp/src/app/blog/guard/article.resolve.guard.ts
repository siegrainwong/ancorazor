import { Injectable } from "@angular/core";
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from "@angular/router";
import ArticleModel from "../models/article-model";
import { ArticleService } from "../services/article.service";

@Injectable({
  providedIn: "root"
})
export class ArticleResolveGuard implements Resolve<ArticleModel> {
  constructor(private _service: ArticleService, private _router: Router) {}
  async resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<ArticleModel> {
    let identifier = route.paramMap.get("id");
    let res = await this._service.getArticle(identifier);
    if (!res) this._router.navigate(["/"]);
    return res;
  }
}
