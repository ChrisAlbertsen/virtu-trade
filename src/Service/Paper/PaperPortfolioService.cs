using System;
using System.Threading.Tasks;
using Data.Entities;
using Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Service.Interfaces;

namespace Service.Paper;

public class PaperPortfolioService(ApplicationDatabaseContext dbContext) : IPortfolioService
{
    public Task<Portfolio?> GetPortfolioAsync(Guid portfolioId)
    {
        return dbContext
            .Portfolios
            .FirstOrDefaultAsync(p => p.Id == portfolioId);
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