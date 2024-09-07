# Используйте официальный .NET SDK образ для сборки, тестирования и выполнения миграций
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Установите инструмент dotnet-ef
RUN dotnet tool install --global dotnet-ef

# Обновите переменную PATH, чтобы dotnet-ef был доступен в контейнере
ENV PATH="${PATH}:/root/.dotnet/tools"

# Копируем все файлы решения в контейнер
COPY ["YouCan.sln", "./"]
COPY ["YouCan.Mvc/YouCan.Mvc.csproj", "YouCan.Mvc/"]
COPY ["YouCan.Entities/YouCan.Entities.csproj", "YouCan.Entities/"]
COPY ["YouCan.Repository/YouCan.Repository.csproj", "YouCan.Repository/"]
COPY ["YouCan.Tests/YouCan.Tests.csproj", "YouCan.Tests/"]

# Восстанавливаем зависимости для всех проектов
RUN dotnet restore

# Копируем все исходные файлы для тестирования и сборки
COPY . .

# Запуск unit-тестов
RUN dotnet test YouCan.Tests/YouCan.Tests.csproj --no-restore

# Выполняем публикацию приложения
RUN dotnet publish YouCan.Mvc/YouCan.Mvc.csproj -c Release -o /app/publish

# Применение миграций
FROM build AS migrations
WORKDIR /app
COPY --from=build /app/publish .
COPY ["YouCan.Mvc/YouCan.Mvc.csproj", "./"]

# Применение миграций
RUN dotnet ef database update --startup-project /app/publish/YouCan.Mvc.dll

# Создание финального образа на основе Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем опубликованные файлы из этапа сборки
COPY --from=build /app/publish .

# Запуск приложения
CMD ["dotnet", "YouCan.Mvc.dll"]
