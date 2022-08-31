using System.Text.Json.Serialization;
using CandlePowered.Helpers;

namespace CandlePowered.Dtos;

public class WebsocketDto
{
    [JsonPropertyName("s")]
    public string StockTicker { get; set; }
    
    [JsonPropertyName("p")]
    [JsonConverter(typeof(StringToDecimalConverter))]
    public decimal Price { get; set; }
    
    [JsonPropertyName("t")]
    [JsonConverter(typeof(UnixEpochDateTimeConverter))]
    public DateTime Time { get; set; }
}