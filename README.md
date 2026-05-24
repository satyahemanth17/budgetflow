# BudgetFlow

![CI](https://github.com/satyahemanth17/budgetflow/actions/workflows/ci.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-blue)

Cloud-native expense tracking REST + GraphQL API built with C# .NET 8, demonstrating Clean Architecture, Entity Framework Core, Redis, Azure Functions, HotChocolate GraphQL, and Kubernetes.

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                        BudgetFlow.API                        │
│         ASP.NET Core 8 · HotChocolate GraphQL · JWT          │
│  /api/expenses  /api/budgets  /api/auth  /graphql  /swagger  │
└──────────────────────────┬──────────────────────────────────┘
                           │ depends on
┌──────────────────────────▼──────────────────────────────────┐
│                    BudgetFlow.Application                     │
│      ExpenseService · BudgetService · AutoMapper · DTOs       │
│        FluentValidation · IExpenseRepository · IUnitOfWork    │
└──────────────┬────────────────────────────┬─────────────────┘
               │ implements                  │ depends on
┌──────────────▼──────────┐   ┌─────────────▼───────────────┐
│  BudgetFlow.Infrastructure  │   │    BudgetFlow.Domain       │
│  EF Core · SQL Server    │   │  Entities: User, Budget,      │
│  Redis · JWT · Repos     │   │  Expense, BudgetAlert         │
│  UnitOfWork · Migrations │   │  Enums: ExpenseCategory,      │
│  ServiceBusPublisher     │   │  AlertStatus                  │
└─────────────────────────┘   └─────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                    BudgetFlow.Functions                       │
│     Azure Functions v4 Isolated Worker                        │
│     BudgetAlertFunction (timer) · MonthlySummaryFunction      │
└─────────────────────────────────────────────────────────────┘
```

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Language | C# (.NET 8) |
| API | ASP.NET Core 8 Web API |
| GraphQL | HotChocolate 13 |
| ORM | Entity Framework Core 8 |
| Database | SQL Server 2022 |
| Cache | Redis 7 (StackExchange.Redis) |
| Auth | JWT Bearer |
| Functions | Azure Functions v4 |
| Validation | FluentValidation |
| Mapping | AutoMapper |
| Testing | xUnit + Moq + FluentAssertions |
| Container | Docker + Docker Compose |
| Orchestration | Kubernetes |
| CI | GitHub Actions |

## Getting Started

### Prerequisites
- .NET 8 SDK
- Docker Desktop
- Azure Functions Core Tools v4

### Run with Docker Compose
```bash
docker-compose up --build
```
API available at `http://localhost:5000/swagger`

### Run locally
```bash
# Start dependencies
docker run -e ACCEPT_EULA=Y -e SA_PASSWORD=BudgetFlow123! -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
docker run -p 6379:6379 -d redis:7-alpine

# Apply migrations
dotnet ef database update --project src/BudgetFlow.Infrastructure --startup-project src/BudgetFlow.API

# Start API
cd src/BudgetFlow.API && dotnet run
```

### Run tests
```bash
dotnet test
```

### Run Azure Functions locally
```bash
cd src/BudgetFlow.Functions && func start
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/auth/token | Get JWT token |
| GET | /api/expenses/{id} | Get expense by ID |
| GET | /api/expenses/user/{userId} | List user expenses |
| POST | /api/expenses | Create expense |
| DELETE | /api/expenses/{id} | Soft-delete expense |
| GET | /api/budgets/{id}/summary | Get budget summary |
| PUT | /api/budgets/{id}/limit | Update budget limit |
| GET | /graphql | GraphQL endpoint (Banana Cake Pop UI) |
| GET | /swagger | Swagger UI |

## Demo Credentials

The `/api/auth/token` stub accepts:
```json
{ "email": "demo@budgetflow.com", "password": "demo123" }
```
Use the returned JWT as `Authorization: Bearer <token>` on all other endpoints.

## ExpenseCategory Values

`Food` · `Transport` · `Housing` · `Healthcare` · `Entertainment` · `Shopping` · `Utilities` · `Other`

## Business Rules

- All entities use soft delete (`IsDeleted = true`, never hard delete)
- Budget alert (`AlertStatus.Pending`) created when spending reaches **80%** of monthly limit
- Expense creation blocked when it would **exceed** the monthly limit
- `SpendingPercentage = Math.Round(CurrentSpending / MonthlyLimit * 100, 2)`
