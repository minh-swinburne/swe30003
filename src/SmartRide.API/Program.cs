using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartRide.API.Controllers.Conventions;
using SmartRide.Application;
using SmartRide.Common.Extensions;
using SmartRide.Infrastructure;
using SmartRide.Infrastructure.Settings;
using System.Text;

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

        // Add authentication
        var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
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
        app.UseAuthentication(); // Add authentication middleware
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
