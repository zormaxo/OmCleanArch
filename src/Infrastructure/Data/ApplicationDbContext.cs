using System.Reflection;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Products;
using CleanArch.Domain.TodoItems;
using CleanArch.Domain.TodoLists;
using CleanArch.Domain.Users;
using CleanArch.Infrastructure.BackgroundJobs.Outbox;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options),
        IApplicationDbContext
{
    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Configure all DateTime properties to use timestamp without time zone for PostgreSQL
        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
        configurationBuilder.Properties<DateTime?>().HaveColumnType("timestamp without time zone");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
