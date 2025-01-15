using Data.Models;

namespace Infrastructure.Binance;

public interface IBinanceApi
{
    Task<PriceResponse> GetCurrentPriceAsync(string symbol);
    
    Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams queryParams);
}