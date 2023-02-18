FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["backend.csproj", "."]
RUN dotnet restore "backend.csproj"

COPY . .
RUN dotnet build "backend.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "backend.csproj" --no-restore -c Release -o /app/publish

FROM base AS final

RUN apk --no-cache add curl

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "backend.dll"]
