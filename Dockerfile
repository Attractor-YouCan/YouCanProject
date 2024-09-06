# Базовый образ для выполнения приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Образ для сборки и миграций
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проектов и восстанавливаем зависимости
COPY YouCan.Mvc/YouCan.Mvc.csproj YouCan.Mvc/
COPY YouCan.Service/YouCan.Service.csproj YouCan.Service/
COPY YouCan.Repository/YouCan.Repository.csproj YouCan.Repository/
COPY YouCan.Test/YouCan.Test.csproj YouCan.Test/

# Восстановление зависимостей для всех проектов
RUN dotnet restore YouCan.Mvc/YouCan.Mvc.csproj

# Копируем все файлы проектов
COPY . .
RUN dotnet publish YouCan.Mvc/YouCan.Mvc.csproj -c Release -o /app/publish

# Образ для выполнения миграций
FROM build AS migrations
WORKDIR /app
COPY --from=build /app/publish .

# Применение миграций
RUN dotnet ef database update --startup-project YouCan.Mvc/YouCan.Mvc.csproj

# Образ для выполнения тестов
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS test
WORKDIR /src

# Копируем проекты для тестирования и зависимости
COPY YouCan.Test/YouCan.Test.csproj YouCan.Test/
RUN dotnet restore YouCan.Test/YouCan.Test.csproj
COPY . .

# Выполнение юнит-тестов
RUN dotnet test YouCan.Test/YouCan.Test.csproj --no-restore --verbosity normal

# Финальный образ на основе Runtime
FROM base AS final
WORKDIR /app

# Копируем опубликованные файлы из этапа сборки
COPY --from=build /app/publish .

# Запуск приложения
CMD ["dotnet", "YouCan.Mvc.dll"]
