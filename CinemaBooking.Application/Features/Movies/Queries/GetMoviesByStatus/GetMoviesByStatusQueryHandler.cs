using CinemaBooking.Application.DTOs;
using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.Movies.Queries.GetMoviesByStatus
{
    public class GetMoviesByStatusQueryHandler : IRequestHandler<GetMoviesByStatusQuery, IEnumerable<MovieByStatusDTO>>
    {
        private readonly IGenericRepository<Movie> _movieRepository;

        public GetMoviesByStatusQueryHandler(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }


        public async Task<IEnumerable<MovieByStatusDTO>> Handle(GetMoviesByStatusQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository.FindAsync(m => m.Status == request.Status && m.IsActive);

            var movieDTOs = new List<MovieByStatusDTO>();


            foreach (var movie in movies)
            {
                var movieDTO = new MovieByStatusDTO
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    AgeRating = movie.AgeRating,
                    PosterUrl = movie.PosterUrl,
                    TrailerUrl = movie.TrailerUrl,
                    DurationMinutes = movie.DurationMinutes,
                    Genre = movie.Genre,
                    ReleaseDate = movie.ReleaseDate
                };
                movieDTOs.Add(movieDTO);
            }

            return movieDTOs;
        }
    }
}
