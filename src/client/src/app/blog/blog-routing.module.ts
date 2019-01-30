import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogAppComponent } from './blog-app.component';
import { PostListComponent } from './components/post-list/post-list.component';
import { RequireAuthenticatedUserRouteGuard } from '../shared/oidc/require-authenticated-user-route.guard';

const routes: Routes = [
  {
    path: '', component: BlogAppComponent,
    children: [
      /**
       * Mark: canActivate，在进入这个路由之前需要做的操作
       * 在这里就是鉴权
       */
      { path: 'post-list', component: PostListComponent, canActivate: [RequireAuthenticatedUserRouteGuard] },
      { path: '**', redirectTo: 'post-list' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule { }
