using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using SmartRide.Web;
using SmartRide.Web.Services;
using SmartRide.Web.Services.Interfaces;
using SmartRide.Web.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register settings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(nameof(ApiSettings)));

// Register HttpClient
builder.Services.AddScoped<HttpClient>(sp =>
{
    var apiSettings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
    return new() { BaseAddress = new Uri(apiSettings.BaseUrl) };
});

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Register ApiClient as the main entry point for API calls
builder.Services.AddScoped<ApiClient>();

await builder.Build().RunAsync();
