import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogAppComponent } from './blog-app.component';
import { PostListComponent } from './components/post-list/post-list.component';

const routes: Routes = [
  {
    path: '', component: BlogAppComponent,
    children: [
      { path: 'post-list', component: PostListComponent },
      { path: '**', redirectTo: 'post-list' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule { }
