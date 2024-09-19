using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly BenefitsCalculatorDbContext _benefitsCalculatorDbContext;

    public EmployeeRepository(BenefitsCalculatorDbContext benefitsCalculatorDbContext)
    {
        _benefitsCalculatorDbContext = benefitsCalculatorDbContext;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
    {
        return await _benefitsCalculatorDbContext.Employees.Include(e => e.Dependents)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await _benefitsCalculatorDbContext.Employees.Include(e => e.Dependents).ToListAsync();
    }
}