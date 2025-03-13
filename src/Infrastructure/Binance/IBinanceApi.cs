using System.Threading.Tasks;
using Data.DTOs.CurrentPrice;
using Data.DTOs.HistoricalPrice;

namespace Infrastructure.Binance;

public interface IBinanceApi
{
    Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol);

    Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams queryParams);
}