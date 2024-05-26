FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /service
EXPOSE 5074
ENV ASPNETCORE_URLS=http://+:5074

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DockerModule/DockerModule.csproj", "DockerModule/"]
RUN dotnet restore "DockerModule/DockerModule.csproj"
COPY . ./
WORKDIR DockerModule
RUN dotnet build "DockerModule.csproj" -c Release -o /service/build

FROM build AS publish
RUN dotnet publish "DockerModule.csproj" -c Release -o /service/publish

FROM base AS service
WORKDIR /service
COPY --from=publish /service/publish .
ENTRYPOINT ["dotnet", "DockerModule.dll"]
