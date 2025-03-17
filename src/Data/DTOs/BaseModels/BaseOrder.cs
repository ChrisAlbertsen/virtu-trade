using System;

namespace Data.DTOs.BaseModels;

public abstract class BaseOrder(Guid portfolioId, string symbol, decimal quantity, decimal price)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid PortfolioId { get; init; } = portfolioId;
    public string Symbol { get; init; } = symbol;
    public decimal Quantity { get; init; } = quantity;
    public decimal Price { get; init; } = price;

    public decimal OrderValue => Quantity * Price;
}