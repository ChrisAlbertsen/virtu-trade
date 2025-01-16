namespace Data.Models;

public class HistoricalPriceResponse
{
    public required DateTime OpenTime { get; set; }
    public required decimal OpenPrice { get; set; }
    public required decimal HighPrice { get; set; }
    public required decimal LowPrice { get; set; }
    public required decimal ClosePrice { get; set; }
    public required decimal Volume { get; set; }
    public required DateTime CloseTime { get; set; }
    public required decimal QuoteVolume { get; set; }
    public required uint NumberOfTrades { get; set; }
    public required decimal TakerBuyBaseAssetVolume { get; set; }
    public required decimal TakerBuyQuoteAssetVolume { get; set; }
    public required decimal UnusedField { get; set; }
}