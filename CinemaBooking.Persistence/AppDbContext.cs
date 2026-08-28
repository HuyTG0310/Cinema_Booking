using CinemaBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<ShowTime> ShowTimes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình bảng Ticket (Bảng trung gian quan trọng nhất)
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Price).HasColumnType("decimal(18,2)");

                // 1 Ticket thuộc về 1 ShowTime
                entity.HasOne(t => t.ShowTime)
                      .WithMany(s => s.Tickets)
                      .HasForeignKey(t => t.ShowTimeId)
                      .OnDelete(DeleteBehavior.Restrict); // Không cho xóa ShowTime nếu đã có vé

                // 1 Ticket gắn với 1 Booking
                entity.HasOne(t => t.Booking)
                      .WithMany(b => b.Tickets)
                      .HasForeignKey(t => t.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);

                // 1 Ticket gắn với 1 Seat
                entity.HasOne(t => t.Seat)
                      .WithMany() // Seat không cần list Ticket
                      .HasForeignKey(t => t.SeatId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // 2. Cấu hình bảng Booking
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(b => b.Status).HasConversion<string>(); // Lưu Enum dưới dạng chuỗi (Pending, Paid) thay vì số
            });

            // 3. Cấu hình bảng Seat
            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Type).HasConversion<string>();

                entity.HasOne(s => s.Room)
                      .WithMany(r => r.Seats)
                      .HasForeignKey(s => s.RoomId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 4. Cấu hình bảng ShowTime
            modelBuilder.Entity<ShowTime>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.HasOne(s => s.Movie)
                      .WithMany()
                      .HasForeignKey(s => s.MovieId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Room)
                      .WithMany()
                      .HasForeignKey(s => s.RoomId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
