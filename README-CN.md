# Ancorazor

Ancorazor 是一个基于 .NET Core 2.2 和 Angular 7 的极简博客系统。

[Demo](https://siegrain.wang)

_项目依然在开发中，而且还没有做后台，不过前台也有基本的管理功能，处于勉强能用的阶段。_

## 启动项目

### 开发环境

确保您的环境已经有这些东西了：

1. .NET Core 2.2 SDK
2. Nodejs 10+
3. SQL Server(有 docker-compose 可以不用这个)

### 常规启动

1. `git clone https://github.com/siegrainwong/ancorazor.git`
2. 替换`ancorazor/Ancorazor.API/appsettings.Development.json`中的连接字符串(可选，取决于你本地的 SQL Server 配置，一般不需要替换)
3. 用 `cd path-to-ancorazor/Ancorazor.API` 进入目录后执行 `dotnet watch run`
4. 打开 `localhost:8088`, 默认用户名密码 admin/123456.

### docker-compose 启动

`cd path-to-ancorazor/build`

#### windows

把在`dev.ps1`里面这样的`F:\Projects\ancorazor\`路径字符串替换成你的，然后运行这个脚本

#### linux

运行 `path-to-ancorazor/build/dev.sh`

docker-compose 会将 sql server、skywalking、nginx 和 ancorazor 一并启动。

- Skywalking: `localhost:8080`, 默认用户名密码 is admin/admin.
- Ancorazor: `localhost:8088`, 默认用户名密码 is admin/123456.

## 发布(CI/CD)

推荐参考我在 azure devops 上的[pipeline](https://dev.azure.com/siegrainwong/Ancorazor/_build?definitionId=6)，都是公开的。

## 项目结构

TODO

## To-do

- [x] Comment
- [ ] Management page
- [ ] Search
- [ ] Categories & tags page
- [ ] Tests

或参考 [project](https://github.com/Seanwong933/ancorazor/projects/1).

## 致谢

[模板: startbootstrap-clean-blog](https://github.com/BlackrockDigital/startbootstrap-clean-blog)

## Licence

Anti-996 & MIT
