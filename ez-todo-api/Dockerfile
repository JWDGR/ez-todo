FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 4000
EXPOSE 7001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ez-todo-api/ez-todo-api.csproj", "ez-todo-api/"]
COPY ["ez-todo-shared/ez-todo-shared.csproj", "ez-todo-shared/"]
COPY ["ez-todo-services/ez-todo-services.csproj", "ez-todo-services/"]
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
ENV ASPNETCORE_URLS=http://+:4000
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV JWT_SECRET=your-super-secret-jwt-key-that-is-at-least-32-characters-long-for-production-use
ENV JWT_ISSUER=ez-todo-auth
ENV JWT_AUDIENCE=ez-todo-auth
ENV TodoService__BaseUrl=http://ez-todo-services:5020
ENV AuthService__BaseUrl=http://ez-todo-auth:5010
ENTRYPOINT ["dotnet", "ez-todo-api.dll"]
