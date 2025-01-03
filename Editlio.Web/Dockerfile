# ✅ Daha geniş özellikli bir temel imaj kullan (Ubuntu 22.04 "Jammy")
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 80

# ✅ Gerekli ağ araçlarını yükle
RUN apt update && apt install -y curl wget iputils-ping net-tools

# ✅ Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src

# 🔹 Bağımlılıkları kopyala ve restore et
COPY ["Editlio.Web/Editlio.Web.csproj", "Editlio.Web/"]
COPY ["Editlio.Shared/Editlio.Shared.csproj", "Editlio.Shared/"]
RUN dotnet restore "Editlio.Web/Editlio.Web.csproj"

# 🔹 Kaynak dosyaları kopyala ve build et
COPY . .
WORKDIR "/src/Editlio.Web"
RUN dotnet publish -c Release -o /app/publish

# ✅ Final çalışma aşaması
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# 🔹 Docker ortamında tüm IP'lerden erişilebilecek şekilde ayarla
ENV ASPNETCORE_URLS=http://0.0.0.0:80

# ✅ Konteyner çalıştığında uygulamanın başlatılması
ENTRYPOINT ["dotnet", "Editlio.Web.dll"]
