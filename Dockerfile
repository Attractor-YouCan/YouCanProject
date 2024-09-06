# Используйте официальный .NET Runtime образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Используйте официальный .NET SDK образ для сборки и выполнения миграций
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Установите инструмент dotnet-ef
RUN dotnet tool install --global dotnet-ef

# Обновите переменную PATH, чтобы dotnet-ef был доступен в контейнере
ENV PATH="$PATH:/root/.dotnet/tools"

# Копируем файлы проекта и выполняем восстановление зависимостей
COPY ["YouCan.Mvc.csproj", "./"]
RUN dotnet restore "./YouCan.Mvc.csproj"

# Копируем остальные файлы проекта и выполняем публикацию
COPY . .
RUN dotnet publish "YouCan.Mvc.csproj" -c Release -o /app/publish

# Создание финального образа на основе Runtime
FROM base AS final
WORKDIR /app

# Копируем опубликованные файлы из этапа сборки
COPY --from=build /app/publish .

# Копируем скрипт запуска в контейнер
COPY scripts/entrypoint.sh /app/
RUN chmod +x /app/entrypoint.sh

# Установка скрипта запуска в качестве команды по умолчанию
ENTRYPOINT ["/app/entrypoint.sh"]

# Запуск приложения
CMD ["dotnet", "YouCan.Mvc.dll"]
