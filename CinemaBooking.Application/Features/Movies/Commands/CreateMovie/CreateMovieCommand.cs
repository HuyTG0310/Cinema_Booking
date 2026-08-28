using MediatR;

namespace CinemaBooking.Application.Features.Movies.Commands.CreateMovie
{
    public class CreateMovieCommand : IRequest<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public int AgeLimit { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
    }
}
