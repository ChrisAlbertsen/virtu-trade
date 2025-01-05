
namespace Api.Services.Models;
public class BinancePriceResponse
{
    public required string Symbol {get; set; }
    public required decimal Price { get; set; }
}