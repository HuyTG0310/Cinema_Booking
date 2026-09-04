namespace CinemaBooking.Application.Interfaces
{
    public interface IBookingCleanupService
    {
        Task CancelExpireBookingsAsync();
    }
}
