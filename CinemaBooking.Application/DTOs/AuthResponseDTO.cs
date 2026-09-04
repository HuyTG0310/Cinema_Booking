namespace CinemaBooking.Application.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
    }
}
