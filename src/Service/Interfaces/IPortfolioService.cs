using System;
using System.Threading.Tasks;
using Data.Entities;

namespace Service.Interfaces;

public interface IPortfolioService
{
    Task<Portfolio?> GetPortfolioAsync(Guid portfolioId);
    Task<bool> HasSufficientCashAsync(Guid portfolioId, decimal neededCash);
    Task CheckAndReserveCashAmountAsync(Guid portfolioId, decimal cashToReserve);
}