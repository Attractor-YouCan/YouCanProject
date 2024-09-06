# Используйте официальный .NET SDK образ для сборки и выполнения миграций
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы решения и проектов
COPY *.sln ./
COPY YouCan.Mvc/YouCan.Mvc.csproj YouCan.Mvc/
COPY YouCan.Entities/YouCan.Entities.csproj YouCan.Entities/
COPY YouCan.Repository/YouCan.Repository.csproj YouCan.Repository/
COPY YouCan.Service/YouCan.Service.csproj YouCan.Service/
COPY YouCan.Test/YouCan.Test.csproj YouCan.Test/

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем остальные файлы проекта
COPY . .

# Строим и публикуем проект
RUN dotnet build -c Release
RUN dotnet publish YouCan.Mvc/YouCan.Mvc.csproj -c Release -o /app/publish

# Этап применения миграций
FROM build AS migrations
WORKDIR /app
COPY --from=build /app/publish .
COPY YouCan.Mvc/YouCan.Mvc.csproj ./

# Применение миграций
RUN dotnet ef database update --startup-project /app/publish/YouCan.Mvc.dll

# Этап тестирования
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS test
WORKDIR /src

# Копируем тестовые проекты и зависимости
COPY *.sln ./
COPY YouCan.Test/YouCan.Test.csproj YouCan.Test/
COPY YouCan.Mvc/YouCan.Mvc.csproj YouCan.Mvc/
COPY YouCan.Entities/YouCan.Entities.csproj YouCan.Entities/
COPY YouCan.Repository/YouCan.Repository.csproj YouCan.Repository/
COPY YouCan.Service/YouCan.Service.csproj YouCan.Service/

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем остальные файлы
COPY . .

# Выполнение тестов
RUN dotnet test YouCan.Test/YouCan.Test.csproj

# Финальный образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем опубликованные файлы из этапа сборки
COPY --from=build /app/publish .

# Запуск приложения
CMD ["dotnet", "YouCan.Mvc.dll"]
