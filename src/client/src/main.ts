import "codemirror/mode/javascript/javascript";
import "codemirror/mode/markdown/markdown";

import { enableProdMode } from "@angular/core";
import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";

import { AppModule } from "./app/app.module";
import { environment } from "./environments/environment";

import "hammerjs";

// SSR: Modified

/**
 * Load scripts while its browser
 */
if (window) {
  require("jquery");
  require("bootstrap/dist/js/bootstrap.bundle.min");
}

export function getBaseUrl() {
  return document.getElementsByTagName("base")[0].href;
}

const providers = [{ provide: "BASE_URL", useFactory: getBaseUrl, deps: [] }];

if (environment.production) {
  enableProdMode();
}

// platformBrowserDynamic(providers)
//   .bootstrapModule(AppModule)
//   .catch(err => console.error(err));

document.addEventListener("DOMContentLoaded", () => {
  platformBrowserDynamic(providers)
    .bootstrapModule(AppModule)
    .catch(err => console.log(err));
});
