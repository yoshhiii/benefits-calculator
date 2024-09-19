using Api.Models;

namespace Api.Persistence.Repositories;

public interface IDependentRepository
{
    Task<Dependent?> GetDependentByIdAsync(int employeeId);
    Task<IEnumerable<Dependent>> GetDependentsAsync();
}