using CinemaBooking.Application.DTOs;
using MediatR;

namespace CinemaBooking.Application.Features.Movies.Queries.GetMoviesByStatus
{
    public class GetMoviesByStatusQuery : IRequest<IEnumerable<MovieByStatusDTO>>
    {
        public string Status { get; set; }
        public GetMoviesByStatusQuery(string status)
        {
            Status = status;
        }
    }
}
