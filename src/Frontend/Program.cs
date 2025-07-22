using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using Frontend.Api.Clients;
using Frontend.Components.Alert;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<BrokerApiClient>();
builder.Services.AddScoped<PaperApiClient>();

builder.Services.AddSingleton<AlertService>();

// Optionally configure base address:
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri("http://localhost:5070/") };
    return client;
});
await builder.Build().RunAsync();