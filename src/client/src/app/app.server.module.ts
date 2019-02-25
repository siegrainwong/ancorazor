import { NgModule } from "@angular/core";
import {
  ServerModule,
  ServerTransferStateModule
} from "@angular/platform-server";
import { ModuleMapLoaderModule } from "@nguniversal/module-map-ngfactory-loader";

import { AppModule } from "./app.module";
import { AppComponent } from "./app.component";
import { Variables } from "./shared/variables";

// const DefinePlugin = require("webpack/lib/DefinePlugin");
// new DefinePlugin({
//   window: undefined,
//   document: undefined
// });
// SSR: Added

@NgModule({
  imports: [
    // The AppServerModule should import your AppModule followed
    // by the ServerModule from @angular/platform-server.
    AppModule,
    ServerModule,
    ModuleMapLoaderModule, // <-- *Important* to have lazy-loaded routes work
    ServerTransferStateModule
  ],
  // Since the bootstrapped component is not inherited from your
  // imported AppModule, it needs to be repeated here.
  bootstrap: [AppComponent]
})
export class AppServerModule {
  constructor(variables: Variables) {
    console.log("app.server ctor.");
    variables.renderFromServer = true;
  }
}
