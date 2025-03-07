using SmartRide.Application;
using SmartRide.Common.Configuration;
using SmartRide.Infrastructure;
using SmartRide.WebAPI.Controllers.Conventions;

namespace SmartRide.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register ConfigurationService as Singleton
        builder.Services.AddSingleton<IConfigurationService>(sp =>
        {
            var env = sp.GetRequiredService<IHostEnvironment>();
            return new ConfigurationService(env);
        });

        // Add services to the container.
        builder.Services.AddInfrastructure();
        builder.Services.AddApplication();

        // Add custom controller conventions
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new PluralizeConvention());
            options.Conventions.Add(new KebaberizeConvention());
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}
