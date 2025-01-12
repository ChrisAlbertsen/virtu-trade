
using Data.Models;

public interface IBrokerService {
    Task<PriceResponse> GetCurrentPriceAsync(string symbol);
    Task PlaceOrderAsync(string symbol, decimal quantity, string orderType);
}