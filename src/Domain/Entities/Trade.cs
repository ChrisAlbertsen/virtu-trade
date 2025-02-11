using System;

namespace Data.Entities;

public class Trade
{
    public required Guid Id { get; set; }
    public required Guid PortfolioId { get; set; }
    public required DateTime TradeDateTime { get; set; }
    public required decimal Price { get; set; }
    public required decimal Quantity { get; set; }
    public required string Symbol { get; set; }
}