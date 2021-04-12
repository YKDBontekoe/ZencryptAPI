#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish YKDResumeAPI -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
COPY --from=build-env /app/YKDResumeAPI/out .

ENTRYPOINT ["dotnet", "YKDResumeAPI.dll"]