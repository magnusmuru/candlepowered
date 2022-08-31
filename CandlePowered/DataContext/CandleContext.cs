using CandlePowered.Entities;
using Microsoft.EntityFrameworkCore;

namespace CandlePowered.DataContext;

public class CandleContext : DbContext
{
    public CandleContext(DbContextOptions<CandleContext> options) : base(options)
    {
    }

    public DbSet<CandleData> CandleDatas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}