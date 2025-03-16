using System;

namespace Data.DTOs.BaseModels;

public abstract class BaseOrder
{
    public required Guid Id { get; set; }
    public required Guid PortfolioId { get; set; }
    public required string Symbol { get; set; }
    public required decimal Quantity { get; set; }
    public required decimal Price { get; set; }
    public decimal OrderValue => Quantity * Price;
}