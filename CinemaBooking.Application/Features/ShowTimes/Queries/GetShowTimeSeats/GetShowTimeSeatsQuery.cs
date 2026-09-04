using CinemaBooking.Application.DTOs;
using MediatR;

namespace CinemaBooking.Application.Features.ShowTimes.Queries.GetShowTimeSeats
{
    public class GetShowTimeSeatsQuery : IRequest<List<SeatDTO>>
    {
        public Guid ShowTimeId { get; set; }
        public Guid? UserId { get; set; }
        public GetShowTimeSeatsQuery(Guid showTimeId, Guid? userId = null)
        {
            ShowTimeId = showTimeId;
            UserId = userId;
        }
    }
}
