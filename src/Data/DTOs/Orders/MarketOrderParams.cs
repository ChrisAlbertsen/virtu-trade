using System;

namespace Data.DTOs.Orders;

public class MarketOrderParams
{
    public required Guid PortfolioId { get; set; }
    public required string Symbol { get; set; }
    public required decimal Quantity { get; set; }
}