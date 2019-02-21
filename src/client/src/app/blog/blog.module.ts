import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { BlogRoutingModule } from "./blog-routing.module";
import { MaterialModule } from "../shared/material/material.module";
import { BlogAppComponent } from "./blog-app.component";
// import { NavComponent } from "./components/nav/nav.component";
import { ToolbarComponent } from "./components/toolbar/toolbar.component";
import { ArticleService } from "./services/article.service";
import { ArticleListComponent } from "./components/article-list/article-list.component";
import { ArticleItemComponent } from "./components/article-item/article-item.component";
import { WriteArticleComponent } from "./components/write-article/write-article.component";
import { LayoutComponent } from './components/layout/layout.component';
import { HeaderComponent } from './components/header/header.component';

@NgModule({
  declarations: [
    BlogAppComponent,
    // NavComponent,
    ToolbarComponent,
    ArticleListComponent,
    ArticleItemComponent,
    WriteArticleComponent,
    LayoutComponent,
    HeaderComponent
  ],
  imports: [CommonModule, BlogRoutingModule, MaterialModule],
  providers: [ArticleService]
})
export class BlogModule {}
