using BudgetFlow.Application.Interfaces;
using BudgetFlow.Infrastructure.Auth;
using BudgetFlow.Infrastructure.Cache;
using BudgetFlow.Infrastructure.Data;
using BudgetFlow.Infrastructure.Data.Repositories;
using BudgetFlow.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BudgetFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();

        var redisConn = config.GetConnectionString("Redis") ?? "localhost:6379";
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisConn));
        services.AddSingleton<RedisCacheService>();

        services.AddSingleton<ServiceBusPublisher>();
        services.AddScoped<JwtService>();

        return services;
    }
}
