using Api.Services.Models;

namespace Infrastructure.Binance;

public class BinanceApi: BaseHttpClient, IBinanceApi
{
    private const string BasePriceUrl = "https://api.binance.com/api/v3/ticker/price";

    public BinanceApi(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<BinancePriceResponse> GetCurrentPriceAsync(string symbol)
    {
        var parameters = new Dictionary<string, string> { { "symbol", symbol } };
        return GetAsync<BinancePriceResponse>(BasePriceUrl, null ,parameters);
    }
}