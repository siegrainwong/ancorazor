# Ancorazor

[![Build status](https://dev.azure.com/siegrainwong/SGBlogCore/_apis/build/status/SGBlogCore-Azure%20Web%20App%20CI)](https://dev.azure.com/siegrainwong/SGBlogCore/_build/latest?definitionId=2)

Ancorazor is a blog system built by dotnet core 2.2 and angular 7(ssr supported for sure) with pretty smooth transition effects.

[Live demo](http://siegrain.wang).

*NOTICE: It's still in progress and has no admin dashboard, so... try it if you are brave :)*

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
4. Open `localhost:8088`

### Build with docker-compose
1. `cd path-to-ancorazor/build` then run `dev.ps1` on windows or `dev.sh` on linux.

*NOTICE: It's better to use regular way while develop cause I haven't figure out how to debug .net core spa in docker yet.*

This would launch SQL Server, Skywalking, nginx and ancorazor in your docker.
- Skywalking: `localhost:8080`, default account/password: admin admin
- Ancorazor: `localhost:8080`

## Release(CI/CD)
I recommend you use azure devops to release this to production, all my pipelines are public here.

## Structure
TODO

## To-do
- [ ] Comment
- [ ] Admin dashboard
