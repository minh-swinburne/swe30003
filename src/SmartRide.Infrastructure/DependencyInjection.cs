using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartRide.Domain.Interfaces;
using SmartRide.Infrastructure.Persistence;
using SmartRide.Infrastructure.Repositories;

namespace SmartRide.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // 🔹 Register DbContext
            //services.AddDbContext<DbContext>(options =>
            //    options.UseSqlServer(connectionString));

            // Register repositories of all entity types
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
