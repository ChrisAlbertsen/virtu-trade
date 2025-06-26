using System.Net.Http.Json;
using System.Threading.Tasks;
using Data.DTOs.CurrentPrice;
using Data.DTOs.HistoricalPrice;
using Integration.Tests.TestData;

namespace Integration.Tests.BrokerDataActions;

[Collection("IntegrationTest")]
public class BrokerControllerIntegrationTests(IntegrationTestSessionFactory factory) : BaseIntegrationTest(factory)
{
    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should return a current price from Binance")]
    public async Task GetCurrentPrice_ShouldReturnPrice()
    {
        var response =
            await HttpClient.GetFromJsonAsync<CurrentPriceResponse>(
                "api/broker/prices/current?symbol=BTCUSDT");
        Assert.NotNull(response);
        Assert.Equal(104080.99000000m, response.Price);
        Assert.Equal("BTCUSDT", response.Symbol);
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should return a historical price from Binance")]
    public async Task GetHistoricalPrice_ShouldReturnHistoricalPrices()
    {
        var response =
            await HttpClient.GetFromJsonAsync<HistoricalPriceResponse>(
                "api/broker/prices/historical?symbol=BTCUSDT&interval=1h");
        Assert.NotNull(response);
    }
}