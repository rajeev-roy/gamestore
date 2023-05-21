using GameStore.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        await dbContext.Database.MigrateAsync();

        var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                                    .CreateLogger("DB Initializer");
        logger.LogInformation("Initializing database");
    }

    public static IServiceCollection AddRespositories(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("GameStoreContext");
        services.AddSqlServer<GameStoreContext>(connectionString);
        services.AddScoped<IGamesRepository, EntityFrameworkGamesRepository>();
        return services;
    }
}