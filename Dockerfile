# Используйте официальный .NET SDK образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копирование файлов проекта и восстановление зависимостей
COPY ["YouCan.Mvc.csproj", "./"]
RUN dotnet restore "./YouCan.Mvc.csproj"

# Копирование всего исходного кода и сборка приложения
COPY . .
RUN dotnet publish "YouCan.Mvc.csproj" -c Release -o /app/publish

# Используйте официальный .NET Runtime образ для финального образа
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Копирование опубликованных файлов из образа сборки
COPY --from=build /app/publish .

# Добавление скрипта для вывода переменных окружения в логи
COPY print-env-vars.sh /app/print-env-vars.sh
RUN chmod +x /app/print-env-vars.sh
ENTRYPOINT ["/app/print-env-vars.sh"]

# Запуск приложения
CMD ["dotnet", "YouCan.Mvc.dll"]
