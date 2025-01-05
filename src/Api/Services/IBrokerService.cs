
using Api.Services.Models;

public interface IBrokerService {
    Task<BinancePriceResponse> GetCurrentPriceAsync(string symbol);
    Task PlaceOrderAsync(string symbol, decimal quantity, string orderType);
}