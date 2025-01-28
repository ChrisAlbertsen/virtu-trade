using System.Threading.Tasks;
using Data.Models;

namespace Service.Binance;

public interface IBrokerService
{
    Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol);
    Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams historicalPriceParams);
}