using CandlePowered.Dtos;

namespace CandlePowered.Services.Interfaces;

public interface ICandleService
{
    public List<CandleDto> FindStockCandles(string ticker, int candleLength);
}