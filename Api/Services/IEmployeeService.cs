using Api.Dtos.Employee;

namespace Api.Services;

public interface IEmployeeService
{
    Task<IReadOnlyList<GetEmployeeDto>> GetAllEmployees();
    Task<GetEmployeeDto?> GetEmployeeById(int id);
}