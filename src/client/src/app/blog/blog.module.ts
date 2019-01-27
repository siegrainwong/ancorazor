import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlogRoutingModule } from './blog-routing.module';
import { MaterialModule } from '../shared/material/material.module';
import { BlogAppComponent } from './blog-app.component';
import { NavComponent } from './components/nav/nav.component'
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { PostService } from './services/post.service';
import { PostListComponent } from './components/post-list/post-list.component';

@NgModule({
  declarations: [
    BlogAppComponent,
    NavComponent,
    ToolbarComponent,
    PostListComponent
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
