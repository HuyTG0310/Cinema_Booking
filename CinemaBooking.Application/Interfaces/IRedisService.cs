namespace CinemaBooking.Application.Interfaces
{
    public interface IRedisService
    {
        Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiration);
        Task ReleaseLockAsync(string key);
        Task<bool> KeyExistsAsync(string key);
        Task<string> GetValueAsync(string key);
    }
}
