using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBooking.Application.Features.Movies.Commands.CreateMovie
{
    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Guid>
    {

        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMovieCommandHandler(IGenericRepository<Movie> movieRepository, IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = new Movie
            {
                Title = request.Title,
                DurationMinutes = request.DurationMinutes,
                AgeLimit = request.AgeLimit,
                PosterUrl = request.PosterUrl
            };

            await _movieRepository.AddAsync(movie);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return movie.Id;
        }
    }
}
