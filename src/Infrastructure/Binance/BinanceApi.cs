using Data.Models;
using Microsoft.Extensions.Options;

namespace Infrastructure.Binance;

public class BinanceApi: BaseHttpClient, IBinanceApi
{
    private readonly string _currentPriceUrl;
    private readonly string _historicalPriceUrl;

    public BinanceApi(HttpClient httpClient, IOptions<BinanceApiSettings> binanceApiSettings) : base(httpClient)
    {
        _currentPriceUrl = binanceApiSettings.Value.CurrentPriceUrl;
        _historicalPriceUrl = binanceApiSettings.Value.HistoricalPriceUrl;
    }

    public Task<PriceResponse> GetCurrentPriceAsync(string symbol)
    {
        var parameters = new Dictionary<string, string> { { "symbol", symbol } };
        return GetAsync<PriceResponse>(_currentPriceUrl, null ,parameters);
    }

    public Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams historicalPriceParams)
    {
        return GetAsync<HistoricalPriceResponse>(_historicalPriceUrl, null ,historicalPriceParams.ToDictionary());
    }
}