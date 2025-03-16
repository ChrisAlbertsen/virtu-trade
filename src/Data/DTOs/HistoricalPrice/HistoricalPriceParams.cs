using Data.DTOs.BaseModels;

namespace Data.DTOs.HistoricalPrice;

public class HistoricalPriceParams : BaseQueryParamModel
{
    public required string Symbol { get; set; }
    public required string Interval { get; set; }
    public long? StartTime { get; set; }
    public long? EndTime { get; set; }
    public string? TimeZone { get; set; }
    public int? Limit { get; set; }
}