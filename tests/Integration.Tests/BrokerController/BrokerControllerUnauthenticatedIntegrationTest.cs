using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Data.DTOs.Orders;
using Infrastructure.Binance;
using Integration.Tests.BrokerController.Stubs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Integration.Tests.BrokerController;

public class BrokerControllerUnauthenticatedIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BrokerControllerUnauthenticatedIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
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
    [Theory(DisplayName = "Not authenticated. Should return 401")]
    [InlineData("prices/current?symbol=BTCUSDT")]
    [InlineData("prices/historical?symbol=BTCUSDT&interval=1h")]
    public async Task GetEndpoints_WhenNotAuthenticated_ShouldReturnUnauthorized(string url)
    {
        var response = await Assert
            .ThrowsAsync<HttpRequestException>(
                () => _client.GetFromJsonAsync<object>($"api/broker/{url}"));
        Assert.Equal("Response status code does not indicate success: 401 (Unauthorized).", response.Message);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Trait("Category", "Integration test")]
    [Theory(DisplayName = "Not authenticated. Should return 401")]
    [InlineData("orders/execute-market-order")]
    public async Task PostEndpoints_WhenNotAuthenticated_ShouldReturnUnauthorized(string url)
    {
        var response =
            await _client.PostAsJsonAsync($"api/broker/{url}", It.IsAny<MarketOrderParams>());
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}