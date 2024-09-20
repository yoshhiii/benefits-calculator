using System.Net.Http;
using Api.Persistence;
using Xunit;

namespace ApiTests;

// NOTE(@doug): Refactored to use IClassFixture to create shared context for tests, 
// as well as using the BenefitsCalculatorFactory to create the HttpClient instance.
public class IntegrationTestBase : IClassFixture<BenefitsCalculatorFactory>
{
    protected readonly BenefitsCalculatorDbContext BenefitsDbContext;
    protected readonly HttpClient HttpClient;

    public IntegrationTestBase(BenefitsCalculatorFactory factory)
    {
        HttpClient = factory.CreateClient();
        HttpClient.DefaultRequestHeaders.Add("accept", "text/plain");
        BenefitsDbContext = factory.CreateDbContext();
    }
}