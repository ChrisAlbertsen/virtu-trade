using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Service.Paper;

public class PortfolioRepository(ApplicationDatabaseContext dbContext) : IPortfolioRepository
{
    public async Task<Portfolio> CreatePortfolioAsync()
    {
        var portfolio = new Portfolio
        {
            Id = Guid.NewGuid(),
            Cash = 0,
            ReservedCash = 0,
            Holdings = new List<Holding>()
        };

        dbContext.Add(portfolio);
        await dbContext.SaveChangesAsync();
        return portfolio;
    }

    public async Task<Portfolio?> GetPortfolioAsync(Guid portfolioId)
    {
        return await dbContext
            .Portfolios
            .FirstOrDefaultAsync(p => p.Id == portfolioId);
    }
}