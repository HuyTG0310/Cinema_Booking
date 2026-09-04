using MediatR;

namespace CinemaBooking.Application.Features.Bookings.Commands.ConfirmPayment
{
    public class ConfirmPaymentCommand : IRequest<bool>
    {
        public Guid BookingId { get; set; }
        
        public ConfirmPaymentCommand(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
