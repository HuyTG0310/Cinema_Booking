using CinemaBooking.Application.DTOs;
using CinemaBooking.Application.Features.Bookings.Commands.ConfirmPayment;
using CinemaBooking.Application.Features.Bookings.Commands.CreateBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return Unauthorized(new { Message = "Token không hợp lệ hoặc thiếu thông tin định danh." });
                }

                var command = new CreateBookingCommand(userId, request.ShowTimeId, request.SeatIds);

                var bookingId = await _mediator.Send(command);

                return Ok(new { BookingId = bookingId, Message = "Giữ ghế thành công. Vui lòng thanh toán trong 10ph" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmPayment(Guid id)
        {
            var command = new ConfirmPaymentCommand(id);
            var result = await _mediator.Send(command);
            return Ok(new { Message = "Thanh toán thành công! Vé đã được xuất" });
        }
    }
}
