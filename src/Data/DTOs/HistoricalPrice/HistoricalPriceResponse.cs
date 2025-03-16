using System.Collections.Generic;

namespace Data.DTOs.HistoricalPrice;

public class HistoricalPriceResponse
{
    public List<HistoricalPrice> HistoricalPrices { get; set; }
}