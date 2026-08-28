using CinemaBooking.Domain.Common;
using CinemaBooking.Domain.Enums;

namespace CinemaBooking.Domain.Entities
{
    public class Seat : BaseEntity
    {
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public SeatType Type { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;
    }
}
