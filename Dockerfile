# Docker file of Blog.API

# Set siegrainwong/aspnetcore-build:2.2 as build env, it includes .net core 2.2 sdk and nodejs
FROM siegrainwong/aspnetcore-build:2.2 AS publish
WORKDIR /src
COPY . .
WORKDIR /src/Blog.API
RUN dotnet publish "Blog.API.csproj" -c Release -o /app

# Install required packages
RUN apk add --no-cache \
    bash

# Remove source code
RUN rm -rf "/src";

WORKDIR /app

EXPOSE 8088

CMD ["dotnet", "Blog.API.dll"]