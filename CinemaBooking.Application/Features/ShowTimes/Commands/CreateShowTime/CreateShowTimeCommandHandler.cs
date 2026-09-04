using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.ShowTimes.Commands.CreateShowTime
{
    public class CreateShowTimeCommandHandler : IRequestHandler<CreateShowTimeCommand, Guid>
    {
        private readonly IGenericRepository<ShowTime> _showTimeRepository;
        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateShowTimeCommandHandler(
            IGenericRepository<ShowTime> showTimeRepository,
            IGenericRepository<Movie> movieRepository,
            IUnitOfWork unitOfWork)
        {
            _showTimeRepository = showTimeRepository;
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateShowTimeCommand request, CancellationToken cancellationToken)
        {
            var movie =  await _movieRepository.GetByIdAsync(request.MovieId);

            if(movie == null)
            {
                throw new Exception("Không tìm thấy thông tin phim.");
            }

            var endTime = request.StartTime.AddMinutes(movie.DurationMinutes + 15);

            var showTime = new ShowTime
            {
                MovieId = request.MovieId,
                RoomId = request.RoomId,
                StartTime = request.StartTime,
                EndTime = endTime
            };

            await _showTimeRepository.AddAsync(showTime);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return showTime.Id;
        }
    }
}
