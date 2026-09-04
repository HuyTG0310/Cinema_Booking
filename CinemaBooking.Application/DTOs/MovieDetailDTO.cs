namespace CinemaBooking.Application.DTOs
{
    public class MovieDetailsDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int DurationMinutes { get; set; }
        public string Language { get; set; }
        public int AgeRating { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
    }
}
