
using Data.Models;

public interface IBrokerService {
    Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol);
    Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams historicalPriceParams);
    Task PlaceOrderAsync(string symbol, decimal quantity, string orderType);
}