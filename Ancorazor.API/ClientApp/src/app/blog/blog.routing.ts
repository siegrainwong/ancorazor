import { NgModule } from "@angular/core";
import { Routes, RouterModule, UrlSegment, Route } from "@angular/router";
import { BlogAppComponent } from "./blog-app.component";
import { WriteArticleComponent } from "./components/write-article/write-article.component";
import { AboutComponent } from "./components/about/about.component";
import RouteData, { RouteKinds } from "../shared/models/route-data.model";
import { ArticleListComponent } from "./components/article-list/article-list.component";
import { ArticleComponent } from "./components/article/article.component";
import { AuthGuard } from "../shared/guard/auth.guard";
import { SGTransitionResolveGuard } from "../shared/animations/sg-transition.resolve.guard";
import { ArticleResolveGuard } from "./guard/article.resolve.guard";
import { SGTransitionDeactivateGuard } from "../shared/animations/sg-transition.deactivate.guard";
import { ArticleListResolveGuard } from "./guard/article-list.resolve.guard";
import { PageNotFoundComponent } from "./components/page-not-found/page-not-found.component";

/**
 * MARK: SGTransitionDeactivateGuard && SGTransitionResolveGuard Setup
 *
 * 这里是因为`Production`是`AOT`编译的原因（或者说开启`angular-cli`的`optimization`）
 * 必须要把两个`Guard`“声明”在每个需要动画的`Route`上才能在生产环境使用
 * 否则如果是`JIT`的话直接`routes.map`就不用这样麻烦了
 */
let routes: Routes = [
  {
    path: "",
    component: BlogAppComponent,
    children: [
      {
        path: "",
        component: ArticleListComponent,
        data: new RouteData({ kind: RouteKinds.home }),
        resolve: {
          list: ArticleListResolveGuard,
          sg_transition: SGTransitionResolveGuard
        },
        canDeactivate: [SGTransitionDeactivateGuard]
      },
      {
        path: "index/:index",
        component: ArticleListComponent,
        data: new RouteData({ kind: RouteKinds.homePaged }),
        resolve: {
          list: ArticleListResolveGuard,
          sg_transition: SGTransitionResolveGuard
        },
        canDeactivate: [SGTransitionDeactivateGuard]
      },
      {
        path: "add",
        component: WriteArticleComponent,
        canActivate: [AuthGuard],
        data: new RouteData({ kind: RouteKinds.add }),
        resolve: {
          sg_transition: SGTransitionResolveGuard
        },
        canDeactivate: [SGTransitionDeactivateGuard]
      },
      {
        path: "edit/:id",
        component: WriteArticleComponent,
        canActivate: [AuthGuard],
        data: new RouteData({ kind: RouteKinds.edit }),
        resolve: {
          article: ArticleResolveGuard,
          sg_transition: SGTransitionResolveGuard
        },
        canDeactivate: [SGTransitionDeactivateGuard]
      },
      {
        path: "article/:id",
        component: ArticleComponent,
        data: new RouteData({ kind: RouteKinds.article }),
        resolve: {
          article: ArticleResolveGuard,
          sg_transition: SGTransitionResolveGuard
        },
        canDeactivate: [SGTransitionDeactivateGuard],
        matcher: articleRouteMatcher
      },
      {
        path: "about",
        component: AboutComponent,
        data: new RouteData({ kind: RouteKinds.about }),
        resolve: {
          sg_transition: SGTransitionResolveGuard
        },
        canDeactivate: [SGTransitionDeactivateGuard]
      },
      {
        path: "notfound",
        component: PageNotFoundComponent,
        data: new RouteData({ kind: RouteKinds.notfound }),
        resolve: {
          sg_transition: SGTransitionResolveGuard
        },
        canDeactivate: [SGTransitionDeactivateGuard]
      },
      { path: "client/:id", redirectTo: "/" },
      { path: "**", redirectTo: "/notfound" }
    ]
  }
];

export function articleRouteMatcher(segments: UrlSegment[]) {
  if (!segments.length || segments[0].path !== "article") return null;
  return {
    consumed: segments,
    posParams: { id: segments[segments.length - 1] }
  };
}

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule {}
