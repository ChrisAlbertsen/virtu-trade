using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Data.Models;
using Infrastructure.Binance;
using Integration.Tests.BrokerController.Stubs;
using Microsoft.Extensions.Options;

public class BrokerControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BrokerControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient<IBinanceApi, BinanceApi>()
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        var config = services.BuildServiceProvider().GetRequiredService<IOptions<BinanceApiSettings>>();
                        return new BinanceStubHttpMessageHandler(config);
                    });
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetCurrentPrice_ShouldReturnPrice()
    {
        var response = await _client.GetFromJsonAsync<CurrentPriceResponse>("api/broker/prices/current?symbol=BTCUSDT");
        Assert.NotNull(response);
        Assert.Equal(104080.99000000m, response.Price);
        Assert.Equal("BTCUSDT", response.Symbol);
    }

    [Fact]
    public async Task GetHistoricalPrice_ShouldReturnHistoricalPrices()
    {
        var response = await _client.GetFromJsonAsync<HistoricalPriceResponse[]>("api/broker/prices/historical?symbol=BTCUSDT");
        Assert.NotNull(response);
        Assert.Equal(2, response.Length);
    }
}