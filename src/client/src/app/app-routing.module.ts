import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// import { SigninOidcComponent } from "./shared/oidc/signin-oidc/signin-oidc.component";
// import { RedirectSilentRenewComponent } from "./shared/oidc/redirect-silent-renew/redirect-silent-renew.component";

const routes: Routes = [
  { path: "", loadChildren: "./blog/blog.module#BlogModule" },
  // { path: "signin-oidc", component: SigninOidcComponent },
  // { path: "redirect-silentrenew", component: RedirectSilentRenewComponent },
  { path: "**", redirectTo: "home" }
];
@NgModule({
  // https://github.com/angular/angular/issues/15716
  // Mark: 修复从 Server render 到 Client render 的闪烁
  // TODO: 闪是不闪了，但依然会掉样式。
  imports: [RouterModule.forRoot(routes, { initialNavigation: "enabled" })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
