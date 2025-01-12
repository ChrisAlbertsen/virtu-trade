using Data.Models;
using Microsoft.Extensions.Options;

namespace Infrastructure.Binance;

public class BinanceApi: BaseHttpClient, IBinanceApi
{
    private readonly string _basePriceUrl;

    public BinanceApi(HttpClient httpClient, IOptions<BinanceApiSettings> binanceApiSettings) : base(httpClient)
    {
        _basePriceUrl = binanceApiSettings.Value.BasePriceUrl;
    }

    public Task<PriceResponse> GetCurrentPriceAsync(string symbol)
    {
        var parameters = new Dictionary<string, string> { { "symbol", symbol } };
        return GetAsync<PriceResponse>(_basePriceUrl, null ,parameters);
    }

    public Task<HistoricalPriceResponse> GetHistoricalPriceAsync(Dictionary<string,string> queryParams)
    {
        return GetAsync<HistoricalPriceResponse>(_basePriceUrl, null ,queryParams);
    }
}