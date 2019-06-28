# Ancorazor

[![Codacy grade](https://img.shields.io/codacy/grade/00a15dd7811e42b7ae6aea01a966fee0.svg?logo=codacy&style=for-the-badge)](https://app.codacy.com/app/siegrainwong/ancorazor?utm_source=github.com&utm_medium=referral&utm_content=siegrainwong/ancorazor&utm_campaign=Badge_Grade_Dashboard)
[![Azure DevOps builds](https://img.shields.io/azure-devops/build/siegrainwong/75cdd93a-e41e-4158-ace3-88dab60c3343/6.svg?label=azure%20pipelines&logo=azure%20pipelines&style=for-the-badge)](https://dev.azure.com/siegrainwong/Ancorazor/_build/latest?definitionId=6&branchName=master)
[![LICENSE](https://img.shields.io/badge/license-Anti--996%20&%20MIT-blue.svg?style=for-the-badge)](https://github.com/996icu/996.ICU/blob/master/LICENSE)

[中文简介](https://github.com/siegrainwong/ancorazor/blob/master/README-CN.md)

---

Ancorazor is a blog system built with dotnet core 2.2 and angular 7 (ssr supported).

[Demo](https://siegrain.wang)

_It's still a work in progress and has no management page, so... try it if don't mind that :)_

## Gif Demostration

GIF 3M

![ancorazor gif demostration](https://s2.ax1x.com/2019/06/28/ZMxQs0.gif)

The transition module is based on `animate.css` and made by myself.

## Getting start

### Requirements

Make sure your machine has these dependancies installed.

1. .NET Core 2.2 SDK
2. Nodejs 10+
3. SQL Server(optional with docker-compose)

### Build

1. `git clone https://github.com/siegrainwong/ancorazor.git`
2. Replace connection string in `ancorazor/Ancorazor.API/appsettings.Development.json`(optional, depends on your use case)
3. `cd path-to-ancorazor/Ancorazor.API` then `dotnet watch run`
4. Open `localhost:8088`, the default username/password is admin/123456.

### Build with docker-compose

`cd path-to-ancorazor/build`

#### windows

Replace all `F:\Projects\ancorazor\` in `dev.ps1` with your project path then run this script.

#### linux

Run `path-to-ancorazor/build/dev.sh`

This will launch sql server, skywalking, nginx and ancorazor simultaneously in your docker.

- Skywalking: `localhost:8080`, the default account/password is admin/admin.
- Ancorazor: `localhost:8088`, the default username/password is admin/123456.

## Release(CI/CD)

Refer to [azure-pipelines.yml](https://github.com/siegrainwong/ancorazor/blob/master/azure-pipelines.yml).

## Structure

TODO

## To-do

- [x] Comment
- [ ] Management page
- [ ] Search
- [ ] Categories & tags page
- [ ] Tests

Or refer [project](https://github.com/Seanwong933/ancorazor/projects/1).

## Acknowledgements

[Template provider: startbootstrap-clean-blog](https://github.com/BlackrockDigital/startbootstrap-clean-blog)

## Licence

Anti-996 & MIT

[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fsiegrainwong%2Fancorazor.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fsiegrainwong%2Fancorazor?ref=badge_large)
