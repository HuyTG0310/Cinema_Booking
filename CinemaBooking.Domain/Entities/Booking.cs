using CinemaBooking.Domain.Common;
using CinemaBooking.Domain.Enums;

namespace CinemaBooking.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime LockExpirationTime { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
