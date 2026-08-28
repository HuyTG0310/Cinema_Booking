using FluentValidation;

namespace CinemaBooking.Application.Features.Movies.Commands.CreateMovie
{
    public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
    {
        public CreateMovieCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tên phim không được để trống.")
                .MaximumLength(200).WithMessage("Tên phim không được vượt quá 200 ký tự.");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Thời lượng phim phải lớn hơn 0 phút.");

            RuleFor(x => x.AgeLimit)
                .GreaterThanOrEqualTo(0).WithMessage("Giới hạn độ tuổi không hợp lệ.");

            RuleFor(x => x.PosterUrl)
                .NotEmpty().WithMessage("Poster URL không được để trống.");
        }
    }
}
