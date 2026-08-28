using CinemaBooking.Application.Features.Movies.Commands.CreateMovie;
using CinemaBooking.Application.Features.Movies.Queries.GetAllMovies;
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


        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _mediator.Send(new GetAllMoviesQuery());
            return Ok(movies);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieCommand command)
        {
            var movieId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetAllMovies), new { id = movieId }, movieId);
        }

    }
}
