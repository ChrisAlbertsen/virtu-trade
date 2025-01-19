using System.Text.Json;
using System.Net.Http;
using Infrastructure.Binance;
using Data.Models;

namespace Models.Services;
public class BinanceBrokerService : IBrokerService {
    private readonly IBinanceApi _binanceApi;

    public BinanceBrokerService (IBinanceApi binanceApi)
    {
        _binanceApi = binanceApi;
    }

    public async Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol)
    {
        var priceResponse = await _binanceApi.GetCurrentPriceAsync(symbol);
        return priceResponse;
    }

    public async Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams historicalPriceParams)
    {
        var historicalPriceResponse = await _binanceApi.GetHistoricalPriceAsync(historicalPriceParams);
        return historicalPriceResponse;
    }

    public async Task PlaceOrderAsync(string symbol, decimal quantity, string orderType) {
        throw new NotImplementedException();
    }
}

