FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ez-todo-api/ez-todo-api.csproj", "ez-todo-api/"]
COPY ["ez-todo-shared/ez-todo-shared.csproj", "ez-todo-shared/"]
RUN dotnet restore "ez-todo-api/ez-todo-api.csproj"
COPY . .
WORKDIR "/src/ez-todo-api"
RUN dotnet build "./ez-todo-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ez-todo-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ez-todo-api.dll"]
