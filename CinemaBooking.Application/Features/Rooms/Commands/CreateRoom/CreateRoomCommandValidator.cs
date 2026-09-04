using FluentValidation;

namespace CinemaBooking.Application.Features.Rooms.Commands.CreateRoom
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên phòng không được để trống");
            RuleFor(x => x.NumRows).InclusiveBetween(1, 26).WithMessage("Số hàng ghế phải từ 1 đến 26 (A-Z).");
            RuleFor(x => x.SeatsPerRow).GreaterThan(0).WithMessage("Số ghế mỗi hàng phải lớn hơn 0");
        }
    }
}
