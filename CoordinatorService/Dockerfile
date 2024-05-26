FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /service
EXPOSE 5287
ENV ASPNETCORE_URLS=http://+:5287

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CoordinatorService/CoordinatorService.csproj", "CoordinatorService/"]
RUN dotnet restore "CoordinatorService/CoordinatorService.csproj"
COPY . ./
WORKDIR CoordinatorService
RUN dotnet build "CoordinatorService.csproj" -c Release -o /service/build

FROM build AS publish
RUN dotnet publish "CoordinatorService.csproj" -c Release -o /service/publish

FROM base AS service
WORKDIR /service
COPY --from=publish /service/publish .
ENTRYPOINT ["dotnet", "CoordinatorService.dll"]
