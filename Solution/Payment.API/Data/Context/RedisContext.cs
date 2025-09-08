namespace Payment.API.Data.Context;

public class RedisContext(IConnectionMultiplexer connectionMultiplexer) : IRedisContext
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<bool> SaveAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        return await _database.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _database.StringGetAsync(key);
        if (!json.HasValue)
            return default;

        return JsonSerializer.Deserialize<T>(json!, JsonOptions);
    }

    public async Task<bool> DeleteAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public static IConnectionMultiplexer CreateConnection(IOptions<RedisConfiguration> options)
    {
        var connectionString = options.Value.ConnectionString;
        return ConnectionMultiplexer.Connect(connectionString);
    }
}
