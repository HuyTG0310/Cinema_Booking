using MediatR;

namespace CinemaBooking.Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<Guid>
    {
        public Guid ShowTimeId { get; set; }
        public List<Guid> SeatIds { get; set; } = new List<Guid>();
        public Guid UserId { get; set; }

        public CreateBookingCommand(Guid userId, Guid showTimeId, List<Guid> seatIds)
        {
            UserId = userId;
            ShowTimeId = showTimeId;
            SeatIds = seatIds;
        }
    }
}
