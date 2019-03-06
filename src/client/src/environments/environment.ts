import { random } from "src/app/shared/utils/random";

// host settings
// const clientHost = "http://localhost:4200";
const clientHost = "https://localhost:5001";
const apiHost = "https://localhost:5001";
const idServerHost = "https://localhost:7000";

export const environment = {
  production: false,

  // customizations
  title: "siegrain🌌wang",
  homeCoverUrl: `assets/img/bg${random(1, 7)}.jpg`,

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
