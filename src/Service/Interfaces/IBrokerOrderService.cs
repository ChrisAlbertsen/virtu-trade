using System;
using System.Threading.Tasks;
using Data.DTOs;

namespace Service.Interfaces;

public interface IBrokerOrderService
{
    Task<OrderFulfillmentResponse> MarketOrder(Guid portfolioId, string symbol, decimal quantity);
}