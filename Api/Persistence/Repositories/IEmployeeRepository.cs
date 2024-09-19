using Api.Models;

namespace Api.Persistence.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeByIdAsync(int employeeId);
    Task<IEnumerable<Employee>> GetEmployeesAsync();
}