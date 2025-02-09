using System;
using System.Threading.Tasks;
using Data.DTOs;
using MediatR;
using Persistence;
using Service.Interfaces;

namespace Service.Paper;

public class PaperBrokerOrderService(IMediator mediator) : IBrokerOrderService
{
    public Task<OrderFulfillmentResponse> MarketOrder(Guid portfolioId, string symbol, decimal quantity)
    {
        mediator.Send(new Mark)
        var currentPriceResponse =  brokerDataService.GetCurrentPriceAsync(symbol);
        var tradeValue = quantity * currentPriceResponse.Result.Price;
        portfolioService.CheckAndReserveCashAmountAsync(portfolioId, tradeValue);
        // create trade in database
        // deduct cash

        // consider how trades should impact holdings and portfolio
    }
}