FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY **/*.csproj ./
RUN dotnet restore ZenCryptAPI/ZenCryptAPI.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ZenCryptAPI/ZenCryptAPI.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "ZenCryptAPI/ZenCryptAPI.dll"]