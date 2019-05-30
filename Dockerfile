# Docker file of Blog.API

# Set siegrainwong/aspnetcore-build:2.2 as build env, it includes .net core 2.2 sdk and nodejs
#FROM siegrainwong/aspnetcore-build:2.2-alpine
FROM siegrainwong/aspnetcore-build:2.2-bionic
WORKDIR /src
COPY . .
WORKDIR /src/Blog.API

RUN npm rebuild node-sass
RUN dotnet publish "Blog.API.csproj" -c Release -o /app

WORKDIR /app

EXPOSE 8088

CMD ["dotnet", "Blog.API.dll"]