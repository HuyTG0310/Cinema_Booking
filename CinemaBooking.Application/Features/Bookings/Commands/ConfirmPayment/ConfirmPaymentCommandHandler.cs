using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.Bookings.Commands.ConfirmPayment
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, bool>
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;

        public ConfirmPaymentCommandHandler(
            IGenericRepository<Booking> bookingRepository,
            IGenericRepository<Ticket> ticketRepository,
            IUnitOfWork unitOfWork,
            IRedisService redisService)
        {
            _bookingRepository = bookingRepository;
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }

        public async Task<bool> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            // 1. kiểm tra đơn hàng
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

            if(booking == null)
            {
                throw new Exception("Không tìm thấy đơn hàng");
            }

            if(booking.Status != Domain.Enums.BookingStatus.Pending)
            {
                throw new Exception("Đơn hàng này đã được xử lý");
            }

            // 2. cập nhật trạng thái thành Paid
            booking.Status = Domain.Enums.BookingStatus.Paid;
            _bookingRepository.Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 3. xóa lock trên redis chủ động
            var tickets = await _ticketRepository.FindAsync(t => t.BookingId == request.BookingId);

            foreach(var ticket in tickets)
            {
                string lockKey = $"seatlock:{ticket.ShowTimeId}:{ticket.SeatId}";
                await _redisService.ReleaseLockAsync(lockKey);
            }

            return true;


        }
    }
}
