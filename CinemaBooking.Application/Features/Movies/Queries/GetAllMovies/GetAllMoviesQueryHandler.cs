using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.Movies.Queries.GetAllMovies
{
    public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, IEnumerable<Movie>>
    {
        private readonly IGenericRepository<Movie> _movieRepository;

        public GetAllMoviesQueryHandler(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            return await _movieRepository.GetAllAsync();
        }
    }
}
