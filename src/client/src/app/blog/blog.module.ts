import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlogRoutingModule } from './blog-routing.module';
import { MaterialModule } from '../shared/material/material.module';
import { BlogAppComponent } from './blog-app.component';
import { NavComponent } from './components/nav/nav.component'
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { PostService } from './services/post.service';
import { ArticleListComponent } from './components/article-list/article-list.component';
import { ArticleItemComponent } from './components/article-item/article-item.component';

@NgModule({
  declarations: [
    BlogAppComponent,
    NavComponent,
    ToolbarComponent,
    ArticleListComponent,
    ArticleItemComponent
  ],
  imports: [
    CommonModule,
    BlogRoutingModule,
    MaterialModule
  ],
  providers: [
    PostService
  ]
})
export class BlogModule { }
