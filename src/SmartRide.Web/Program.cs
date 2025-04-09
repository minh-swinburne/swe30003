using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartRide.Web;
using SmartRide.Web.Services;
using SmartRide.Web.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddre   ss) });

// Register services
builder.Services.AddScoped<IUserService, UserService>();

// Register ApiClient as the main entry point for API calls
builder.Services.AddScoped<ApiClient>();

await builder.Build().RunAsync();
