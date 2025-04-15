using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Data.AuthModels;
using Infrastructure.Binance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Service.Binance;
using Service.Interfaces;
using Service.Paper;
using Service.Paper.Authorization;
using IAuthorizationService = Service.Interfaces.IAuthorizationService;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(o => { });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IBrokerDataService, BinanceBrokerDataService>();
builder.Services.AddScoped<IBrokerOrderService, PaperOrderService>();
builder.Services.AddScoped<IPaperPortfolioService, PaperPortfolioService>();
builder.Services.AddScoped<IPaperTradeCatchService, PaperTradeCatchService>();
builder.Services.AddScoped<IBinanceApi, BinanceApi>();
builder.Services.AddScoped<IAuthorizationService, PaperAuthorizationService>();
builder.Services.AddScoped<HttpClient>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthorizationHandler, PortfolioAccessHandler>();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanAccessPortfolio", policy =>
        policy.Requirements.Add(new PortfolioAccessRequirement()));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();


builder.Services.Configure<BinanceApiSettings>(builder.Configuration.GetSection("BinanceApi"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();

public partial class Program
{
}