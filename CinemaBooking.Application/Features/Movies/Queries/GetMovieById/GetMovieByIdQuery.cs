using CinemaBooking.Application.DTOs;
using MediatR;

namespace CinemaBooking.Application.Features.Movies.Queries.GetMovieById
{
    public class GetMovieByIdQuery : IRequest<MovieDetailsDTO>
    {
        public Guid Id { get; set; }
        public GetMovieByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
