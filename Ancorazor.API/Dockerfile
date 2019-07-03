FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Ancorazor.API/Ancorazor.API.csproj", "Ancorazor.API/"]
COPY ["Siegrain.Common/Siegrain.Common.csproj", "Siegrain.Common/"]
COPY ["Ancorazor.Messages/Ancorazor.API.Messages.csproj", "Ancorazor.Messages/"]
COPY ["Ancorazor.Entity/Ancorazor.Entity.csproj", "Ancorazor.Entity/"]
COPY ["Ancorazor.API.Common/Ancorazor.API.Common.csproj", "Ancorazor.API.Common/"]
COPY ["Ancorazor.Service/Ancorazor.Service.csproj", "Ancorazor.Service/"]
RUN dotnet restore "Ancorazor.API/Ancorazor.API.csproj"
COPY . .
WORKDIR "/src/Ancorazor.API"
RUN dotnet build "Ancorazor.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Ancorazor.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ancorazor.API.dll"]