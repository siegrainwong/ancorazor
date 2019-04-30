import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { BlogAppComponent } from "./blog-app.component";
import { WriteArticleComponent } from "./components/write-article/write-article.component";
import { AboutComponent } from "./components/about/about.component";
import RouteData, { RouteKinds } from "../shared/models/route-data.model";
import { ArticleListComponent } from "./components/article-list/article-list.component";
import { ArticleComponent } from "./components/article/article.component";
import { AuthGuard } from "../shared/guard/auth.guard";
import {
  SGTransitionResolveGuard,
  TestObservableResolveGuard,
  TestDirectResolveGuard
} from "../shared/animations/sg-transition.resolve.guard";
import { ArticleResolveGuard } from "./guard/article.resolve.guard";
import { SGTransitionDeactivateGuard } from "../shared/animations/sg-transition.deactivate.guard";
import { ArticleListResolveGuard } from "./guard/article-list.resolve.guard";

const routes: Routes = [
  {
    path: "",
    component: BlogAppComponent,
    children: [
      {
        path: "",
        component: ArticleListComponent,
        data: new RouteData({ kind: RouteKinds.home }),
        resolve: {
          list: ArticleListResolveGuard
        }
      },
      {
        path: "index/:index",
        component: ArticleListComponent,
        data: new RouteData({ kind: RouteKinds.homePaged }),
        resolve: {
          list: ArticleListResolveGuard
        }
      },
      {
        path: "add",
        component: WriteArticleComponent,
        canActivate: [AuthGuard],
        data: new RouteData({ kind: RouteKinds.add })
      },
      {
        path: "edit/:id",
        component: WriteArticleComponent,
        canActivate: [AuthGuard],
        data: new RouteData({ kind: RouteKinds.edit })
      },
      {
        path: "article/:id",
        component: ArticleComponent,
        data: new RouteData({ kind: RouteKinds.article }),
        resolve: {
          article: ArticleResolveGuard
        }
      },
      {
        path: "about",
        component: AboutComponent,
        data: new RouteData({ kind: RouteKinds.about })
      }
    ]
  }
];
// setup SGTransition guards
routes[0].children.map(x => {
  if (!x.canDeactivate) x.canDeactivate = [SGTransitionDeactivateGuard];
  x.canDeactivate.push();
  if (!x.resolve) x.resolve = {};
  x.resolve.sg_transition = SGTransitionResolveGuard;
});

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule {}
