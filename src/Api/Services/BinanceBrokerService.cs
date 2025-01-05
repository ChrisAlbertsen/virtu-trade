using System.Text.Json;
using System.Net.Http;
using Api.Services.Models;
using Infrastructure.Binance;

namespace Api.Services;
public class BinanceBrokerService : IBrokerService {
    private readonly IBinanceApi _binanceApi;

    public BinanceBrokerService (IBinanceApi binanceApi)
    {
        _binanceApi = binanceApi;
    }

    public async Task<BinancePriceResponse> GetCurrentPriceAsync(string symbol)
    {
        var priceResponse = await _binanceApi.GetCurrentPriceAsync(symbol);
        return priceResponse;
    }

    public async Task PlaceOrderAsync(string symbol, decimal quantity, string orderType) {
        throw new NotImplementedException();
    }
}

