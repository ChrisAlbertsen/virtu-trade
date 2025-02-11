using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;
using Service.Paper.Mediator.Operations;

namespace Service.Paper;

public interface IPaperHoldingService
{
    Task ProcessTrade(BaseOrderCommand command, decimal price);
    Task<List<Holding>> GetHoldingsAsync(Guid portfolioId);
}