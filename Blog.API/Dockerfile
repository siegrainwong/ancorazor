FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Blog.API/Blog.API.csproj", "Blog.API/"]
COPY ["Siegrain.Common/Siegrain.Common.csproj", "Siegrain.Common/"]
COPY ["Blog.Service/Blog.Service.csproj", "Blog.Service/"]
COPY ["Blog.Messages/Blog.API.Messages.csproj", "Blog.Messages/"]
COPY ["Blog.Entity/Blog.Entity.csproj", "Blog.Entity/"]
COPY ["Blog.API.Common/Blog.API.Common.csproj", "Blog.API.Common/"]
RUN dotnet restore "Blog.API/Blog.API.csproj"
COPY . .
WORKDIR "/src/Blog.API"
RUN dotnet build "Blog.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Blog.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Blog.API.dll"]