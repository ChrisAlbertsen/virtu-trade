using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Service.Paper;

public interface IHoldingRepository
{
    Task<List<Holding>> GetHoldingsAsync(Guid portfolioId);
    void CreateHolding(Holding holding);
}