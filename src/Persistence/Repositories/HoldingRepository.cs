using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Service.Paper;

public class HoldingRepository(ApplicationDatabaseContext dbContext) : IHoldingRepository
{
    public async Task<List<Holding>> GetHoldingsAsync(Guid portfolioId)
    {
        return await dbContext
            .Holdings
            .Where(h => h.PortfolioId == portfolioId)
            .ToListAsync();
    }

    public void CreateHolding(Holding holding)
    {
        dbContext.Holdings.Add(
            holding);
    }
}