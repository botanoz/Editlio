# ✅ Daha geniş özellikli temel imaj kullan (Ubuntu 22.04 "Jammy")
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 7273

# ✅ Gerekli ağ araçlarını yükle
RUN apt update && apt install -y curl wget iputils-ping net-tools

# ✅ Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src

# 🔹 Bağımlılıkları kopyala ve restore et
COPY ["Editlio.Api/Editlio.Api.csproj", "Editlio.Api/"]
COPY ["Editlio.Infrastructure/Editlio.Infrastructure.csproj", "Editlio.Infrastructure/"]
COPY ["Editlio.Domain/Editlio.Domain.csproj", "Editlio.Domain/"]
COPY ["Editlio.Shared/Editlio.Shared.csproj", "Editlio.Shared/"]
RUN dotnet restore "Editlio.Api/Editlio.Api.csproj"

# 🔹 Kaynak dosyaları kopyala ve build et
COPY . .
WORKDIR "/src/Editlio.Api"
RUN dotnet publish -c Release -o /app/publish

# ✅ Final çalışma aşaması
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# 🔹 Docker ortamında tüm IP'lerden erişilebilecek şekilde ayarla
ENV ASPNETCORE_URLS=http://0.0.0.0:7273

# ✅ Konteyner çalıştığında API uygulamasının başlatılması
ENTRYPOINT ["dotnet", "Editlio.Api.dll"]
