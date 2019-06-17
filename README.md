# Ancorazor

[![Build Status](https://dev.azure.com/siegrainwong/Ancorazor/_apis/build/status/ancorazor?branchName=master)](https://dev.azure.com/siegrainwong/Ancorazor/_build/latest?definitionId=6&branchName=master)

Ancorazor is a blog system built by dotnet core 2.2 and angular 7 (ssr supported) with a very smooth transition effects.

[Live demo](https://siegrain.wang).

*NOTICE: It's still in progress and has no management page, so... try it if you brave. :)*

## Getting start

### Requirements
Make sure you have these already installed on your machine.
1. .NET Core 2.2 SDK
2. Nodejs 10+
3. SQL Server(optional with docker-compose)

### Build
1. `git clone https://github.com/Seanwong933/ancorazor.git`
2. Replace connection string in `ancorazor/Blog.API/appsettings.Development.json`(optional depends on your condition)
3. `cd path-to-ancorazor/Blog.API` then `dotnet watch run`
4. Open `localhost:8088`, the default username/password is admin/123456.


*NOTICE: It's better to use this way while developing cause I haven't figure out how to debug .net core spa in docker yet.*

### Build with docker-compose
`cd path-to-ancorazor/build`
#### windows
Replace all `F:\Projects\ancorazor\` in `dev.ps1` to yours path then run this script.
#### linux
Run `path-to-ancorazor/build/dev.sh`


This would launch sql server, skywalking, nginx and ancorazor in your docker.
- Skywalking: `localhost:8080`, the default account/password is admin/admin.
- Ancorazor: `localhost:8088`, the default username/password is admin/123456.

## Release(CI/CD)
I recommend you use azure devops to release this to production, all my pipelines are public [here](https://dev.azure.com/siegrainwong/Ancorazor/_build?definitionId=6).

## Structure
TODO

## To-do
- [x] Comment
- [ ] Management page
- [ ] Search
- [ ] Categories & tags page

Or you can see more details in [projects](https://github.com/Seanwong933/ancorazor/projects/1).

## Acknowledgements
[Template provider: startbootstrap-clean-blog](https://github.com/BlackrockDigital/startbootstrap-clean-blog)

## Licence
MIT
