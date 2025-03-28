﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Data.DTOs.CurrentPrice;
using Data.DTOs.HistoricalPrice;
using Infrastructure.Binance;
using Integration.Tests.BrokerController.Stubs;
using Integration.Tests.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public class BrokerControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BrokerControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });

                services.AddHttpClient<IBinanceApi, BinanceApi>()
                    .ConfigurePrimaryHttpMessageHandler(sp =>
                    {
                        var config = sp.GetRequiredService<IOptions<BinanceApiSettings>>();
                        return new BinanceStubHttpMessageHandler(config);
                    });
            });
        }).CreateClient();
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should return a current price from Binance")]
    public async Task GetCurrentPrice_ShouldReturnPrice()
    {
        var response = await _client.GetFromJsonAsync<CurrentPriceResponse>("api/broker/prices/current?symbol=BTCUSDT");
        Assert.NotNull(response);
        Assert.Equal(104080.99000000m, response.Price);
        Assert.Equal("BTCUSDT", response.Symbol);
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should return a historical price from Binance")]
    public async Task GetHistoricalPrice_ShouldReturnHistoricalPrices()
    {
        var response =
            await _client.GetFromJsonAsync<HistoricalPriceResponse>(
                "api/broker/prices/historical?symbol=BTCUSDT&interval=1h");
        Assert.NotNull(response);
    }
}