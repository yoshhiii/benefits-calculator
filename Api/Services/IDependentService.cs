using Api.Dtos.Dependent;

namespace Api.Services;

public interface IDependentService
{
    public Task<IReadOnlyList<GetDependentDto>> GetAllDependents();
    public Task<GetDependentDto?> GetDependentById(int dependentId);
}