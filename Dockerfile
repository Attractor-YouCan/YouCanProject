# Используйте официальный .NET Runtime образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Используйте официальный .NET SDK образ для сборки и выполнения миграций
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта и выполняем восстановление зависимостей
COPY YouCan.Mvc/YouCan.Mvc.csproj YouCan.Mvc/
COPY YouCan.Service/YouCan.Service.csproj YouCan.Service/
COPY YouCan.Repository/YouCan.Repository.csproj YouCan.Repository/
COPY YouCan.Test/YouCan.Test.csproj YouCan.Test/

# Восстанавливаем зависимости
RUN dotnet restore YouCan.Mvc/YouCan.Mvc.csproj

# Копируем остальные файлы проекта и выполняем публикацию
COPY . .
RUN dotnet publish YouCan.Mvc/YouCan.Mvc.csproj -c Release -o /app/publish

# Применение миграций
FROM build AS migrations
WORKDIR /app
COPY --from=build /app/publish .
COPY YouCan.Mvc/YouCan.Mvc.csproj YouCan.Mvc/

# Применение миграций
RUN dotnet ef database update --startup-project /app/publish/YouCan.Mvc.dll

# Создание образа для тестов
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS test
WORKDIR /src

# Копируем тестовые проекты и зависимости
COPY YouCan.Test/YouCan.Test.csproj YouCan.Test/
RUN dotnet restore YouCan.Test/YouCan.Test.csproj
COPY . .

# Выполнение тестов
RUN dotnet test YouCan.Test/YouCan.Test.csproj

# Создание финального образа на основе Runtime
FROM base AS final
WORKDIR /app

# Копируем опубликованные файлы из этапа сборки
COPY --from=build /app/publish .

# Запуск приложения
CMD ["dotnet", "YouCan.Mvc.dll"]
