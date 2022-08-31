using CandlePowered.DataContext;
using CandlePowered.Dtos;
using CandlePowered.Services.Interfaces;

namespace CandlePowered.Services;

public class CandleService : ICandleService
{
    private readonly CandleContext _context;

    public CandleService(CandleContext context)
    {
        _context = context;
    }

    public List<CandleDto> FindStockCandles(string ticker, int candleLength)
    {
        var candles = new List<CandleDto>();


        var stockData = _context.CandleDatas
            .Where(x => x.StockTicker == ticker);

        var groupedTimes = stockData
            .Where(x => x.StockTicker == ticker)
            .ToList()
            .GroupBy(x =>
            {
                var stamp = x.Time;
                stamp = stamp.AddMinutes(-(stamp.Minute % candleLength));
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp;
            });


        foreach (var data in groupedTimes)
        {
            var candle = new CandleDto();

            var sortedData = data.OrderBy(x => x.Time).ToList();
            candle.Stock = sortedData.First().StockTicker;
            candle.Open = sortedData.First().Price;
            candle.StartTime = data.Key;
            candle.Close = sortedData.Last().Price;

            candle.Low = data.OrderBy(d => d.Price).First().Price;
            candle.High = data.OrderByDescending(d => d.Price).First().Price;

            candles.Add(candle);
        }

        candles = candles.OrderBy(x => x.StartTime).ToList();

        return candles;
    }
}