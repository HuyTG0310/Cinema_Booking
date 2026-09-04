namespace CinemaBooking.Application.DTOs
{
    public class SeatDTO
    {
        public Guid Id { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public string Name => $"{Row}{Number}";
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}
