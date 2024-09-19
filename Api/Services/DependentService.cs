using Api.Dtos.Dependent;
using Api.Persistence.Repositories;
using AutoMapper;

namespace Api.Services;

public class DependentService : IDependentService
{
    private readonly IDependentRepository _dependentRepository;
    private readonly IMapper _mapper;

    public DependentService(IMapper mapper, IDependentRepository dependentRepository)
    {
        _mapper = mapper;
        _dependentRepository = dependentRepository;
    }

    public async Task<IReadOnlyList<GetDependentDto>> GetAllDependents()
    {
        var dependents = await _dependentRepository.GetDependentsAsync();

        return _mapper.Map<IReadOnlyList<GetDependentDto>>(dependents);
    }

    public async Task<GetDependentDto?> GetDependentById(int dependentId)
    {
        var dependent = await _dependentRepository.GetDependentByIdAsync(dependentId);
        return dependent is not null ? _mapper.Map<GetDependentDto>(dependent) : null;
    }
}