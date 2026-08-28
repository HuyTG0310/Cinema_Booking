using CinemaBooking.Domain.Common;

namespace CinemaBooking.Domain.Entities
{
    public class Room : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
