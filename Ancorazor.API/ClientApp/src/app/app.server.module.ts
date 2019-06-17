import { NgModule } from "@angular/core";
import {
  ServerModule,
  ServerTransferStateModule
} from "@angular/platform-server";
import { ModuleMapLoaderModule } from "@nguniversal/module-map-ngfactory-loader";

import { AppModule } from "./app.module";
import { AppComponent } from "./app.component";
import { LoggingService } from "./shared/services/logging.service";

/**
 * Mark: 在 SSR 下由于是AOT编译，所有 es6 modules 不能用 export default
 * 相关参考：
 * https://stackoverflow.com/questions/45962317/why-isnt-export-default-recommended-in-angular/45963067
 * https://github.com/UltimateAngular/aot-loader/wiki/Limitations-with-AoT
 * https://www.zhihu.com/question/297101183
 */

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
  constructor(logger: LoggingService) {
    logger.info("app.server ctor.");
  }
}
