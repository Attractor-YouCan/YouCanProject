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
ENV PATH="${PATH}:/root/.dotnet/tools"

# Копируем файлы проекта и выполняем восстановление зависимостей
COPY ["YouCan.Mvc.csproj", "./"]
RUN dotnet restore "./YouCan.Mvc.csproj"

# Копируем остальные файлы проекта и выполняем публикацию
COPY . .
RUN dotnet publish "YouCan.Mvc.csproj" -c Release -o /app/publish

# Применение миграций
FROM build AS migrations
WORKDIR /app
COPY --from=build /app/publish .
COPY ["YouCan.Mvc.csproj", "./"]

# Применение миграций
RUN dotnet ef database update --startup-project /app/publish/YouCan.Mvc.dll

# Создание финального образа на основе Runtime
FROM base AS final
WORKDIR /app

# Копируем опубликованные файлы из этапа сборки
COPY --from=build /app/publish .

# Запуск приложения
CMD ["dotnet", "YouCan.Mvc.dll"]
