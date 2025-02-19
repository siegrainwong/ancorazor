# Ancorazor

[![Codacy grade](https://img.shields.io/codacy/grade/00a15dd7811e42b7ae6aea01a966fee0.svg?logo=codacy&style=for-the-badge)](https://app.codacy.com/app/siegrainwong/ancorazor?utm_source=github.com&utm_medium=referral&utm_content=siegrainwong/ancorazor&utm_campaign=Badge_Grade_Dashboard)
[![Azure DevOps builds](https://img.shields.io/azure-devops/build/siegrainwong/75cdd93a-e41e-4158-ace3-88dab60c3343/6.svg?label=azure%20pipelines&logo=azure%20pipelines&style=for-the-badge)](https://dev.azure.com/siegrainwong/Ancorazor/_build/latest?definitionId=6&branchName=master)
[![LICENSE](https://img.shields.io/badge/license-Anti--996%20&%20MIT-blue.svg?style=for-the-badge)](https://github.com/996icu/996.ICU/blob/master/LICENSE)

[English Readme](https://github.com/siegrainwong/ancorazor/blob/master/README-EN.md)

---

Ancorazor 是一个基于 .NET Core 2.2 和 Angular 7 的极简博客系统。

[Demo](https://siegrain.wang)

_项目依然在开发中，而且还没有做后台，不过前台也有基本的管理功能，处于勉强能用的阶段。_

## 演示

GIF 3M

![ancorazor gif demostration](https://s2.ax1x.com/2019/06/28/ZMxQs0.gif)

转场动画模块是我花了较大精力基于`animate.css`写的，因为觉得`Angular animation`不太好用，2333。

## 启动项目

### 开发环境

确保您的环境已经有这些东西了：

1. .NET Core 2.2 SDK
2. Nodejs 12
3. SQL Server(有 docker-compose 可以不用这个)

### 2025-02-08 更新启动方式

出于一些奇怪的原因最近又要跑这个项目做演示，更新一下启动方式。

1. 安装 [nvm-windows](https://github.com/coreybutler/nvm-windows)、安装 .NET Core 2.2 SDK、安装 SQL Express 2022
2. `dotnet restore`
3. `nvm install 12`，报错说找不到 npm 就去 [nodejs previous releases](https://nodejs.org/zh-cn/about/previous-releases) 里找到 12 的包扔进 `C:\Users\Siegrain\AppData\Local\nvm` 对应版本的目录中也能用
4. 安装 7.2.3 的 [Angular CLI](https://github.com/angular/angular-cli)
5. 在 ClientApp 里 `yarn` 一下，然后 `yarn start`，没有报错就停掉，如果报`node-sass`就把`node_modules`干掉再`yarn`
6. 回 `Ancorazor.API` 目录跑 `dotnet watch run`

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

我会在之后写一篇教程如何在`Azure DevOps`上进行 CI/CD，现在你也可以参考 [azure-pipelines.yml](https://github.com/siegrainwong/ancorazor/blob/master/azure-pipelines.yml)。

## 项目结构

TODO

## To-do

- [x] Comment
- [ ] Management page
- [ ] Search
- [ ] Categories & tags page
- [ ] Tests

或参考 [project](https://github.com/siegrainwong/ancorazor/projects/1).

## 致谢

[模板: startbootstrap-clean-blog](https://github.com/BlackrockDigital/startbootstrap-clean-blog)

##

## Licence

Anti-996 & MIT

[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fsiegrainwong%2Fancorazor.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fsiegrainwong%2Fancorazor?ref=badge_large)
