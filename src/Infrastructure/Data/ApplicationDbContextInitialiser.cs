using System.Text.Json;
using CleanArch.Domain.Products;
using CleanArch.Domain.TodoLists;
using CleanArch.Domain.Users;
using CleanArch.Domain.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanArch.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(ApplicationDbContextInitialiser));
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await ApplicationDbContextInitialiser.InitialiseAsync(context, logger);
        await ApplicationDbContextInitialiser.SeedAsync(context, logger);
    }
}

public static class ApplicationDbContextInitialiser
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new ColourJsonConverter() },
    };

    public static async Task InitialiseAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            await TrySeedAsync(context, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private static async Task TrySeedAsync(ApplicationDbContext context, ILogger logger)
    {
        var baseDir = AppContext.BaseDirectory;
        var seedDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Infrastructure", "Data", "Seed"));
        var usersJsonPath = Path.Combine(seedDir, "users.json");
        var listsJsonPath = Path.Combine(seedDir, "todolists.json");
        var productsJsonPath = Path.Combine(seedDir, "products.json");

        if (!context.Users.Any())
        {
            if (!File.Exists(usersJsonPath))
            {
                logger.LogWarning("Seed file not found: {Path}", usersJsonPath);
            }
            else
            {
                logger.LogInformation("Seeding users from {Path}", usersJsonPath);
                var usersJson = await File.ReadAllTextAsync(usersJsonPath);
                var users = JsonSerializer.Deserialize<List<User>>(usersJson, _jsonOptions);
                if (users is null || users.Count == 0)
                {
                    logger.LogWarning("No data found in users.json");
                    return;
                }
                context.Users.AddRange(users);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {UserCount} users.", users.Count);
            }
        }
        if (!context.TodoLists.Any())
        {
            if (!File.Exists(listsJsonPath))
            {
                logger.LogWarning("Seed file not found: {Path}", listsJsonPath);
            }
            else
            {
                logger.LogInformation("Seeding todolists from {Path}", listsJsonPath);
                var listsJson = await File.ReadAllTextAsync(listsJsonPath);
                var listsData = JsonSerializer.Deserialize<List<TodoList>>(listsJson, _jsonOptions);
                if (listsData is null || listsData.Count == 0)
                {
                    logger.LogWarning("No data found in todolists.json");
                    return;
                }
                context.TodoLists.AddRange(listsData);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {ListCount} lists.", listsData.Count);
            }
        }
        if (!context.Products.Any() && File.Exists(productsJsonPath))
        {
            logger.LogInformation("Seeding products from {Path}", productsJsonPath);
            var productsJson = await File.ReadAllTextAsync(productsJsonPath);
            var products = JsonSerializer.Deserialize<List<Product>>(productsJson, _jsonOptions);
            if (products is null || products.Count == 0)
            {
                logger.LogWarning("No data found in products.json");
                return;
            }
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {ProductCount} products.", products.Count);
        }
    }
}
