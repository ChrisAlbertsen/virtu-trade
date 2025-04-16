using System.Threading.Tasks;
using Data.DTOs.CurrentPrice;
using Integration.Tests.TestData;
using JetBrains.Annotations;
using Service.Paper;

namespace Integration.Tests.Paper.PortfolioActions;

[TestSubject(typeof(PaperPortfolioService))]
public class CreatePortfolioTests : BaseIntegrationTest
{
    private readonly PaperPortfolioService _paperPortfolioService;

    public CreatePortfolioTests(IntegrationTestAppDbFactory dbFactory) : base(dbFactory)
    {
    }
    
    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should return a current price from Binance")]
    public async Task CreatePortfolio_ShouldReturnGuid()
    {
        var response = await _client.GetFromJsonAsync<CurrentPriceResponse>("api/broker/prices/current?symbol=BTCUSDT");
        Assert.NotNull(response);
        Assert.Equal(104080.99000000m, response.Price);
        Assert.Equal("BTCUSDT", response.Symbol);
    }
}