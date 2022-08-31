using System.ComponentModel.DataAnnotations.Schema;

namespace CandlePowered.Entities;

public class CandleData
{
    public int Id { get; set; }
    public string StockTicker { get; set; }
    public DateTime Time { get; set; }
    public decimal Price { get; set; }
}