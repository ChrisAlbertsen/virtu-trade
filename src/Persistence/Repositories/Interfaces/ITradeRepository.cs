using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Service.Paper;

public interface ITradeRepository
{
    void CreateTrade(Trade trade, decimal price);
    Task<List<Trade>> GetTrades(Guid portfolioId);
}