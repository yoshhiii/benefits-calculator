using Api.Dtos.Employee;
using Api.Models;
using Api.Persistence.Repositories;

namespace Api.Services;

public class PaycheckService : IPaycheckService
{
    // TODO: Move these constants to a configuration file or store in DB
    private const decimal MonthlyBaseSalaryCost = 1000;
    private const decimal MonthlyDependentCost = 600;
    private const decimal SalarayAdjustedBenefitCost = 0.02M;
    private const decimal AnnualSalaryThreshold = 80000;
    private const decimal AgeAdjustedBenefitCost = 200;
    private const int AgeAdjustmentThreshold = 50;
    private const int AnnualPaycheckCount = 26;

    private readonly IEmployeeRepository _employeeRepository;

    public PaycheckService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<GetEmployeePaycheckDto> GetPaycheckByEmployee(int employeeId)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);

        if (employee is null) throw new KeyNotFoundException($"Employee with id {employeeId} not found.");

        var paycheck = CalculatePaycheck(employee);

        return new GetEmployeePaycheckDto
        {
            EmployeeId = employee.Id,
            EmployeeFirstName = employee.FirstName,
            EmployeeLastName = employee.LastName,
            BasePay = paycheck.BasePay,
            BenefitCosts = paycheck.BenefitCosts,
            NetPay = paycheck.NetPay
        };
    }

    private Paycheck CalculatePaycheck(Employee employee)
    {
        var basePay = Math.Round(employee.Salary / AnnualPaycheckCount, 2);
        var annualBaseBenefitCost = MonthlyBaseSalaryCost * 12;
        var annualDependentBenefitCost =
            employee.Dependents.Any() ? employee.Dependents.Count * MonthlyDependentCost * 12 : 0;
        var totalAnnualBenefitsCost = annualBaseBenefitCost + annualDependentBenefitCost;

        if (DateTime.Today.Year - employee.DateOfBirth.Year > AgeAdjustmentThreshold)
            totalAnnualBenefitsCost += AgeAdjustedBenefitCost * 12;

        if (employee.Salary > AnnualSalaryThreshold)
            totalAnnualBenefitsCost += employee.Salary * SalarayAdjustedBenefitCost;

        var paycheckBenefitsCost = Math.Round(totalAnnualBenefitsCost / AnnualPaycheckCount, 2);

        return new Paycheck
        {
            BasePay = basePay,
            BenefitCosts = paycheckBenefitsCost,
            NetPay = Math.Round(basePay - paycheckBenefitsCost, 2)
        };
    }
}