namespace Data.DTOs.CurrentPrice;

public class CurrentPriceResponse
{
    public required string Symbol { get; set; }
    public required decimal Price { get; set; }
}