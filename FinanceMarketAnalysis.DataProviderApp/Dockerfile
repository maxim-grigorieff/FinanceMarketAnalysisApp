#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FinanceMarketAnalysis.DataProviderApp/FinanceMarketAnalysis.DataProviderApp.csproj", "FinanceMarketAnalysis.DataProviderApp/"]
COPY ["FinanceMarketAnalysis.Common/FinanceMarketAnalysis.Common.csproj", "FinanceMarketAnalysis.Common/"]
COPY ["FinanceMarketAnalysis.DataProvider/FinanceMarketAnalysis.DataProvider.csproj", "FinanceMarketAnalysis.DataProvider/"]
COPY ["FinanceMarketAnalysis.DataModel/FinanceMarketAnalysis.DataModel.csproj", "FinanceMarketAnalysis.DataModel/"]
COPY ["FinanceMarketAnalysis.Store/FinanceMarketAnalysis.Store.csproj", "FinanceMarketAnalysis.Store/"]
RUN dotnet restore "FinanceMarketAnalysis.DataProviderApp/FinanceMarketAnalysis.DataProviderApp.csproj"
COPY . .
WORKDIR "/src/FinanceMarketAnalysis.DataProviderApp"
RUN dotnet build "FinanceMarketAnalysis.DataProviderApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinanceMarketAnalysis.DataProviderApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceMarketAnalysis.DataProviderApp.dll"]
