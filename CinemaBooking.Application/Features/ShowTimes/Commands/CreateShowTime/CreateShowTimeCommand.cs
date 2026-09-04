using MediatR;

namespace CinemaBooking.Application.Features.ShowTimes.Commands.CreateShowTime
{
    public class CreateShowTimeCommand : IRequest<Guid>
    {
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
