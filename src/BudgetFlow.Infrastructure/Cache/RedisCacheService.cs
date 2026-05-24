using StackExchange.Redis;
using System.Text.Json;

namespace BudgetFlow.Infrastructure.Cache;

public class RedisCacheService
{
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer redis) =>
        _db = redis.GetDatabase();

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) =>
        await _db.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        return value.IsNull ? default : JsonSerializer.Deserialize<T>(value!);
    }

    public async Task RemoveAsync(string key) =>
        await _db.KeyDeleteAsync(key);
}
