# Siegrain·Blog

[![Build status](https://dev.azure.com/siegrainwong/SGBlogCore/_apis/build/status/SGBlogCore-Azure%20Web%20App%20CI)](https://dev.azure.com/siegrainwong/SGBlogCore/_build/latest?definitionId=2)

## 故障排除

1. 发布后网站没有更新、看到的是老内容。
   在 release stage 中加入 azure cli task 来重启 azure webapp
   `az webapp restart --resource-group xxx --name xxx`
   Ref: [Deployment vs runtime issues
   ](https://github.com/projectkudu/kudu/wiki/Deployment-vs-runtime-issues#deployments-and-web-app-restarts%22)
