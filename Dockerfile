FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY . .

ARG API_V4_URL
ARG REPOSITORY_PROJECT_ID
ARG GITLAB_USER
ARG GITLAB_PASSWORD

RUN dotnet nuget add source "${API_V4_URL}/projects/${REPOSITORY_PROJECT_ID}/packages/nuget/index.json" \
    --name "Avt Media" \
    --username ${GITLAB_USER} \
    --password ${GITLAB_PASSWORD} \
    --store-password-in-clear-text 

WORKDIR "/src/src/App"

RUN dotnet build "App.csproj" -c Release -o /app/build

RUN dotnet publish "App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
EXPOSE 8080
WORKDIR /app
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "App.dll"]