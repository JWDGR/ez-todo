FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5020
EXPOSE 7020

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ez-todo-services/ez-todo-services.csproj", "ez-todo-services/"]
RUN dotnet restore "ez-todo-services/ez-todo-services.csproj"
COPY . .
WORKDIR "/src/ez-todo-services"
RUN dotnet build "./ez-todo-services.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ez-todo-services.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ez-todo-services.dll"]
