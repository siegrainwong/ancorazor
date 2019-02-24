import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { BlogRoutingModule } from "./blog-routing.module";
import { MaterialModule } from "../shared/material/material.module";
import { BlogAppComponent } from "./blog-app.component";
import { NavComponent } from "./components/nav/nav.component";
import { ArticleService } from "./services/article.service";
import { ArticleListComponent } from "./components/article-list/article-list.component";
import { ArticleItemComponent } from "./components/article-item/article-item.component";
import { WriteArticleComponent } from "./components/write-article/write-article.component";
import { HeaderComponent } from "./components/header/header.component";
import { FooterComponent } from "./components/footer/footer.component";
import { HomeComponent } from "./components/home/home.component";
import { AboutComponent } from "./components/about/about.component";
import { ArticleComponent } from "./components/article/article.component";
import { CodemirrorModule } from "@ctrl/ngx-codemirror";
import { FormsModule } from "@angular/forms";

@NgModule({
  declarations: [
    BlogAppComponent,
    NavComponent,
    ArticleListComponent,
    ArticleItemComponent,
    WriteArticleComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    AboutComponent,
    ArticleComponent
  ],
  imports: [
    CommonModule,
    BlogRoutingModule,
    MaterialModule,
    FormsModule,
    CodemirrorModule
  ],
  providers: [ArticleService]
})
export class BlogModule {
  constructor() {
    console.log("blog ctor.");
  }
}
