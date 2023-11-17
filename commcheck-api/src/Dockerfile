FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5004

ENV ASPNETCORE_URLS=http://+:5004

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["commcheck-api.csproj", "./"]
RUN dotnet restore "commcheck-api.csproj"

#COPY . .

COPY ./src /src
COPY ["rules.json", "./"]
COPY ["appsettings.json", "./"]
COPY ["commcheck-api.csproj", "./"]

WORKDIR "/src/."
RUN dotnet build "commcheck-api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "commcheck-api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final

USER $APP_UID

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "commcheck-api.dll"]
