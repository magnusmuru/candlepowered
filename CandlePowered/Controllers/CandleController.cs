using CandlePowered.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CandlePowered.Controllers;

[ApiController]
[Route("[controller]")]
public class CandleController : ControllerBase
{
    private readonly ILogger<CandleController> _logger;
    private readonly ICandleService _candleService;

    public CandleController(ILogger<CandleController> logger,
        ICandleService candleService)
    {
        _logger = logger;
        _candleService = candleService;
    }

    [HttpGet(Name = "GetCandles")]
    public IActionResult Get([FromQuery] string ticker, [FromQuery] int candleLength = 1)
    {
        if (candleLength > 60)
        {
            _logger.LogWarning("User is trying to use candle length over parameter limit");
            return BadRequest("Parameter 'candleLength' cannot be over 60 minutes");
        }

        if (candleLength <= 0)
        {
            _logger.LogWarning("User is trying to use candle length under parameter limit");
            return BadRequest("Parameter 'candleLength' cannot be under 1 minute");
        }

        var candles = _candleService.FindStockCandles(ticker, candleLength);

        return Ok(candles);
        ;
    }
}