using Api.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence;

public static class PersistenceConfiguration
{
    public static void AddPersistence(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<BenefitsCalculatorDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<EmployeeRepository>();
        services.AddScoped<IEmployeeRepository, CachedEmployeeRepository>();
        services.AddScoped<IDependentRepository, DependentRepository>();

        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BenefitsCalculatorDbContext>();
        dbContext.Database.EnsureCreated();
    }
}