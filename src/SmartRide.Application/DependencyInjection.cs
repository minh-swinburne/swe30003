using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SmartRide.Application.Behaviors;
using SmartRide.Common.Configuration;

namespace SmartRide.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register MediatR (Scan all handlers)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register FluentValidation (Scan all validators)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add Validation Pipeline to MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Register AutoMapper
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
