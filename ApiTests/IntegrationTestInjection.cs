using System.Data;
using Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApiTests;

public static class IntegrationTestInjection
{
    public static void AddTestDatabase(this IServiceCollection services, string connectionString)
    {
        services.RemoveAll<DbContextOptions<BenefitsCalculatorDbContext>>();
        services.RemoveAll<IDbConnection>();

        services.AddDbContext<BenefitsCalculatorDbContext>(options => options.UseSqlite(connectionString));
    }
}