# Используем официальный образ .NET SDK 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Устанавливаем dotnet-ef tool
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# Копируем файлы решения и проекты
COPY ["YouCan.sln", "./"]
COPY ["YouCan.Mvc.csproj", "./"]
COPY ["YouCan.Entites/YouCan.Entites.csproj", "YouCan.Entites/"]
COPY ["YouCan.Repository/YouCan.Repository.csproj", "YouCan.Repository/"]
COPY ["YouCan.Service/YouCan.Service.csproj", "YouCan.Service/"]
COPY ["YouCan.Tests/YouCan.Tests.csproj", "YouCan.Tests/"]

# Копируем все файлы
COPY . .

# Выполняем миграции
WORKDIR /app/YouCan.Repository
RUN dotnet ef database update --project YouCan.Repository.csproj --startup-project ../YouCan.Mvc/YouCan.Mvc.csproj

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
