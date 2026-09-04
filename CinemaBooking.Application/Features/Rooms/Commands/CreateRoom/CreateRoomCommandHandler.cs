using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Enums;
using MediatR;

namespace CinemaBooking.Application.Features.Rooms.Commands.CreateRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
    {

        private readonly IGenericRepository<Room> _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoomCommandHandler(IGenericRepository<Room> roomRepository, IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var totalSeats = request.NumRows * request.SeatsPerRow;


            var room = new Room
            {
                Name = request.Name,
                TotalSeats = totalSeats,
                Seats = new List<Seat>()
            };

            for (int i = 0; i < request.NumRows; i++)
            {
                char rowChar = (char)('A' + i);
                string rowName = rowChar.ToString();


                SeatType currentStatus = SeatType.Normal;

                // nếu rạp có từ hàng 5 trở lên thì mới chia VIP
                if (request.NumRows >= 5)
                {
                    // hàng cuối là sweet box
                    if (i == request.NumRows - 1)
                    {
                        currentStatus = SeatType.Sweetbox;
                    }
                    else if (i >= 3)
                    {
                        currentStatus = SeatType.VIP;
                    }
                }


                for (int j = 1; j <= request.SeatsPerRow; j++)
                {
                    var seat = new Seat
                    {
                        Row = rowName,
                        Number = j,
                        Type = currentStatus
                    };

                    room.Seats.Add(seat);
                }
            }

            await _roomRepository.AddAsync(room);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return room.Id;
        }
    }
}
