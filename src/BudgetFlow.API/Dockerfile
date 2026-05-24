FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/BudgetFlow.API/BudgetFlow.API.csproj", "src/BudgetFlow.API/"]
COPY ["src/BudgetFlow.Application/BudgetFlow.Application.csproj", "src/BudgetFlow.Application/"]
COPY ["src/BudgetFlow.Domain/BudgetFlow.Domain.csproj", "src/BudgetFlow.Domain/"]
COPY ["src/BudgetFlow.Infrastructure/BudgetFlow.Infrastructure.csproj", "src/BudgetFlow.Infrastructure/"]
RUN dotnet restore "src/BudgetFlow.API/BudgetFlow.API.csproj"
COPY . .
WORKDIR "/src/src/BudgetFlow.API"
RUN dotnet build "BudgetFlow.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetFlow.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetFlow.API.dll"]
