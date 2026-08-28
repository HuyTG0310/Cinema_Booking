using MediatR;

namespace CinemaBooking.Application.Features.Rooms
{
    public class CreateRoomCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public int NumRows { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
