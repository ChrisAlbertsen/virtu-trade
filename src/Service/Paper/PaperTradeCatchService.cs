using System;
using System.Threading.Tasks;
using Data.Entities;
using Persistence;
using Service.Interfaces;

namespace Service.Paper;

public class PaperTradeCatchService(IPaperPortfolioService portfolioService, ApplicationDatabaseContext dbContext)
    : IPaperTradeCatchService
{
    public async Task CatchTrade(Guid portfolioId, string symbol, decimal quantity, decimal price)
    {
        var holdings = await portfolioService.GetHoldingsAsync(portfolioId);
        var holding = portfolioService.FindHoldingWithSymbol(holdings, symbol);
        AddTrade(portfolioId, symbol, quantity, price);

        if (holding is null)
        {
            dbContext.Holdings.Add(new Holding
            {
                Id = Guid.NewGuid(),
                PortfolioId = portfolioId,
                Symbol = symbol,
                Quantity = quantity,
                AveragePurchasePrice = price
            });
            await dbContext.SaveChangesAsync();
            return;
        }

        holding.Quantity += quantity;

        if (holding.Quantity == 0)
        {
            dbContext.Holdings.Remove(holding);
            await dbContext.SaveChangesAsync();
            return;
        }

        holding.AveragePurchasePrice = CalculateNewAveragePrice(holding, quantity, price);

        dbContext.Holdings.Update(holding);
        await dbContext.SaveChangesAsync();
    }

    private void AddTrade(Guid portfolioId, string symbol, decimal quantity, decimal price)
    {
        dbContext.Trades.Add(
            new Trade
            {
                Id = Guid.NewGuid(),
                PortfolioId = portfolioId,
                Symbol = symbol,
                Quantity = quantity,
                Price = price,
                TradeDateTime = DateTime.UtcNow
            });
    }

    private static decimal CalculateNewAveragePrice(Holding holding, decimal quantity, decimal price)
    {
        return (holding.Quantity * holding.AveragePurchasePrice + quantity * price) /
               (holding.Quantity + quantity);
    }
}