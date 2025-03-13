﻿using System;
using System.Threading.Tasks;
using Data.DTOs.CurrentPrice;
using Data.DTOs.Orders;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Service.Interfaces;
using Service.Paper;

namespace Service.Tests.Paper;

[TestSubject(typeof(PaperOrderService))]
public class PaperOrderServiceTests
{
    private readonly Mock<IBrokerDataService> _brokerDataService;
    private readonly PaperOrderService _paperOrderService;
    private readonly Mock<IPaperPortfolioService> _paperPortfolioService;
    private readonly Mock<IPaperTradeCatchService> _paperTradeCatchService;

    public PaperOrderServiceTests()
    {
        _brokerDataService = new Mock<IBrokerDataService>();
        _paperPortfolioService = new Mock<IPaperPortfolioService>();
        _paperTradeCatchService = new Mock<IPaperTradeCatchService>();
        var logger = new Mock<ILogger<PaperOrderService>>();
        _paperOrderService = new PaperOrderService(_paperPortfolioService.Object, _paperTradeCatchService.Object,
            _brokerDataService.Object, logger.Object);
    }

    [Trait("Category", "Unit test")]
    [Fact(DisplayName = "Should return an OrderFulfillmentResponse")]
    public async Task MarketOrder_ShouldReturnFulfillmentResponse()
    {
        
        var portfolioId = It.IsAny<Guid>();
        var symbol = It.IsAny<string>();
        var quantity = It.IsAny<int>();
        var price = It.IsAny<decimal>();
        var order = new MarketOrder(price, portfolioId, symbol, quantity);
        var marketOrderParams = new MarketOrderParams() {PortfolioId = portfolioId, Quantity = quantity, Symbol = symbol};

        var expectedResponse = new CurrentPriceResponse { Symbol = symbol, Price = price };
        _brokerDataService.Setup(api => api.GetCurrentPriceAsync(symbol)).ReturnsAsync(expectedResponse);
        _paperTradeCatchService.Setup(api => api.CatchTrade(order))
            .Returns(Task.CompletedTask);
        _paperPortfolioService.Setup(api => api.PayReservedCash(portfolioId, quantity * 45))
            .Returns(Task.CompletedTask);

        var result = await _paperOrderService.ExecuteMarketOrder(marketOrderParams);
        Assert.NotNull(result);
        Assert.Equal(symbol, result.Symbol);
        Assert.Equal(price, result.Price);
        Assert.Equal(quantity, result.Quantity);
    }

    [Trait("Category", "Unit test")]
    [Fact(DisplayName = "Should unreserve cash and throw exception")]
    public async Task MarketOrder_ShouldUnreserveCashAndThrowException()
    {
        var expectedErrorMessage = It.IsAny<string>();
        
        var portfolioId = It.IsAny<Guid>();
        var symbol = It.IsAny<string>();
        var quantity = It.IsAny<int>();
        var price = It.IsAny<decimal>();
        var order = new MarketOrder(price, portfolioId, symbol, quantity);
        var marketOrderParams = new MarketOrderParams() {PortfolioId = portfolioId, Quantity = quantity, Symbol = symbol};

        var expectedResponse = new CurrentPriceResponse { Symbol = symbol, Price = price };
        _brokerDataService.Setup(api => api.GetCurrentPriceAsync(symbol)).ReturnsAsync(expectedResponse);
        _paperTradeCatchService.Setup(api => api.CatchTrade(order))
            .Throws(new Exception(expectedErrorMessage));

        var exception = await Assert.ThrowsAsync<Exception>(() => _paperOrderService.ExecuteMarketOrder(marketOrderParams));
        Assert.Equal(expectedErrorMessage, exception.Message);
        _paperPortfolioService.Verify(api => api.UnreserveCash(portfolioId, It.IsAny<decimal>()), Times.Once);
    }
}