using Api.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace ApiTests;

// NOTE(@doug): Refactored to use WebApplicationFactory to create an instance of the api for testing
// as opposed to running the api in a separate process -- mostly done as a convenience for local
// development and testing.
public class BenefitsCalculatorFactory : WebApplicationFactory<Program>
{
    private const string IntegrationDatabase = "Data Source=IntegrationDatabase.db";
    private static volatile bool _databaseInitialized;
    private static readonly object _lock = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Integration");
        builder.ConfigureServices(services => { services.AddTestDatabase(IntegrationDatabase); });
    }

    protected internal BenefitsCalculatorDbContext CreateDbContext()
    {
        lock (_lock)
        {
            var dbContext = new BenefitsCalculatorDbContext(new DbContextOptionsBuilder<BenefitsCalculatorDbContext>()
                .UseSqlite(IntegrationDatabase).Options);

            dbContext.Database.EnsureCreated();
            _databaseInitialized = true;
            return dbContext;
        }
    }
}