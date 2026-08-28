using CinemaBooking.Domain.Common;

namespace CinemaBooking.Domain.Entities
{
    public class ShowTime : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;
        //Một suất chiếu có nhiều vé được bán ra
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
