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

    public async Task<CurrentPriceResponse> GetCurrentPriceAsync(string symbol)
    {
        var parameters = new Dictionary<string, string> { { "symbol", symbol } };
        return await GetAsync<CurrentPriceResponse>(_binanceApiSettings.CurrentPriceUrl, null ,parameters);
    }

    public async Task<HistoricalPriceResponse> GetHistoricalPriceAsync(HistoricalPriceParams historicalPriceParams)
    {
        var rawResult = await GetAsync<List<object[]>>(_binanceApiSettings.HistoricalPriceUrl, null ,historicalPriceParams.ToDictionary());
        return HistoricalPriceMapper.ConvertRawResponse(rawResult);
    }
}







