using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;
using Service.Paper.Mediator.Operations;

namespace Service.Paper;

public interface IPaperTradeService
{
    void CreateTrade(BaseOrderCommand command, decimal price);
    Task<List<Trade>> GetTrades(Guid portfolioId);
}