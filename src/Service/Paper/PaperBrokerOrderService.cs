using System;
using System.Threading.Tasks;
using Data.DTOs;
using Persistence;
using Service.Interfaces;

namespace Service.Paper;

public class PaperBrokerOrderService(IBrokerDataService brokerDataService, IPortfolioService portfolioService) : IBrokerOrderService
{
    public Task<OrderFulfillmentResponse> MarketOrder(Guid portfolioId, string symbol, decimal quantity)
    {
        var currentPriceResponse =  brokerDataService.GetCurrentPriceAsync(symbol);
        var tradeValue = quantity * currentPriceResponse.Result.Price;
        portfolioService.CheckAndReserveCashAmountAsync(portfolioId, tradeValue);
        // create trade in database
        // deduct cash

        // consider how trades should impact holdings and portfolio
    }
}