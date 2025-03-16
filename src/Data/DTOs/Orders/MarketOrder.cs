using System;
using Data.DTOs.BaseModels;
using Data.DTOs.Interfaces;

namespace Data.DTOs.Orders;

public class MarketOrder(decimal price, Guid portfolioId, string symbol, decimal quantity)
    : IOrder
{
    public Guid PortfolioId { get; } = portfolioId;
    public string Symbol { get; } = symbol;
    public decimal Quantity { get; } = quantity;
    public decimal Price { get; } = price;
    public decimal OrderValue => Quantity * Price;
}