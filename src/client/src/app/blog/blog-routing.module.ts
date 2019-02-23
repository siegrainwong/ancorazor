import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { BlogAppComponent } from "./blog-app.component";
import { RequireAuthenticatedUserRouteGuard } from "../shared/oidc/require-authenticated-user-route.guard";
import { WriteArticleComponent } from "./components/write-article/write-article.component";
import { AboutComponent } from "./components/about/about.component";
import { HomeComponent } from "./components/home/home.component";
import { ArticleComponent } from "./components/article/article.component";
import RouteData from "../shared/models/route-data.model";

const routes: Routes = [
  {
    path: "",
    component: BlogAppComponent,
    children: [
      {
        path: "",
        component: HomeComponent,
        data: new RouteData("home")
      },
      /**
       * Mark: canActivate，在进入这个路由之前需要做的操作
       * 在这里就是鉴权，其实这一坨就是类似Attribute的东西，也可以有多个
       */
      {
        path: "add",
        component: WriteArticleComponent,
        canActivate: [RequireAuthenticatedUserRouteGuard],
        data: new RouteData("add")
      },
      {
        path: "article/:id",
        component: ArticleComponent,
        data: new RouteData("article")
      },
      { path: "about", component: AboutComponent, data: new RouteData("about") }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule {}
