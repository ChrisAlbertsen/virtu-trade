using System.Text.Json;
using System.Net.Http;
using Api.Services.Models;

namespace Api.Services;
public class BinanceBrokerService : IBrokerService {
    private readonly HttpClient _httpClient;

    public BinanceBrokerService (HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetCurrentPriceAsync(string symbol) {
        HttpResponseMessage response = await _httpClient.GetAsync($"https://api.binance.com/api/v3/ticker/price?symbol={symbol}");
        response.EnsureSuccessStatusCode();
        
        string json = await response.Content.ReadAsStringAsync();

        BinancePriceResponse? data = JsonSerializer.Deserialize<BinancePriceResponse>(json);
        return data.Price;
    }

    public async Task PlaceOrderAsync(string symbol, decimal quantity, string orderType) {
        throw new NotImplementedException();
    }
}

