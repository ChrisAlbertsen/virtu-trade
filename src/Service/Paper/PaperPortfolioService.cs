using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Service.Exceptions.PortfolioExceptions;
using Service.Interfaces;

namespace Service.Paper;

public class PaperPortfolioService(ApplicationDatabaseContext dbContext) : IPaperPortfolioService
{
    public async Task<List<Holding>> GetHoldingsAsync(Guid portfolioId)
    {
        return await dbContext
            .Holdings
            .Where(h => h.PortfolioId == portfolioId)
            .ToListAsync();
    }

    public async Task<Holding?> FindHoldingWithSymbol(Guid portfolioId, string symbol)
    {
        return await dbContext
            .Holdings
            .FirstOrDefaultAsync(h => h.Symbol == symbol && h.PortfolioId == portfolioId);
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
        await dbContext.EnsuredSaveChangesAsync();
        return portfolio;
    }

    public async Task DepositMoneyToPortfolio(Guid portfolioId, decimal moneyToDeposit)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        if (portfolio is null) throw new PortfolioNotFoundException(portfolioId);
        portfolio.Cash += moneyToDeposit;
        await dbContext.EnsuredSaveChangesAsync();
    }

    public async Task CheckAndReserveCashAmountAsync(Guid portfolioId, decimal cashToReserve)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        if (portfolio is null) throw new PortfolioNotFoundException(portfolioId);
        if (!PortfolioHasSufficientCash(portfolio, cashToReserve)) throw new PortfolioLacksCashException(portfolioId);

        portfolio.Cash -= cashToReserve;
        portfolio.ReservedCash += cashToReserve;
        await dbContext.EnsuredSaveChangesAsync();
    }

    public async Task PayReservedCash(Guid portfolioId, decimal cashToPay)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        if (portfolio is null) throw new PortfolioNotFoundException(portfolioId);
        portfolio.ReservedCash -= cashToPay;
        await dbContext.EnsuredSaveChangesAsync();
    }

    public async Task UnreserveCash(Guid portfolioId, decimal cashToUnreserve)
    {
        var portfolio = await GetPortfolioAsync(portfolioId);
        if (portfolio is null) throw new PortfolioNotFoundException(portfolioId);
        portfolio.ReservedCash -= cashToUnreserve;
        portfolio.Cash += cashToUnreserve;
        await dbContext.EnsuredSaveChangesAsync(1);
    }

    public async Task<Portfolio?> GetPortfolioAsync(Guid portfolioId)
    {
        return await dbContext
            .Portfolios
            .FirstOrDefaultAsync(p => p.Id == portfolioId);
    }

    public async Task<Portfolio?> GetPortfolioWithHoldingsAsync(Guid portfolioId)
    {
        return await dbContext
            .Portfolios
            .Include(h => h.Holdings)
            .FirstOrDefaultAsync(p => p.Id == portfolioId);
    }

    private bool PortfolioHasSufficientCash(Portfolio portfolio, decimal neededCash)
    {
        return portfolio.Cash >= neededCash;
    }
}