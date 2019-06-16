import ArticleModel from "src/app/blog/models/article-model";
import { SGTransitionCommands } from "../animations/sg-transition.model";
import { PagedResult } from "./response-result";

export const enum RouteKinds {
  home = "home",
  homePaged = "homePaged",
  add = "add",
  edit = "edit",
  article = "article",
  about = "about",
  notfound = "notfound"
}

export default class RouteData {
  kind: string;
  /** resolved from `ArticleResolveGuard` */
  article?: ArticleModel;
  /** resolved from `ArticleListResolveGuard` */
  list?: PagedResult<ArticleModel>;
  /** resolved from `SGTransitionResolveGuard` */
  sg_transition?: SGTransitionCommands;
  constructor(obj?: Partial<RouteData>) {
    Object.assign(this, obj);
  }
}
