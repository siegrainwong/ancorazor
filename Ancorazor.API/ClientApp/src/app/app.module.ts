import {
  BrowserModule,
  BrowserTransferStateModule
} from "@angular/platform-browser";
import { NgModule, ErrorHandler, Inject, PLATFORM_ID } from "@angular/core";

import { AppRoutingModule } from "./app.routing";
import { AppComponent } from "./app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { LoggingService } from "./shared/services/logging.service";
import { GlobalErrorHandler } from "./shared/services/global-error-handler";
import { Store } from "./shared/store/store";
import { isPlatformBrowser } from "@angular/common";
import { TaskWrapper } from "./shared/services/async-helper.service";

@NgModule({
  declarations: [AppComponent],
  imports: [
    // Add .withServerTransition() to support Universal rendering.
    // The application ID can be any identifier which is unique on
    // the page.
    BrowserModule.withServerTransition({
      appId: `app-ancore`
    }),
    BrowserTransferStateModule,
    AppRoutingModule,
    BrowserAnimationsModule
  ],
  providers: [
    LoggingService,
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    },
    TaskWrapper
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(
    logger: LoggingService,
    store: Store,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    logger.info("app.module ctor.");
    store.renderFromClient = isPlatformBrowser(platformId);
  }
}
