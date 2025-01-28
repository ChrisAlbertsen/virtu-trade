namespace Data.Models;

public class CurrentPriceResponse
{
    public required string Symbol { get; set; }
    public required decimal Price { get; set; }
}