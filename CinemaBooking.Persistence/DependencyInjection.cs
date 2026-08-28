using CinemaBooking.Application.Interfaces;
using CinemaBooking.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBooking.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            // Đăng ký Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Đăng ký UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }

}
