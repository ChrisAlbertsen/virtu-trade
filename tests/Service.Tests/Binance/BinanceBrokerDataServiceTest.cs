using Data.DTOs.CurrentPrice;
using Data.DTOs.HistoricalPrice;
using Infrastructure.Binance;
using JetBrains.Annotations;
using Moq;
using Service.Binance;

namespace Service.Tests.Binance;

[TestSubject(typeof(BinanceBrokerDataService))]
public class BinanceBrokerDataServiceTests
{
    private readonly BinanceBrokerDataService _binanceBrokerDataService;
    private readonly Mock<IBinanceApi> _mockBinanceApi;

    public BinanceBrokerDataServiceTests()
    {
        _mockBinanceApi = new Mock<IBinanceApi>();
        _binanceBrokerDataService = new BinanceBrokerDataService(_mockBinanceApi.Object);
    }

    [Trait("Category", "Unit test")]
    [Fact(DisplayName = "Should return an unmodified CurrentPriceResponse")]
    public async void GetCurrentPriceAsync_ShoudlReturnCurrentPriceResponse()
    {
        var symbol = "BTCUSDT";
        var expectedResponse = new CurrentPriceResponse { Symbol = symbol, Price = 45000.50m };

        _mockBinanceApi.Setup(api => api.GetCurrentPriceAsync(symbol)).ReturnsAsync(expectedResponse);

        var result = await _binanceBrokerDataService.GetCurrentPriceAsync(symbol);


        Assert.NotNull(result);
        Assert.Equal(expectedResponse, result);
    }

    [Trait("Category", "Unit test")]
    [Fact(DisplayName = "Should return an unmodified HistoricalPriceResponse")]
    public async void GetHistoricalPriceAsync_ShoudlReturnHistoricalPriceResponse()
    {
        var historicalPriceParams = new HistoricalPriceParams
        {
            Symbol = "BTCUSDT",
            Interval = "m"
        };

        var expectedResponse = new HistoricalPriceResponse
        {
            HistoricalPrices =
            [
                new HistoricalPrice
                {
                    OpenTime = 1630453200000L,
                    OpenPrice = 45000.50m,
                    HighPrice = 46000.75m,
                    LowPrice = 44000.25m,
                    ClosePrice = 45500.60m,
                    Volume = 3500.75m,
                    CloseTime = 1630456800000L,
                    QuoteVolume = 160000000m,
                    NumberOfTrades = 1250,
                    TakerBuyBaseAssetVolume = 1800.50m,
                    TakerBuyQuoteAssetVolume = 90050000m,
                    UnusedField = 0.00m
                }
            ]
        };

        _mockBinanceApi.Setup(api => api.GetHistoricalPriceAsync(historicalPriceParams)).ReturnsAsync(expectedResponse);


        var result = await _binanceBrokerDataService.GetHistoricalPriceAsync(historicalPriceParams);

        Assert.NotNull(result);
        Assert.Equal(expectedResponse, result);
    }
}