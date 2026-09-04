namespace CinemaBooking.Application.DTOs
{
    public class CreateBookingRequest
    {
        public Guid ShowTimeId { get; set; }
        public List<Guid> SeatIds { get; set; } = new List<Guid>();
    }
}
