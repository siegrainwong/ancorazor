import { WebStorageStateStore } from "src/libraries/oidc-client-js-dev";
import { CookieStorage } from "cookie-storage";

// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

const clientHost = "https://localhost:5001";
const apiHost = "https://localhost:5001";
const idServerHost = "https://localhost:7000";

export const environment = {
  production: false,
  apiUrlBase: `${apiHost}/api`,
  // oidc
  openIdConnectSettings: {
    authority: idServerHost,

    client_id: "siegrain-blog-client",
    scope: "openid profile email restapi",
    response_type: "id_token token",

    // 登录后跳转地址
    redirect_uri: `${clientHost}/signin-oidc`,
    // 注销后跳转地址
    post_logout_redirect_uri: `${clientHost}`,

    // userStore: new WebStorageStateStore({ store: window.localStorage }),
    // userStore: new WebStorageStateStore({ store: CookieStorage }),

    // 静默更新用户的 Token
    automaticSilentRenew: true,
    silent_redirect_uri: `${clientHost}/redirect-silentrenew`
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
