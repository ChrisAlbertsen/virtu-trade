using System;
using System.Threading.Tasks;

namespace Service.Interfaces;

public interface IPaperTradeCatchService
{
    Task CatchTrade(Guid portfolioId, string symbol, decimal quantity, decimal price);
}