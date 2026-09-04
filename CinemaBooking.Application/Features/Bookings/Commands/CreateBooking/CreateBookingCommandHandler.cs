using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Seat> _seatRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<ShowTime> _showTimeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;

        public CreateBookingCommandHandler(IGenericRepository<Booking> bookingRepository, IGenericRepository<Seat> seatRepository, IGenericRepository<Ticket> ticketRepository, IGenericRepository<ShowTime> showTimeRepository, IUnitOfWork unitOfWork, IRedisService redisService)
        {
            _bookingRepository = bookingRepository;
            _seatRepository = seatRepository;
            _ticketRepository = ticketRepository;
            _showTimeRepository = showTimeRepository;
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }



        public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // 1. xác thực suất chiếu và ghế
            var showTime = await _showTimeRepository.GetByIdAsync(request.ShowTimeId);
            if(showTime == null)
            {
                throw new Exception("Suất chiếu không tồn tại");
            }

            var requestedSeats = await _seatRepository.FindAsync(s => request.SeatIds.Contains(s.Id));

            if(request.SeatIds.Count() != requestedSeats.Count())
            {
                throw new Exception("Danh sách ghế chứa ID không tồn tại");
            }

            if(requestedSeats.Any(s => s.RoomId != showTime.RoomId))
            {
                throw new Exception("Có ghế được chọn không thuộc phòng chiếu này");
            }
            

            // 2. bảo vệ tầng sql (chống đặt trùng vé đã được bán)
            var existingTickets = await _ticketRepository.FindAsync(t => t.ShowTimeId == request.ShowTimeId && request.SeatIds.Contains(t.SeatId));

            if (existingTickets.Any())
            {
                throw new Exception("Một hoặc nhiều ghế bạn chọn đã được bán. Vui lòng làm mới trang");
            }

            // 3. sắp xếp tài nguyên (chống deadlock trên redis)
            var sortedSeatIds = request.SeatIds.OrderBy(id => id).ToList().Distinct() ;

            var lockedKeys = new List<string>();
            var lockDuration = TimeSpan.FromMinutes(10);    // tgian giữ ghế chờ thanh toán

            try
            {
                // 4. Bảo vệ tầng Redis
                foreach(var seatId in sortedSeatIds)
                {
                    string lockKey = $"seatlock:{request.ShowTimeId}:{seatId}";
                    string lockValue = request.UserId.ToString();
                    bool isLocked = await _redisService.AcquireLockAsync(lockKey, lockValue, lockDuration);

                    if (!isLocked)
                    {
                        // nếu có 1 ghế fail, nhả ngay các ghế đã khóa thành công trước đó
                        throw new Exception($"Ghế bạn chọn đã có người khác nhanh tay hơn, vui lòng chọn ghế khác");
                    }
                    lockedKeys.Add(lockKey);
                }


                // 5. tính tiền ghế
                decimal totalPrice = 0;
                var ticketsToCreate = new List<Ticket>();

                foreach(var seat in requestedSeats)
                {
                    decimal price = seat.Type switch
                    {
                        Domain.Enums.SeatType.Normal => 80000m,
                        Domain.Enums.SeatType.VIP => 100000m,
                        Domain.Enums.SeatType.Sweetbox => 150000m,
                        _ => 80000m
                    };

                    totalPrice += price;

                    ticketsToCreate.Add(new Ticket
                    {
                        SeatId = seat.Id,
                        Price = price,
                        ShowTimeId = request.ShowTimeId
                    });
                }




                var booking = new Booking
                {
                    UserId = request.UserId,
                    Status = Domain.Enums.BookingStatus.Pending,
                    TotalPrice = totalPrice,
                    LockExpirationTime = DateTime.UtcNow.Add(lockDuration),
                    Tickets = ticketsToCreate
                };


                await _bookingRepository.AddAsync(booking);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return booking.Id;
            }
            catch (Exception)
            {
                foreach(var key in lockedKeys)
                {
                    await _redisService.ReleaseLockAsync(key);
                }
                throw;
            }
        }
    }
}
