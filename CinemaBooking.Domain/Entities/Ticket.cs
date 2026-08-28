using CinemaBooking.Domain.Common;

namespace CinemaBooking.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public decimal Price { get; set; }
        public Guid ShowTimeId { get; set; }
        public ShowTime ShowTime { get; set; } = null!;
        public Guid SeatId { get; set; }
        public Seat Seat { get; set; } = null!;
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
    }
}
