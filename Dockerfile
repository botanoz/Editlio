# Build aþamasý
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Tüm proje dosyalarýný kopyala ve baðýmlýlýklarý yükle
COPY ["Editlio.Web/*.csproj", "Editlio.Web/"]
COPY ["Editlio.Api/*.csproj", "Editlio.Api/"]
COPY ["Editlio.Domain/*.csproj", "Editlio.Domain/"]
COPY ["Editlio.Infrastructure/*.csproj", "Editlio.Infrastructure/"]
COPY ["Editlio.Shared/*.csproj", "Editlio.Shared/"]

RUN dotnet restore "Editlio.Web/Editlio.Web.csproj"
RUN dotnet restore "Editlio.Api/Editlio.Api.csproj"

# Tüm projeleri kopyala ve yayýnla (publish)
COPY . .
RUN dotnet publish "Editlio.Web/Editlio.Web.csproj" -c Release -o /app/web/publish
RUN dotnet publish "Editlio.Api/Editlio.Api.csproj" -c Release -o /app/api/publish

# Web Runtime aþamasý
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS web
WORKDIR /app
COPY --from=build /app/web/publish .
ENV ASPNETCORE_URLS http://+:5000
ENTRYPOINT ["dotnet", "Editlio.Web.dll"]

# API Runtime aþamasý
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api
WORKDIR /app
COPY --from=build /app/api/publish .
ENV ASPNETCORE_URLS http://+:5001
ENTRYPOINT ["dotnet", "Editlio.Api.dll"]
