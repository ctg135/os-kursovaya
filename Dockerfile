# Использования образа dotnet как платформы для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Создание рабочего каталога с приложением
WORKDIR /app
# Открытие TCP-порта для веб-приложения
EXPOSE 5038
# Использование TCP-порта для прослушивания приложением
ENV ASPNETCORE_URLS=http://+:5038
# Конфигурация пользователя приложения
USER app
# Сборка исходного кода приложений
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["client/client.csproj", "client/"]
COPY srv1/ srv1/
COPY srv2/ srv2/
# Восстановление зависимостей проекта
RUN dotnet restore "client/client.csproj"
COPY . .
WORKDIR "/src/client"
RUN dotnet build "client.csproj" -c $configuration -o /app/build
# Публикация релизной версии приложения в образ
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "client.csproj" -c $configuration -o /app/publish /p:UseAppHost=false
# Команда для запуска образа
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "client.dll"]
