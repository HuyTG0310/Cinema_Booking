using CinemaBooking.Application.Interfaces;
using StackExchange.Redis;

namespace CinemaBooking.Infrastructure.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiration)
        {
            var db = _redis.GetDatabase();
            var existingValue = await db.StringGetAsync(key);

            if (!existingValue.IsNullOrEmpty)
            {
                if(existingValue.ToString() == value)
                {
                    await db.KeyExpireAsync(key, expiration);
                    return true;
                }
                // Khóa của người khác
                return false;
            }

            return await db.StringSetAsync(key, value, expiration, When.NotExists);
        }

        public async Task ReleaseLockAsync(string key)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.KeyExistsAsync(key);
        }

        public async Task<string> GetValueAsync(string key)
        {
            var existingValue = await _redis.GetDatabase().StringGetAsync(key);
            return existingValue.HasValue ? existingValue.ToString() : null;
        }
    }
}
