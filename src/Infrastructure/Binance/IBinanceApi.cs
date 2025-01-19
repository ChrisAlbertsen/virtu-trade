using Data.Models;

namespace Infrastructure.Binance;

public interface IBinanceApi
{
    Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol);
    
    Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams queryParams);
}