import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { Routes, RouterModule } from "@angular/router";
import { RequireAuthenticatedUserRouteGuard } from "./shared/oidc/require-authenticated-user-route.guard";
import { SigninOidcComponent } from "./shared/oidc/signin-oidc/signin-oidc.component";
import { RedirectSilentRenewComponent } from "./shared/oidc/redirect-silent-renew/redirect-silent-renew.component";
import { OpenIdConnectService } from "./shared/oidc/open-id-connect.service";
import { BlogAppComponent } from "./blog/blog-app.component";
import { BlogModule } from "./blog/blog.module";
import { Variables } from "./shared/variables";

const routes: Routes = [
  { path: "blog", loadChildren: "./blog/blog.module#BlogModule" },
  { path: "signin-oidc", component: SigninOidcComponent },
  { path: "redirect-silentrenew", component: RedirectSilentRenewComponent },
  { path: "**", component: BlogAppComponent }
];
@NgModule({
  declarations: [
    AppComponent,
    SigninOidcComponent,
    RedirectSilentRenewComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "app" }), // SSR: Modified
    AppRoutingModule,
    RouterModule.forRoot(routes),
    BrowserAnimationsModule,
    BlogModule
  ],
  providers: [OpenIdConnectService, RequireAuthenticatedUserRouteGuard],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(variables: Variables) {
    variables.renderFromServer = false;
  }
}
