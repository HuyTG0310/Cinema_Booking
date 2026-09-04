using CinemaBooking.Application.DTOs;
using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.Movies.Queries.GetMovieById
{
    public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieDetailsDTO>
    {
        private readonly IGenericRepository<Movie> _movieRepository;


        public GetMovieByIdQueryHandler(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }


        public async Task<MovieDetailsDTO> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetByIdAsync(request.Id);
            if (movie == null || !movie.IsActive)
            {
                throw new Exception("Không tìm thấy bộ phim này");
            }


            var movieDTO = new MovieDetailsDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                Cast = movie.Cast,
                Genre = movie.Genre,
                ReleaseDate = movie.ReleaseDate,
                DurationMinutes = movie.DurationMinutes,
                Language = movie.Language,
                AgeRating = movie.AgeRating,
                PosterUrl = movie.PosterUrl,
                TrailerUrl = movie.TrailerUrl
            };

            return movieDTO;
        }
    }
}
