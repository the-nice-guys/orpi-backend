FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /service
EXPOSE 5056
ENV ASPNETCORE_URLS=http://+:5056

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HistoryService/HistoryService.csproj", "HistoryService/"]
RUN dotnet restore "HistoryService/HistoryService.csproj"
COPY . ./
WORKDIR HistoryService
RUN dotnet build "HistoryService.csproj" -c Release -o /service/build

FROM build AS publish
RUN dotnet publish "HistoryService.csproj" -c Release -o /service/publish

FROM base AS service
WORKDIR /service
COPY --from=publish /service/publish .
ENTRYPOINT ["dotnet", "HistoryService.dll"]
