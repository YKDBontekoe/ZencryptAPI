#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5001
ENV ASPNETCORE_URLS=https://*:5001

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ZenCryptAPI/ZenCryptAPI.csproj", "ZenCryptAPI/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Domain.Services/Domain.Services.csproj", "Domain.Services/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure.EF/Infrastructure.EF.csproj", "Infrastructure.EF/"]
RUN dotnet restore "ZenCryptAPI/ZenCryptAPI.csproj"
COPY . .
WORKDIR "/src/ZenCryptAPI"
RUN dotnet build "ZenCryptAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZenCryptAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "ZenCryptAPI.dll"] 