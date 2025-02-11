using System;
using System.Threading.Tasks;

namespace Service.Interfaces;

public interface IPortfolioService
{
    Task<bool> HasSufficientCashAsync(Guid portfolioId, decimal neededCash);
    Task CheckAndReserveCashAmountAsync(Guid portfolioId, decimal cashToReserve);
}