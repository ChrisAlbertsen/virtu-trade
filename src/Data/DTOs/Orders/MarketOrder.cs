using System;
using Data.DTOs.BaseModels;

namespace Data.DTOs.Orders;

public class MarketOrder(Guid portfolioId, string symbol, decimal quantity, decimal price)
    : BaseOrder(portfolioId, symbol, quantity, price)
{
}