using CinemaBooking.Application.Features.Movies.Commands.CreateMovie;
using CinemaBooking.Application.Features.Movies.Queries.GetMovieById;
using CinemaBooking.Application.Features.Movies.Queries.GetMoviesByStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MoviesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("by-status")]
        public async Task<IActionResult> GetMoviesByStatus([FromQuery] string status)
        {
            var query = new GetMoviesByStatusQuery(status);
            var movies = await _mediator.Send(query);
            return Ok(movies);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            var query = new GetMovieByIdQuery(id);
            var movie = await _mediator.Send(query);
            return Ok(movie);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieCommand command)
        {
            var movieId = await _mediator.Send(command);

            return Ok(new { Message = "Tạo thành công", Id = movieId });
        }

    }
}
