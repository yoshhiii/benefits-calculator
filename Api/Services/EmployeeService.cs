using Api.Dtos.Employee;
using Api.Persistence.Repositories;
using AutoMapper;

namespace Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository)
    {
        _mapper = mapper;
        _employeeRepository = employeeRepository;
    }

    public async Task<IReadOnlyList<GetEmployeeDto>> GetAllEmployees()
    {
        var employees = await _employeeRepository.GetEmployeesAsync();

        return _mapper.Map<IReadOnlyList<GetEmployeeDto>>(employees);
    }

    public async Task<GetEmployeeDto?> GetEmployeeById(int employeeId)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        return employee is not null ? _mapper.Map<GetEmployeeDto>(employee) : null;
    }
}