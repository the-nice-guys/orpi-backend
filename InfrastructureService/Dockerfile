FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 44392

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.sln .
COPY InfrastructureService/*.csproj ./InfrastructureService/
COPY OrpiLibrary/*.csproj ./OrpiLibrary/
RUN dotnet restore ./InfrastructureService/InfrastructureService.csproj

# copy everything else and build app
COPY InfrastructureService/. ./InfrastructureService/
COPY OrpiLibrary/. ./OrpiLibrary/
WORKDIR /src/InfrastructureService
RUN dotnet publish -c Release -o /app/build-dll

FROM build AS publish
RUN dotnet publish -c Release -o /app/build-dll

FROM base AS final
WORKDIR /app
COPY --from=publish /app/build-dll .
ENTRYPOINT ["dotnet", "InfrastructureService.dll"]
