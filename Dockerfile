# Используйте официальный .NET SDK образ для сборки
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Используйте официальный .NET SDK образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["YouCan.Mvc.csproj", "./"]
RUN dotnet restore "./YouCan.Mvc.csproj"
COPY . .
RUN dotnet publish "YouCan.Mvc.csproj" -c Release -o /app/publish

# Создание финального образа
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "YouCan.Mvc.dll"]
