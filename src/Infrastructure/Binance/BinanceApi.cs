using Data.Models;
using Microsoft.Extensions.Options;

namespace Infrastructure.Binance;

public class BinanceApi: BaseHttpClient, IBinanceApi
{
    private readonly BinanceApiSettings _binanceApiSettings;

    public BinanceApi(HttpClient httpClient, IOptions<BinanceApiSettings> binanceApiSettings) : base(httpClient)
    {
        _binanceApiSettings = binanceApiSettings.Value;
    }

    public Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol)
    {
        var parameters = new Dictionary<string, string> { { "symbol", symbol } };
        return GetAsync<CurrentPriceResponse>(_binanceApiSettings.CurrentPriceUrl, null ,parameters);
    }

    public Task<HistoricalPriceResponse[]> GetHistoricalPriceAsync(HistoricalPriceParams historicalPriceParams)
    {
        return GetAsync<HistoricalPriceResponse[]>(_binanceApiSettings.HistoricalPriceUrl, null ,historicalPriceParams.ToDictionary());
    }
}







