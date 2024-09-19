using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public record GetEmployeeDto
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public decimal Salary { get; init; }
    public DateTime DateOfBirth { get; init; }
    public ICollection<GetDependentDto> Dependents { get; init; } = new List<GetDependentDto>();
}