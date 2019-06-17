# Dockerfile of Ancorazor.API

# Set siegrainwong/aspnetcore-build:2.2-bionic as base image, it includes .net core 2.2 sdk and nodejs
FROM siegrainwong/aspnetcore-build:2.2-bionic as publish
WORKDIR /src
COPY . .
WORKDIR /src/Ancorazor.API
RUN npm rebuild node-sass
RUN dotnet publish "Ancorazor.API.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim
EXPOSE 8088
WORKDIR /app
COPY --from=publish /app .
CMD ["dotnet", "Ancorazor.API.dll"]