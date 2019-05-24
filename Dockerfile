# Docker file of Blog.API

# 1. 将 siegrainwong/aspnetcore-build 作为运行环境（包含2.2的sdk和nodejs），运行目录为app目录，并暴露端口8088
FROM siegrainwong/aspnetcore-build:2.2 AS base
WORKDIR /app
EXPOSE 8088

# 2. 将 siegrainwong/aspnetcore-build 作为编译环境，编译目录为src
# 接着就是dotnet restore命令，然后进入目录，执行publish命令，并将编译结果输出到/app目录。
FROM siegrainwong/aspnetcore-build:2.2 AS publish
WORKDIR /src
COPY . .
WORKDIR /src/Blog.API
RUN dotnet publish "Blog.API.csproj" -c Release -o /app
WORKDIR /app
CMD ["dotnet", "Blog.API.dll"]