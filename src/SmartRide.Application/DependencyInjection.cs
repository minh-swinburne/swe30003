using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartRide.Application.Behaviors;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Services;
using System.Reflection;

namespace SmartRide.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Register MediatR (Scan all handlers)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register FluentValidation (Scan all validators)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add Validation Pipeline to MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register all services in the Application layer
            //services.AddScoped<IUserService, UserService>();
            services.Scan(scan => scan
                .FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classes => classes.InNamespaces("SmartRide.Application.Services"))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}
