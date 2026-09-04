namespace CinemaBooking.Application.DTOs
{
    public class MovieByStatusDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int AgeRating { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int DurationMinutes { get; set; }
    }
}
