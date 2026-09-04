using CinemaBooking.Application.DTOs;
using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.Auth.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponseDTO>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LoginQueryHandler(IGenericRepository<User> userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }


        public async Task<AuthResponseDTO> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.FindAsync(u => u.Email == request.Email);

            var user = users.FirstOrDefault();

            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash)) {
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không chính xác");
            }

            var token = _jwtProvider.Generate(user);

            return new AuthResponseDTO
            {
                FullName = user.FullName,
                UserId = user.Id,
                Token = token
            };
        }
    }
}
