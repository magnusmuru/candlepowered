using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using AutoMapper;
using CandlePowered.DataContext;
using CandlePowered.Dtos;
using CandlePowered.Entities;
using CandlePowered.Helpers;

namespace CandlePowered.Services;

public class WebsocketService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;

    private List<WebsocketDto>? _messages;
    private readonly ILogger<WebsocketService> _logger;
    private Timer? _timer;
    private readonly IMapper _mapper;

    public WebsocketService(ILogger<WebsocketService> logger,
        IMapper mapper, 
        IServiceScopeFactory scopeFactory)
    {
        _messages = new List<WebsocketDto>();
        _logger = logger;
        _mapper = mapper;
        _scopeFactory = scopeFactory;
    }

    private async Task RunWebSockets()
    {
        var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri("wss://testing-random-data.herokuapp.com/websockets"),
            CancellationToken.None);

        _logger.LogDebug("Connected to FVST Websocket");

        var receiving = Receiving(client);

        await receiving;
    }

    private async Task Receiving(WebSocket client)
    {
        var buffer = new byte[1024 * 4];

        while (true)
        {
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new UnixEpochDateTimeConverter());
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                var currentTicker = JsonSerializer.Deserialize<WebsocketDto>(message);

                if (currentTicker != null)
                {
                    _messages?.Add(currentTicker);
                }
            }

            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                break;
            }

            if (_messages?.Count > 250)
            {
                _logger.LogDebug("Adding to Cache");

                var mappedMessages = _mapper.Map<List<WebsocketDto>, List<CandleData>>(_messages);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var candleContext = scope.ServiceProvider.GetRequiredService<CandleContext>();
                    
                    await candleContext.CandleDatas.AddRangeAsync(mappedMessages);
                    await candleContext.SaveChangesAsync();
                }
                
                _messages = new List<WebsocketDto>();
            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(15));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        RunWebSockets().GetAwaiter().GetResult();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}