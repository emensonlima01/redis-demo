namespace Payment.API.Interfaces;

public interface IRedisContext
{
    Task<bool> SaveAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<T?> GetAsync<T>(string key);
    Task<bool> DeleteAsync(string key);
}
