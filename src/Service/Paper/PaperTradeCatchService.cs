﻿using System;
using System.Threading.Tasks;
using Data.DTOs.BaseModels;
using Data.Entities;
using Persistence;
using Service.Interfaces;

namespace Service.Paper;

public class PaperTradeCatchService(IPortfolioService portfolioService, AppDbContext dbContext)
    : ITradeCatchService
{
    public async Task CatchTrade(BaseOrder order)
    {
        var holding = await portfolioService.FindHoldingWithSymbol(order.PortfolioId, order.Symbol);
        AddTrade(order);

        if (holding is null)
        {
            AddHolding(order);
            await dbContext.EnsuredSaveChangesAsync();
            return;
        }

        holding.Quantity += order.Quantity;
        if (holding.Quantity == 0)
        {
            dbContext.Holdings.Remove(holding);
            await dbContext.EnsuredSaveChangesAsync();
            return;
        }

        holding.AveragePurchasePrice = CalculateNewAveragePrice(holding, order.Quantity, order.Price);

        dbContext.Holdings.Update(holding);
        await dbContext.EnsuredSaveChangesAsync();
    }

    private void AddHolding(BaseOrder order)
    {
        dbContext.Holdings.Add(new Holding
        {
            Id = Guid.NewGuid(),
            PortfolioId = order.PortfolioId,
            Symbol = order.Symbol,
            Quantity = order.Quantity,
            AveragePurchasePrice = order.Price
        });
    }

    private void AddTrade(BaseOrder order)
    {
        dbContext.Trades.Add(
            new Trade
            {
                Id = Guid.NewGuid(),
                PortfolioId = order.PortfolioId,
                Symbol = order.Symbol,
                Quantity = order.Quantity,
                Price = order.Price,
                TradeDateTime = DateTime.UtcNow
            });
    }

    private static decimal CalculateNewAveragePrice(Holding holding, decimal quantity, decimal price)
    {
        return (holding.Quantity * holding.AveragePurchasePrice + quantity * price) /
               (holding.Quantity + quantity);
    }
}