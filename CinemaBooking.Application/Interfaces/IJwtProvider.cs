using CinemaBooking.Domain.Entities;

namespace CinemaBooking.Application.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
