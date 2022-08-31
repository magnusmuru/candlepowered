namespace CandlePowered.Dtos;

public class CandleDto
{
    public string Stock { get; set; }
    public DateTime StartTime { get; set; }
    public decimal Open { get; set; }
    public decimal Close { get; set; }
    public decimal Low { get; set; }
    public decimal High { get; set; }
}