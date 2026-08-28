using CinemaBooking.Application.Features.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
        {
            var roomId = await _mediator.Send(command);
            return Ok(new { RoomId = roomId, Message = "Tạo phòng và sinh ghế thành công" });
        }
    }
}
