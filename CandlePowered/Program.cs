using CandlePowered.DataContext;
using CandlePowered.Services;
using CandlePowered.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<CandleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CandlePowered")));

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Wire Serilog into builder
builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration));

builder.Services.AddHostedService<WebsocketService>();
builder.Services.AddScoped<ICandleService, CandleService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

DataHelper.SetupAppData(app, app.Environment, app.Configuration);

app.UseWebSockets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();