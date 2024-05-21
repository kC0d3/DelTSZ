using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DelTSZ.Data;

public static class DataContextExtension
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        if (!await dataContext.Database.CanConnectAsync() || !await AllMigrationsApplied(dataContext))
        {
            await dataContext.Database.MigrateAsync();
        }
    }

    private static async Task<bool> AllMigrationsApplied(DbContext context)
    {
        var applied = await context.GetService<IHistoryRepository>()
            .GetAppliedMigrationsAsync();

        var total = context.GetService<IMigrationsAssembly>().Migrations.Select(m => m.Key);

        return !total.Except(applied.Select(m => m.MigrationId)).Any();
    }
}