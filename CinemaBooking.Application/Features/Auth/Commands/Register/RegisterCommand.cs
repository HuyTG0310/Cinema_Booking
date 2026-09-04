using MediatR;

namespace CinemaBooking.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<bool>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterCommand(string fullName, string email, string password)
        {
            FullName = fullName;
            Email = email;
            Password = password;
        }
    }
}
