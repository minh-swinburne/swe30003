using SmartRide.API.Controllers.Conventions;
using SmartRide.Application;
using SmartRide.Common.Extensions;
using SmartRide.Infrastructure;

namespace SmartRide.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Clear default sources and manually load configuration
        builder.Configuration.ApplyAppSettings(builder.Environment);

        // Add services to the container.
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication(builder.Configuration);

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
