#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY . ./
RUN dotnet restore YKDResumeAPI/YKDResumeAPI.csproj
RUN dotnet publish YKDResumeAPI/YKDResumeAPI.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
COPY --from=build-env /app/YKDResumeAPI/out .
ENTRYPOINT ["dotnet", "YKDResumeAPI.dll"]