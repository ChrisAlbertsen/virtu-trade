using System.Text.Json.Serialization;

namespace Api.Services.Models;
public class BinancePriceResponse
{
    [JsonPropertyName("price")]
    [JsonConverter(typeof(StringToDecimalConverter))]
    public required decimal Price { get; set; }
    [JsonPropertyName("symbol")]
    public required string Symbol {get; set; }
}