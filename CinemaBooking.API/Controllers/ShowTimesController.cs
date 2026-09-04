using CinemaBooking.Application.Features.ShowTimes.Commands.CreateShowTime;
using CinemaBooking.Application.Features.ShowTimes.Queries.GetShowTimeSeats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShowTimesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShowTime([FromBody] CreateShowTimeCommand command)
        {
            var showTimeId = await _mediator.Send(command);
            return Ok(new { ShowTimeId = showTimeId, Message = "Tạo suất chiếu thành công" });
        }

        [HttpGet("{showTimeId}/seats")]
        public async Task<IActionResult> GetSeats(Guid showTimeId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            Guid? currentUserId = string.IsNullOrEmpty(userIdClaim) ? null : Guid.Parse(userIdClaim);
            var query = new GetShowTimeSeatsQuery(showTimeId, currentUserId);
            var seats = await _mediator.Send(query);
            return Ok(seats);
        }
    }
}
