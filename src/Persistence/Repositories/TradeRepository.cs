using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Service.Paper;

public class TradeRepository(ApplicationDatabaseContext dbContext) : ITradeRepository
{
    public void CreateTrade(Trade trade, decimal price)
    {
        dbContext.Trades.Add(trade);
    }

    public Task<List<Trade>> GetTrades(Guid portfolioId)
    {
        return dbContext
            .Trades
            .Where(t => t.PortfolioId == portfolioId)
            .ToListAsync();
    }
}