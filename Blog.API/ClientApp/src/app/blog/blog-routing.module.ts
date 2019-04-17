import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { BlogAppComponent } from "./blog-app.component";
import { WriteArticleComponent } from "./components/write-article/write-article.component";
import { AboutComponent } from "./components/about/about.component";
import RouteData from "../shared/models/route-data.model";
import { ArticleListComponent } from "./components/article-list/article-list.component";
import { ArticleComponent } from "./components/article/article.component";
import { AuthGuard } from "../shared/guard/auth.guard";
import { ArticleResolver } from "./services/article.resolver";
import { SGBaseResolver } from "../shared/services/siegrain.animation.resolver";

const routes: Routes = [
  {
    path: "",
    component: BlogAppComponent,
    children: [
      {
        path: "",
        component: ArticleListComponent,
        data: new RouteData({ kind: "home" })
      },
      {
        path: "home",
        component: ArticleListComponent,
        data: new RouteData({ kind: "home" })
      },
      {
        path: "add",
        component: WriteArticleComponent,
        canActivate: [AuthGuard],
        data: new RouteData({ kind: "add" })
      },
      {
        path: "edit/:id",
        component: WriteArticleComponent,
        canActivate: [AuthGuard],
        data: new RouteData({ kind: "edit" }),
        resolve: {
          article: ArticleResolver
        }
      },
      {
        path: "article/:id",
        component: ArticleComponent,
        data: new RouteData({ kind: "article" }),
        resolve: {
          article: ArticleResolver
        }
      },
      {
        path: "about",
        component: AboutComponent,
        data: new RouteData({ kind: "about" }),
        resolve: {
          article: SGBaseResolver
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule {}
