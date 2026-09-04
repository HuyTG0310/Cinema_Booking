using CinemaBooking.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CinemaBooking.API.Hubs
{
    public class SeatHub : Hub
    {

        private readonly IRedisService _redisService;

        public SeatHub(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task JoinShowTime(string showTimeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, showTimeId);
        }


        public async Task NotifySeatStatus(string showTimeId, Guid seatId, string status, string userId)
        {
            string lockKey = $"seatlock:{showTimeId.ToLower()}:{seatId.ToString().ToLower()}";
            
            if (status == "Locked")
            {
                // khóa ghế trong 10ph (chờ gian giữ chỗ thanh toán)
                await _redisService.AcquireLockAsync(lockKey, userId, TimeSpan.FromMinutes(10));
            }
            else
            {
                // hủy khóa nếu người dùng bỏ chọn
                await _redisService.ReleaseLockAsync(lockKey);
            }

            await Clients.Group(showTimeId).SendAsync("ReceiveSeatUpdate", seatId, status);
        }
    }
}
