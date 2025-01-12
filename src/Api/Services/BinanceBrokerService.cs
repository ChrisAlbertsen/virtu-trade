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

    public async Task<PriceResponse> GetCurrentPriceAsync(string symbol)
    {
        var priceResponse = await _binanceApi.GetCurrentPriceAsync(symbol);
        return priceResponse;
    }

    public async Task PlaceOrderAsync(string symbol, decimal quantity, string orderType) {
        throw new NotImplementedException();
    }
}

