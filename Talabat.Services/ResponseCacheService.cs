using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Services.Contract;

namespace Talabat.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly StackExchange.Redis.IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = (StackExchange.Redis.IDatabase?)redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response is null) return;
            var serializeOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedResponse = JsonSerializer.Serialize(response, serializeOptions);
            await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
        }

        public async Task<string?> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);
            if (cachedResponse.IsNullOrEmpty) return null;
            return cachedResponse;
        }
    }
}
