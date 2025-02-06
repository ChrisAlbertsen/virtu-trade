using System.Threading.Tasks;
using Data.Models;

namespace Service.Interfaces;

public interface IBrokerDataService
{
    Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol);
    Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams historicalPriceParams);
}