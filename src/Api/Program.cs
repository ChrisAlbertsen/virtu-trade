using System.Globalization;
using System.Net.Http;
using Infrastructure.Binance;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Service.Binance;
using Service.Interfaces;
using Service.Paper;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<ApplicationDatabaseContext>(options => options.UseNpgsql(connectionString));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PaperBrokerOrderService>());

builder.Services.AddScoped<IBrokerDataService, BinanceBrokerDataService>();
builder.Services.AddScoped<IBrokerOrderService, PaperBrokerOrderService>();
builder.Services.AddScoped<IPaperTradeService, PaperTradeService>();
builder.Services.AddScoped<IPaperHoldingService, PaperHoldingService>();
builder.Services.AddScoped<IPortfolioService, PaperPortfolioService>();
builder.Services.AddScoped<IBinanceApi, BinanceApi>();
builder.Services.AddScoped<HttpClient>();

builder.Services.Configure<BinanceApiSettings>(builder.Configuration.GetSection("BinanceApi"));

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

public partial class Program
{
}