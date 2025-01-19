﻿namespace Data.Models
{
    public class HistoricalPrice
    {
        public required long OpenTime { get; set; }
        public required decimal OpenPrice { get; set; }
        public required decimal HighPrice { get; set; }
        public required decimal LowPrice { get; set; }
        public required decimal ClosePrice { get; set; }
        public required decimal Volume { get; set; }
        public required long CloseTime { get; set; }
        public required decimal QuoteVolume { get; set; }
        public required uint NumberOfTrades { get; set; }
        public required decimal TakerBuyBaseAssetVolume { get; set; }
        public required decimal TakerBuyQuoteAssetVolume { get; set; }
        public required decimal UnusedField { get; set; }
    }

    public class HistoricalPriceResponse
    {
        public List<HistoricalPrice> HistoricalPrices { get; set; }
    }
    public static class HistoricalPriceMapper
    {
        public static HistoricalPriceResponse ConvertRawResponse(List<object[]> rawPrices)
        {
            var historicalPriceResponse = new HistoricalPriceResponse(){HistoricalPrices = new List<HistoricalPrice>()};

            foreach (var item in rawPrices)
            {
                
                var price = new HistoricalPrice
                {
                    OpenTime = Convert.ToInt64(item[0]),
                    OpenPrice = Convert.ToDecimal(item[1]),
                    HighPrice = Convert.ToDecimal(item[2]),
                    LowPrice = Convert.ToDecimal(item[3]),
                    ClosePrice = Convert.ToDecimal(item[4]),
                    Volume = Convert.ToDecimal(item[5]),
                    CloseTime = Convert.ToInt64(item[6]),
                    QuoteVolume = Convert.ToDecimal(item[7]),
                    NumberOfTrades = Convert.ToUInt32(item[8]),
                    TakerBuyBaseAssetVolume = Convert.ToDecimal(item[9]),
                    TakerBuyQuoteAssetVolume = Convert.ToDecimal(item[10]),
                    UnusedField = Convert.ToDecimal(item[11])
                };
                historicalPriceResponse.HistoricalPrices.Add(price);
            }

            return historicalPriceResponse;
        }
    }
}