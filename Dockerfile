#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY *.sln ./
# COPY ClassLibraryProject/*.csproj ./ClassLibraryProject/
COPY YKDResumeAPI/*.csproj ./YKDResumeAPI/

RUN dotnet restore
COPY . .
# WORKDIR /src/ClassLibraryProject
# RUN dotnet build -c Release -o /app

WORKDIR /src/YKDResumeAPI
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YKDResumeAPI.dll"]