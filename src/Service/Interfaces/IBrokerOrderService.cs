using System.Threading.Tasks;
using Data.Models;

namespace Service.Interfaces;

public interface IBrokerOrderService
{
    Task<OrderFulfillmentResponse> MarketOrder(string symbol, decimal quantity);
}