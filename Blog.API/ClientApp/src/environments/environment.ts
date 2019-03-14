import { random } from "src/app/shared/utils/random";

const apiHost = "http://localhost:8088";

export const environment = {
  production: false,

  // customizations
  title: "siegrain🌌wang",
  titlePlainText: "siegrain.wang",
  homeCoverUrl: `assets/img/bg${random(1, 7)}.jpg`,

  apiUrlBase: `${apiHost}/api`,
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
