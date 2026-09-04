using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;

namespace CinemaBooking.Application.Services
{
    public class BookingCleanupService : IBookingCleanupService
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookingCleanupService(IGenericRepository<Booking> bookingRepository, IGenericRepository<Ticket> ticketRepository, IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CancelExpireBookingsAsync()
        {
            var expiredBookings = await _bookingRepository.FindAsync(b => b.Status == Domain.Enums.BookingStatus.Pending 
            && b.LockExpirationTime < DateTime.UtcNow);

            if (!expiredBookings.Any())
                return;

            foreach (var booking in expiredBookings)
            {
                // 1. hủy đơn
                booking.Status = Domain.Enums.BookingStatus.Cancelled;
                _bookingRepository.Update(booking);

                // 2. dọn dẹp vé treo
                var pendingTickets = await _ticketRepository.FindAsync(t => t.BookingId == booking.Id);
                foreach(var ticket in pendingTickets)
                {
                    _ticketRepository.Delete(ticket);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
