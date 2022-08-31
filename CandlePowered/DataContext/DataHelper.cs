using Microsoft.EntityFrameworkCore;

namespace CandlePowered.DataContext;

public class DataHelper
{
    private static readonly Guid DataSeedUser = Guid.Parse("bf3a5bc2-0e6e-4e77-a477-5445ad46c990");

    public static void SetupAppData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        using var context = serviceScope
            .ServiceProvider.GetService<CandleContext>();

        if (context == null)
        {
            throw new ApplicationException("Problem in services. No db context.");
        }

        if (configuration.GetValue<bool>("DataInitialization:DropDatabase"))
        {
            context.Database.EnsureDeleted();
        }

        if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase"))
        {
            context.Database.Migrate();
        }
    }
}