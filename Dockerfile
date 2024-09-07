# Используем официальный .NET SDK образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы решения и проекты
COPY ["YouCan.sln", "./"]
COPY ["YouCan.Mvc.csproj", "./"]
COPY ["YouCan.Entites/YouCan.Entites.csproj", "YouCan.Entites/"]
COPY ["YouCan.Repository/YouCan.Repository.csproj", "YouCan.Repository/"]
COPY ["YouCan.Service/YouCan.Service.csproj", "YouCan.Service/"]
COPY ["YouCan.Tests/YouCan.Tests.csproj", "YouCan.Tests/"]

# Восстановление зависимостей
RUN dotnet restore "YouCan.sln"

# Копируем остальные файлы проекта
COPY . .

# Сборка проекта
RUN dotnet build "YouCan.Mvc.csproj" -c Release -o /app/build

# Запуск юнит-тестов
RUN dotnet test "YouCan.Tests/YouCan.Tests.csproj" --no-restore

# Публикация приложения
RUN dotnet publish "YouCan.Mvc.csproj" -c Release -o /app/publish

# Создание финального образа с .NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем опубликованные файлы из этапа сборки
COPY --from=build /app/publish .

# Запуск приложения
CMD ["dotnet", "YouCan.Mvc.dll"]
