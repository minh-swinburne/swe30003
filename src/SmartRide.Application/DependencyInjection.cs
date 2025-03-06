using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SmartRide.Application.Behaviors;
using SmartRide.Infrastructure.Repositories;
using System.Reflection;

namespace SmartRide.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // 🔹 Register MediatR (Scan all handlers)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // 🔹 Register FluentValidation (Scan all validators)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // 🔹 Add Validation Pipeline to MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
