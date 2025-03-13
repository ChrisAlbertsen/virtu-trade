using System;

namespace Data.DTOs.Interfaces;

public interface IOrder
{
        Guid PortfolioId { get; }
        string Symbol { get; }
        decimal Quantity { get; }
        decimal Price { get; }
        
        decimal OrderValue { get; }
}