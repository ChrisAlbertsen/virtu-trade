using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.DTOs;
using Data.Entities;
using Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Service.Interfaces;

namespace Service.Paper;

public class PaperBrokerOrderService(ApplicationDatabaseContext dbContext, IBrokerDataService brokerDataService)
    : IBrokerOrderService, IPaperBrokerOrderService
{
    public async Task<OrderFulfillmentResponse> MarketOrder(Guid portfolioId, string symbol, decimal quantity)
    {
        var currentPriceResponse = await brokerDataService.GetCurrentPriceAsync(symbol);
        var orderValue = currentPriceResponse.Price * quantity;
        await CheckAndReserveCashAmountAsync(portfolioId, orderValue);

        try
        {
            await CatchTrade(portfolioId, symbol, quantity, currentPriceResponse.Price);
        }
        catch (Exception)
        {
            await UnreserveCash(portfolioId, orderValue);
            throw;
        }

        await PayReservedCash(portfolioId, orderValue);

        return new OrderFulfillmentResponse
        {
            Id = Guid.NewGuid(),
            Symbol = symbol,
            Price = currentPriceResponse.Price,
            Quantity = quantity
        };
    }

    public async Task<Portfolio> CreatePortfolio()
    {
        var portfolio = new Portfolio
        {
            Id = Guid.NewGuid(),
            Cash = 0,
            ReservedCash = 0,
            Holdings = new List<Holding>()
        };

        dbContext.Portfolios.Add(portfolio);
        await dbContext.SaveChangesAsync();
        return portfolio;
    }

    public async Task DepositMoneyToPortfolio(Guid portfolioId, decimal moneyToDeposit)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        if (portfolio is null) throw new PortfolioNotFoundException(portfolioId);
        portfolio.Cash += moneyToDeposit;
        await dbContext.SaveChangesAsync();
    }

    private async Task CatchTrade(Guid portfolioId, string symbol, decimal quantity, decimal price)
    {
        var holdings = await GetHoldingsAsync(portfolioId);
        var holding = FindHoldingWithSymbol(holdings, symbol);
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

    public async Task<List<Holding>> GetHoldingsAsync(Guid portfolioId)
    {
        return await dbContext
            .Holdings
            .Where(h => h.PortfolioId == portfolioId)
            .ToListAsync();
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
                TradeDateTime = DateTime.UtcNow,
            });
    }

    private static decimal CalculateNewAveragePrice(Holding holding, decimal quantity, decimal price)
    {
        return (holding.Quantity * holding.AveragePurchasePrice + quantity * price) /
               (holding.Quantity + quantity);
    }

    private static Holding? FindHoldingWithSymbol(List<Holding> holdings, string symbol)
    {
        return holdings.Find(h => h.Symbol == symbol) ?? null;
    }

    public async Task<Portfolio?> GetPortfolioAsync(Guid portfolioId)
    {
        return await dbContext
            .Portfolios
            .FirstOrDefaultAsync(p => p.Id == portfolioId);
    }

    private async Task CheckAndReserveCashAmountAsync(Guid portfolioId, decimal cashToReserve)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        if (portfolio is null) throw new PortfolioNotFoundException(portfolioId);
        if (!portfolio.HasSufficientCash(cashToReserve)) throw new PortfolioLacksCash(portfolioId);

        portfolio.Cash -= cashToReserve;
        portfolio.ReservedCash += cashToReserve;
        await dbContext.SaveChangesAsync();
    }

    private async Task PayReservedCash(Guid portfolioId, decimal cashToPay)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        if (portfolio is null) throw new PortfolioNotFoundException(portfolioId);
        portfolio.ReservedCash -= cashToPay;
        await dbContext.SaveChangesAsync();
    }

    private async Task UnreserveCash(Guid portfolioId, decimal cashToUnreserve)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        if (portfolio is null) throw new PortfolioNotFoundException(portfolioId);
        portfolio.ReservedCash -= cashToUnreserve;
        portfolio.Cash += cashToUnreserve;
        await dbContext.SaveChangesAsync();
    }
}