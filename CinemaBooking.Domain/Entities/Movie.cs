using CinemaBooking.Domain.Common;

namespace CinemaBooking.Domain.Entities
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public int AgeLimit { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
    }
}
