using Api.Dtos.Employee;

namespace Api.Services;

public interface IPaycheckService
{
    public Task<GetEmployeePaycheckDto> GetPaycheckByEmployee(int employeeId);
}