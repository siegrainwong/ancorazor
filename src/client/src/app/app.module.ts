import { BrowserModule } from "@angular/platform-browser";
import { NgModule, ErrorHandler } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { RequireAuthenticatedUserRouteGuard } from "./shared/oidc/require-authenticated-user-route.guard";
import { SigninOidcComponent } from "./shared/oidc/signin-oidc/signin-oidc.component";
import { RedirectSilentRenewComponent } from "./shared/oidc/redirect-silent-renew/redirect-silent-renew.component";
import { OpenIdConnectService } from "./shared/oidc/open-id-connect.service";
import { LoggingService } from "./shared/services/logging.service";
import { GlobalErrorHandler } from "./shared/services/global-error-handler";

@NgModule({
  declarations: [
    AppComponent,
    SigninOidcComponent,
    RedirectSilentRenewComponent
  ],
  imports: [
    // Add .withServerTransition() to support Universal rendering.
    // The application ID can be any identifier which is unique on
    // the page.
    // TODO: configurable
    BrowserModule.withServerTransition({ appId: "siegrain.blog" }), // SSR: Modified
    // Add TransferHttpCacheModule to install a Http interceptor
    AppRoutingModule,
    BrowserAnimationsModule
  ],
  providers: [
    LoggingService,
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    },
    OpenIdConnectService,
    RequireAuthenticatedUserRouteGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(logger: LoggingService) {
    logger.info("app.module ctor.");
  }
}
