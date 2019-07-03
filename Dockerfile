# Dockerfile of Ancorazor.API

# Set siegrainwong/aspnetcore-build:2.2-bionic as base image, includes .net core 2.2 sdk and nodejs
FROM siegrainwong/aspnetcore-build:2.2-bionic as publish
WORKDIR /src
COPY . .
RUN dotnet restore "Ancorazor.API/Ancorazor.API.csproj"
COPY . .
WORKDIR /src/Ancorazor.API
RUN npm rebuild node-sass
RUN dotnet publish "Ancorazor.API.csproj" -c Release -o /app

# This one includes 2.2 runtime and nodejs
FROM siegrainwong/aspnetcore:2.2-bionic
EXPOSE 8088
WORKDIR /app
COPY --from=publish /app .
CMD ["dotnet", "Ancorazor.API.dll"]