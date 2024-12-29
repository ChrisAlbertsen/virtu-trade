
public interface IBrokerService {
    Task<decimal> GetCurrentPriceAsync(string symbol);
    Task PlaceOrderAsync(string symbol, decimal quantity, string orderType);
}