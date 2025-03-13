using System.Threading.Tasks;
using Data.DTOs.CurrentPrice;
using Data.DTOs.HistoricalPrice;

namespace Service.Binance;

public interface IBrokerService
{
    Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol);
    Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams historicalPriceParams);
}