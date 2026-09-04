using CinemaBooking.Application.DTOs;
using MediatR;

namespace CinemaBooking.Application.Features.Auth.Queries.Login
{
    public class LoginQuery : IRequest<AuthResponseDTO>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
