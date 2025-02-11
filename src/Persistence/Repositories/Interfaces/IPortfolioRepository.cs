using System;
using System.Threading.Tasks;
using Data.Entities;

namespace Service.Paper;

public interface IPortfolioRepository
{
    Task<Portfolio> CreatePortfolioAsync();
    Task<Portfolio?> GetPortfolioAsync(Guid portfolioId);
}