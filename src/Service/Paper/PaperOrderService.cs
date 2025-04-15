using System;
using System.Threading.Tasks;
using Data.DTOs.BaseModels;
using Data.DTOs.Orders;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Paper;

public class PaperOrderService(
    IPortfolioService portfolioService,
    ITradeCatchService tradeCatchService,
    IBrokerDataService brokerDataService,
    ILogger<PaperOrderService> logger)
    : IBrokerOrderService
{
    public async Task<OrderFulfillmentResponse> ExecuteMarketOrder(MarketOrderParams marketOrderParams)
    {
        var marketOrder = await CreateMarketOrder(marketOrderParams);
        return await ResolveOrder(marketOrder);
    }

    private async Task<MarketOrder> CreateMarketOrder(MarketOrderParams marketOrderParams)
    {
        var currentPriceResponse = await brokerDataService.GetCurrentPriceAsync(marketOrderParams.Symbol);

        return new MarketOrder(
            marketOrderParams.PortfolioId,
            marketOrderParams.Symbol,
            currentPriceResponse.Price,
            marketOrderParams.Quantity
        );
    }

    private async Task<OrderFulfillmentResponse> ResolveOrder(BaseOrder order)
    {
        await portfolioService.CheckAndReserveCashAmountAsync(order.PortfolioId, order.Price);

        try
        {
            await tradeCatchService.CatchTrade(order);
        }
        catch (Exception e)
        {
            logger.LogError(
                "An exception occured during catching a market order trade for {portfolioId} on {symbol} with {ExceptionMessage}",
                order.PortfolioId,
                order.Symbol,
                e.Message);
            await portfolioService.UnreserveCash(order.PortfolioId, order.OrderValue);
            throw;
        }

        await portfolioService.PayReservedCash(order.PortfolioId, order.OrderValue);

        return new OrderFulfillmentResponse
        {
            Id = Guid.NewGuid(),
            Symbol = order.Symbol,
            Price = order.Price,
            Quantity = order.Quantity
        };
    }
}