# Используем официальный образ .NET SDK для сборки и тестов
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем файлы решения и проекты
COPY ["YouCan.sln", "./"]
COPY ["YouCan.Mvc.csproj", "./"]
COPY ["YouCan.Entites/YouCan.Entites.csproj", "YouCan.Entites/"]
COPY ["YouCan.Repository/YouCan.Repository.csproj", "YouCan.Repository/"]
COPY ["YouCan.Service/YouCan.Service.csproj", "YouCan.Service/"]
COPY ["YouCan.Tests/YouCan.Tests.csproj", "YouCan.Tests/"]

# Восстановление зависимостей
RUN dotnet restore "YouCan.sln"

# Копируем все файлы
COPY . .

# Сборка всех проектов
RUN dotnet build "YouCan.sln" -c Release -o /app/build

# Публикация приложения
RUN dotnet publish "YouCan.Mvc.csproj" -c Release -o /app/publish

# Этап выполнения: используем официальный образ ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Запуск приложения
ENTRYPOINT ["dotnet", "YouCan.Mvc.dll"]
