using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence.Repositories;

public class DependentRepository : IDependentRepository
{
    private readonly BenefitsCalculatorDbContext _dbContext;

    public DependentRepository(BenefitsCalculatorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Dependent?> GetDependentByIdAsync(int dependentId)
    {
        return await _dbContext.Dependents.FindAsync(dependentId);
    }

    public async Task<IEnumerable<Dependent>> GetDependentsAsync()
    {
        return await _dbContext.Dependents.ToListAsync();
    }
}