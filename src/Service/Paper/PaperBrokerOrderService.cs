using System;
using System.Threading.Tasks;
using Data.DTOs;
using MediatR;
using Persistence;
using Service.Interfaces;
using Service.Paper.Mediator.Operations;

namespace Service.Paper;

public class PaperBrokerOrderService(ApplicationDatabaseContext dbContext) : IBrokerOrderService
{
    public Task<OrderFulfillmentResponse> MarketOrder(Guid portfolioId, string symbol, decimal quantity)
    {
        
    }
    
    public async Task ProcessTrade(BaseOrderCommand command, decimal price)
    {
        var holdings = await GetHoldingsAsync(command.PortfolioId);
        var holding = FindHoldingWithSymbol(holdings, command.Symbol);

        if (holding is null)
        {
            CreateHolding(command, price);
            return;
        }

        holding.AveragePurchasePrice = CalculateNewAveragePrice(holding, command, price);
        holding.Quantity += command.Quantity;

        if (holding.Quantity == 0)
        {
            dbContext.Holdings.Remove(holding);
            return;
        }

        dbContext.Holdings.Update(holding);
    }

    private static decimal CalculateNewAveragePrice(Holding holding, BaseOrderCommand command, decimal price)
    {
        return (holding.Quantity * holding.AveragePurchasePrice + command.Quantity * price) /
               (holding.Quantity + command.Quantity);
    }

    private static Holding? FindHoldingWithSymbol(List<Holding> holdings, string symbol)
    {
        return holdings.Find(h => h.Symbol == symbol) ?? null;
    }
    
    public async Task<bool> HasSufficientCashAsync(Guid portfolioId, decimal neededCash)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        return portfolio != null && portfolio.HasSufficientCash(neededCash);
    }

    public async Task CheckAndReserveCashAmountAsync(Guid portfolioId, decimal cashToReserve)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        if (portfolio is null) throw new PortfolioNotFoundException(portfolioId);
        if (!portfolio.HasSufficientCash(cashToReserve)) throw new PortfolioLacksCash(portfolioId);

        portfolio.Cash -= cashToReserve;
        portfolio.ReservedCash += cashToReserve;
        await dbContext.SaveChangesAsync();
    }
}