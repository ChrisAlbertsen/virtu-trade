using System.Net.Http.Json;
using Frontend.Api.Models;

namespace Frontend.Api.Clients;

public class BrokerApiClient(HttpClient http)
{
    public async Task<decimal> GetCurrentPriceAsync(string symbol)
    {
        var resp = await http.GetAsync($"api/Broker/prices/current?symbol={Uri.EscapeDataString(symbol)}");
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<decimal>()!;
    }

    public async Task<IEnumerable<object>?> GetHistoricalPricesAsync(string symbol, string interval, long startTime, long endTime, string timeZone, int limit)
    {
        var url = $"api/Broker/prices/historical?Symbol={symbol}&Interval={interval}&StartTime={startTime}&EndTime={endTime}&TimeZone={timeZone}&Limit={limit}";
        var resp = await http.GetAsync(url);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<IEnumerable<object>>()!;
    }

    public async Task ExecuteMarketOrderAsync(MarketOrderParams req)
    {
        var resp = await http.PostAsJsonAsync("api/Broker/orders/execute-market-order", req);
        resp.EnsureSuccessStatusCode();
    }
}