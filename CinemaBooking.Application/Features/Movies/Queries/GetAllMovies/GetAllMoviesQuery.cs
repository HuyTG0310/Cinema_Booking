using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.Movies.Queries.GetAllMovies
{
    public class GetAllMoviesQuery : IRequest<IEnumerable<Movie>>
    {

    }
}
