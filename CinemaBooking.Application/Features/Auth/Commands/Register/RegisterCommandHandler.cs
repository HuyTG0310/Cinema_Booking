using CinemaBooking.Application.Interfaces;
using CinemaBooking.Domain.Entities;
using MediatR;

namespace CinemaBooking.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }


        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUsers = await _userRepository.FindAsync(u => u.Email == request.Email);

            if (existingUsers.Any())
            {
                throw new Exception("Email đã tồn tại");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };


            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
