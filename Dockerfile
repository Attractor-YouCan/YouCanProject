# Используйте официальный .NET SDK образ для сборки, тестирования и выполнения миграций
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта и выполняем восстановление зависимостей
COPY ["YouCan.Mvc/YouCan.Mvc.csproj", "YouCan.Mvc/"]
COPY ["YouCan.Entities/YouCan.Entities.csproj", "YouCan.Entities/"]
COPY ["YouCan.Repository/YouCan.Repository.csproj", "YouCan.Repository/"]
COPY ["YouCan.Tests/YouCan.Tests.csproj", "YouCan.Tests/"]
COPY ["YouCan.sln", "./"]

# Восстановление зависимостей
RUN dotnet restore "YouCan.Mvc/YouCan.Mvc.csproj"

# Копируем все файлы проекта
COPY . .

# Запуск юнит-тестов
RUN dotnet test "YouCan.Tests/YouCan.Tests.csproj" --no-restore

# Выполняем публикацию
RUN dotnet publish "YouCan.Mvc/YouCan.Mvc.csproj" -c Release -o /app/publish

# Применение миграций
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrations
WORKDIR /app
COPY --from=build /app/publish .
COPY ["YouCan.Repository/YouCan.Repository.csproj", "YouCan.Repository/"]
COPY ["YouCan.Entities/YouCan.Entities.csproj", "YouCan.Entities/"]
COPY ["YouCan.sln", "./"]

# Применение миграций
RUN dotnet ef database update --project YouCan.Repository/YouCan.Repository.csproj --startup-project /app/YouCan.Mvc.dll

# Создание финального образа на основе Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем опубликованные файлы из этапа сборки
COPY --from=build /app/publish .

# Запуск приложения
CMD ["dotnet", "YouCan.Mvc.dll"]
