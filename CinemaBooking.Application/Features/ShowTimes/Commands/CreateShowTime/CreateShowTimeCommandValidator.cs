using FluentValidation;

namespace CinemaBooking.Application.Features.ShowTimes.Commands.CreateShowTime
{
    public class CreateShowTimeCommandValidator : AbstractValidator<CreateShowTimeCommand>
    {
        public CreateShowTimeCommandValidator()
        {
            RuleFor(x => x.MovieId).NotEmpty().WithMessage("Vui lòng chọn phim");
            RuleFor(x => x.RoomId).NotEmpty().WithMessage("Vui lòng chọn phòng chiếu");
            RuleFor(x => x.StartTime).GreaterThan(DateTime.UtcNow).WithMessage("Thời gian chiếu phải ở trong tương lai");
        }
    }
}
