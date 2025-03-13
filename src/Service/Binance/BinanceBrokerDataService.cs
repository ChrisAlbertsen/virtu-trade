using System.Threading.Tasks;
using Data.DTOs.CurrentPrice;
using Data.DTOs.HistoricalPrice;
using Infrastructure.Binance;
using Service.Interfaces;

namespace Service.Binance;

public class BinanceBrokerDataService(IBinanceApi binanceApi) : IBrokerDataService
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