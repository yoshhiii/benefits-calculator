namespace Api.Dtos.Employee;

public record GetEmployeePaycheckDto
{
    public int EmployeeId { get; init; }
    public string? EmployeeFirstName { get; init; } = string.Empty;
    public string? EmployeeLastName { get; init; } = string.Empty;
    public decimal BasePay { get; init; }
    public decimal BenefitCosts { get; init; }
    public decimal NetPay { get; init; }
}