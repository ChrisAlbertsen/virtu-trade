using Api.Services.Models;

namespace Infrastructure.Binance;

public interface IBinanceApi
{
    Task<BinancePriceResponse> GetCurrentPriceAsync(string symbol);
    
}