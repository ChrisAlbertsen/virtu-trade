using System.Threading;
using System.Threading.Tasks;
using Data.DTOs;
using MediatR;
using Service.Interfaces;

namespace Service.Paper.Mediator.Commands;

public class MarketOrderHandler(IBrokerDataService brokerDataService, IPortfolioService portfolioService) : IRequestHandler<MarketOrderCommand, OrderFulfillmentResponse>
{
    public async Task<OrderFulfillmentResponse> Handle(MarketOrderCommand request, CancellationToken cancellationToken)
    {
        var currentPriceResponse = await brokerDataService.GetCurrentPriceAsync(request.Symbol);
        var tradeValue = request.Quantity * currentPriceResponse.Price;

        await portfolioService.CheckAndReserveCashAmountAsync(request.PortfolioId, tradeValue);
        
        // TODO: Add logic to create a trade, deduct cash, and update holdings

        return 
    }
}

