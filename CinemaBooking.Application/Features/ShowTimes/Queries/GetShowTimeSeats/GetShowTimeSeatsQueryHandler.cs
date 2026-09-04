using CinemaBooking.Application.DTOs;
using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.ShowTimes.Queries.GetShowTimeSeats
{
    public class GetShowTimeSeatsQueryHandler : IRequestHandler<GetShowTimeSeatsQuery, List<SeatDTO>>
    {
        private readonly IGenericRepository<ShowTime> _showTimeRepository;
        private readonly IGenericRepository<Seat> _seatRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IRedisService _redisService;

        public GetShowTimeSeatsQueryHandler(IGenericRepository<ShowTime> showTimeRepository, IGenericRepository<Seat> seatRepository, IGenericRepository<Ticket> ticketRepository, IRedisService redisService)
        {
            _showTimeRepository = showTimeRepository;
            _seatRepository = seatRepository;
            _ticketRepository = ticketRepository;
            _redisService = redisService;
        }

        public async Task<List<SeatDTO>> Handle(GetShowTimeSeatsQuery request, CancellationToken cancellationToken)
        {
            var showTime = await _showTimeRepository.GetByIdAsync(request.ShowTimeId);

            if (showTime == null)
            {
                throw new Exception("Suất chiếu không tồn tại.");
            }

            var allSeats = await _seatRepository.FindAsync(s => s.RoomId == showTime.RoomId);

            var soldTickets = await _ticketRepository.FindAsync(t => t.ShowTimeId == request.ShowTimeId);

            var soldSeatIds = soldTickets.Select(t => t.SeatId).ToList();

            var result = new List<SeatDTO>();

            foreach (var seat in allSeats.OrderBy(s => s.Row).ThenBy(s => s.Number))
            {
                // mặc định ghế là có sẵn
                var dto = new SeatDTO
                {
                    Id = seat.Id,
                    Row = seat.Row,
                    Number = seat.Number,
                    Type = seat.Type.ToString(),
                    Price = seat.Type switch
                    {
                        Domain.Enums.SeatType.Normal => 80000,
                        Domain.Enums.SeatType.VIP => 100000,
                        Domain.Enums.SeatType.Sweetbox => 150000,
                        _ => 80000
                    },
                    Status = "Available"
                };
                
                // nếu seat id nằm trong các vé đã bán
                if (soldSeatIds.Contains(seat.Id))
                {
                    dto.Status = "Sold";
                }
                // nếu chưa bán thì check coi có ai đang thanh toán (lock ghế này ko)
                else
                {
                    string lockKey = $"seatlock:{request.ShowTimeId.ToString().ToLower()}:{seat.Id.ToString().ToLower()}";
                    // lấy value dựa trên key
                    string lockOwner = await _redisService.GetValueAsync(lockKey);

                    // nếu có value tức là ghế bị lock
                    if (!string.IsNullOrEmpty(lockOwner))
                    {
                        // kiểm tra xem mình có là người lock ko
                        if(request.UserId.HasValue && lockOwner == request.UserId.Value.ToString())
                        {
                            dto.Status = "MyLocked";
                        }
                        else
                        {
                            dto.Status = "Locked";
                        }
                    }
                }

                result.Add(dto);
            }

            return result;
        }
    }
}
