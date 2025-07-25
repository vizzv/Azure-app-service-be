namespace WebApi.Helpers;

using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
         var host = Environment.GetEnvironmentVariable("DB_HOST");
    var db = Environment.GetEnvironmentVariable("DB_NAME");
    var user = Environment.GetEnvironmentVariable("DB_USER");
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

    var connectionString = $"Host={host};Database={db};Username={user};Password={password}";
    Console.WriteLine(connectionString);
        options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention();
    }

    public DbSet<User> Users { get; set; }
}
