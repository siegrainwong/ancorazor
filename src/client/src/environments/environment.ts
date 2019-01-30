// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiUrlBase: 'https://localhost:5001/api',
  // oidc
  openIdConnectSettings: {
    authority: 'https://localhost:5033/',

    client_id: 'siegrain-blog-client',
    scope: 'openid profile email restapi',
    response_type: 'id_token token',

    // 登录后跳转地址
    redirect_uri: 'http://localhost:4200/signin-oidc',
    // 注销后跳转地址
    post_logout_redirect_uri: 'http://localhost:4200/',

    // 静默更新用户的 Token
    automaticSilentRenew: true,
    silent_redirect_uri: 'http://localhost:4200/redirect-silentrenew'
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
