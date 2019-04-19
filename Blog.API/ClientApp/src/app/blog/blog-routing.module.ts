import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { BlogAppComponent } from "./blog-app.component";
import { WriteArticleComponent } from "./components/write-article/write-article.component";
import { AboutComponent } from "./components/about/about.component";
import RouteData from "../shared/models/route-data.model";
import { ArticleListComponent } from "./components/article-list/article-list.component";
import { ArticleComponent } from "./components/article/article.component";
import { AuthGuard } from "../shared/guard/auth.guard";
import { SGTransitionToLeaveGuard } from "../shared/animations/sg-transition-to-leave.deactivate.guard";

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
        path: ":index",
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
        data: new RouteData({ kind: "edit" })
      },
      {
        path: "article/:id",
        component: ArticleComponent,
        data: new RouteData({ kind: "article" })
      },
      {
        path: "about",
        component: AboutComponent,
        data: new RouteData({ kind: "about" })
      }
    ]
  }
];
// guard for transition
routes[0].children.map(x => (x.canDeactivate = [SGTransitionToLeaveGuard]));

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule {}
