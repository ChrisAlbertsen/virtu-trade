using System;
using System.Threading.Tasks;
using Data.DTOs;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Paper;

public class PaperOrderService(
    IPaperPortfolioService paperPortfolioService,
    IPaperTradeCatchService paperTradeCatchService,
    IBrokerDataService brokerDataService,
    ILogger<PaperOrderService> logger)
    : IBrokerOrderService
{
    public async Task<OrderFulfillmentResponse> MarketOrder(Guid portfolioId, string symbol, decimal quantity)
    {
        var currentPriceResponse = await brokerDataService.GetCurrentPriceAsync(symbol);
        var orderValue = currentPriceResponse.Price * quantity;
        await paperPortfolioService.CheckAndReserveCashAmountAsync(portfolioId, orderValue);

        try
        {
            await paperTradeCatchService.CatchTrade(portfolioId, symbol, quantity, currentPriceResponse.Price);
        }
        catch (Exception e)
        {
            logger.LogError("An exception occured during catching a market order trade with {ExceptionMessage}", e.Message);
            await paperPortfolioService.UnreserveCash(portfolioId, orderValue);
            throw;
        }

        await paperPortfolioService.PayReservedCash(portfolioId, orderValue);

        return new OrderFulfillmentResponse
        {
            Id = Guid.NewGuid(),
            Symbol = symbol,
            Price = currentPriceResponse.Price,
            Quantity = quantity
        };
    }
}