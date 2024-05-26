FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /service
EXPOSE 5292
ENV ASPNETCORE_URLS=http://+:5292

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MonitoringModule/MonitoringModule.csproj", "MonitoringModule/"]
COPY ["OrpiLibrary/OrpiLibrary.csproj", "OrpiLibrary/"]
RUN dotnet restore "MonitoringModule/MonitoringModule.csproj"
COPY . .
WORKDIR "/src/MonitoringModule"
RUN dotnet build "MonitoringModule.csproj" -c Release -o /service/build

FROM build AS publish
RUN dotnet publish "MonitoringModule.csproj" -c Release -o /service/publish

FROM base AS service
WORKDIR /service
COPY --from=publish /service/publish .
ENTRYPOINT ["dotnet", "MonitoringModule.dll"]
