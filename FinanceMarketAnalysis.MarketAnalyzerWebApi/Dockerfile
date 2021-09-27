#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FinanceMarketAnalysis.MarketAnalyzerWebApi/FinanceMarketAnalysis.MarketAnalyzerWebApi.csproj", "FinanceMarketAnalysis.MarketAnalyzerWebApi/"]
RUN dotnet restore "FinanceMarketAnalysis.MarketAnalyzerWebApi/FinanceMarketAnalysis.MarketAnalyzerWebApi.csproj"
COPY . .
WORKDIR "/src/FinanceMarketAnalysis.MarketAnalyzerWebApi"
RUN dotnet build "FinanceMarketAnalysis.MarketAnalyzerWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinanceMarketAnalysis.MarketAnalyzerWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceMarketAnalysis.MarketAnalyzerWebApi.dll"]
