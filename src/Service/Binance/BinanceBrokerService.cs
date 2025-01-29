using System.Threading.Tasks;
using Data.Models;
using Infrastructure.Binance;

namespace Service.Binance;

public class BinanceBrokerService(IBinanceApi binanceApi) : IBrokerService
{
    public async Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol)
    {
        return await binanceApi.GetCurrentPriceAsync(symbol);
    }

    public async Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams historicalPriceParams)
    {
        return await binanceApi.GetHistoricalPriceAsync(historicalPriceParams);
    }
}