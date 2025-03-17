using System.Threading.Tasks;
using Data.DTOs.Orders;

namespace Service.Interfaces;

public interface IBrokerOrderService
{
    Task<OrderFulfillmentResponse> ExecuteMarketOrder(MarketOrderParams marketOrderParams);
}