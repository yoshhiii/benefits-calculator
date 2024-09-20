using Api.Models;
using Api.Persistence.Repositories;
using Api.Services;
using NSubstitute;

namespace UnitTests;

public class PaycheckServiceTests
{
    private readonly List<Employee> _expectedEmployees = new()
    {
        new Employee
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        },
        new Employee
        {
            Id = 2,
            FirstName = "Ja",
            LastName = "Morant",
            Salary = 92365.22m,
            DateOfBirth = new DateTime(1999, 8, 10),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Spouse",
                    LastName = "Morant",
                    Relationship = Relationship.Spouse,
                    DateOfBirth = new DateTime(1998, 3, 3)
                },
                new()
                {
                    Id = 2,
                    FirstName = "Child1",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2020, 6, 23)
                },
                new()
                {
                    Id = 3,
                    FirstName = "Child2",
                    LastName = "Morant",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2021, 5, 18)
                }
            }
        },
        new Employee
        {
            Id = 3,
            FirstName = "Michael",
            LastName = "Jordan",
            Salary = 143211.12m,
            DateOfBirth = new DateTime(1963, 2, 17),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 4,
                    FirstName = "DP",
                    LastName = "Jordan",
                    Relationship = Relationship.DomesticPartner,
                    DateOfBirth = new DateTime(1974, 1, 2)
                }
            }
        }
    };

    private readonly IEmployeeRepository _mockEmployeeRepository = Substitute.For<IEmployeeRepository>();
    private readonly IPaycheckService _paycheckService;

    public PaycheckServiceTests()
    {
        _mockEmployeeRepository.GetEmployeeByIdAsync(Arg.Is<int>(id => id <= 3 && id > 0))
            .Returns(i => _expectedEmployees.First(d => d.Id == i.Arg<int>()));
        _mockEmployeeRepository.GetEmployeeByIdAsync(Arg.Is<int>(id => id > 3)).Returns((Employee?)null);

        _paycheckService = new PaycheckService(_mockEmployeeRepository);
    }

    [Fact]
    public async Task GetPaycheckByEmployeeUnder50WithNoDependents_WithValidId_ReturnsValidPaycheck()
    {
        var expectedEmployee = _expectedEmployees.First();
        var expectedPaycheck = new Paycheck
        {
            BasePay = 2900.81m, // 75420.99 / 26,
            BenefitCosts = 461.54m, // 12000 / 26,
            NetPay = 2439.27m // 2900.81 - 461.54
        };

        var actualPaycheck = await _paycheckService.GetPaycheckByEmployee(expectedEmployee.Id);

        Assert.NotNull(actualPaycheck);
        Assert.Equal(expectedPaycheck.BasePay, actualPaycheck.BasePay);
        Assert.Equal(expectedPaycheck.BenefitCosts, actualPaycheck.BenefitCosts);
        Assert.Equal(expectedPaycheck.NetPay, actualPaycheck.NetPay);
    }


    [Fact]
    public async Task GetPaycheckByEmployeeUnder50WithADependent_WithValidId_ReturnsValidPaycheck()
    {
        var expectedEmployee = _expectedEmployees.First();
        expectedEmployee.Dependents.Add(
            new Dependent
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            }
        );
        var expectedPaycheck = new Paycheck
        {
            BasePay = 2900.81m, // 75420.99 / 26,
            BenefitCosts = 738.46m, // 19200 / 26,
            NetPay = 2162.35m // 2900.81 - 738.46
        };

        var actualPaycheck = await _paycheckService.GetPaycheckByEmployee(expectedEmployee.Id);

        Assert.NotNull(actualPaycheck);
        Assert.Equal(expectedPaycheck.BasePay, actualPaycheck.BasePay);
        Assert.Equal(expectedPaycheck.BenefitCosts, actualPaycheck.BenefitCosts);
        Assert.Equal(expectedPaycheck.NetPay, actualPaycheck.NetPay);
    }

    [Fact]
    public async Task GetPaycheckByEmployeeOver50WithNoDependents_WithValidId_ReturnsValidPaycheck()
    {
        var expectedEmployee = _expectedEmployees.First();
        expectedEmployee.DateOfBirth = expectedEmployee.DateOfBirth.AddYears(-30);
        var expectedPaycheck = new Paycheck
        {
            BasePay = 2900.81m, // 75420.99 / 26,
            BenefitCosts = 553.85m, // 14400 / 26,
            NetPay = 2346.96m // 2900.81 - 738.46
        };

        var actualPaycheck = await _paycheckService.GetPaycheckByEmployee(expectedEmployee.Id);

        Assert.NotNull(actualPaycheck);
        Assert.Equal(expectedPaycheck.BasePay, actualPaycheck.BasePay);
        Assert.Equal(expectedPaycheck.BenefitCosts, actualPaycheck.BenefitCosts);
        Assert.Equal(expectedPaycheck.NetPay, actualPaycheck.NetPay);
    }

    [Fact]
    public async Task GetPaycheckByEmployeeOver50WithDependents_WithValidId_ReturnsValidPaycheck()
    {
        var expectedEmployee = _expectedEmployees.First();
        expectedEmployee.DateOfBirth = expectedEmployee.DateOfBirth.AddYears(-30);
        expectedEmployee.Dependents.Add(
            new Dependent
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            }
        );

        var expectedPaycheck = new Paycheck
        {
            BasePay = 2900.81m, // 75420.99 / 26,
            BenefitCosts = 830.77m, // 21600 / 26,
            NetPay = 2070.04m // 2900.81 - 830.77
        };

        var actualPaycheck = await _paycheckService.GetPaycheckByEmployee(expectedEmployee.Id);

        Assert.NotNull(actualPaycheck);
        Assert.Equal(expectedPaycheck.BasePay, actualPaycheck.BasePay);
        Assert.Equal(expectedPaycheck.BenefitCosts, actualPaycheck.BenefitCosts);
        Assert.Equal(expectedPaycheck.NetPay, actualPaycheck.NetPay);
    }

    [Fact]
    public async Task GetPaycheckByEmployeeWithHighSalary_WithValidId_ReturnsValidPaycheck()
    {
        var expectedEmployee = _expectedEmployees.First();
        expectedEmployee.Salary = 200000;
        var expectedPaycheck = new Paycheck
        {
            BasePay = 7692.31m, // 200000 / 26,
            BenefitCosts = 615.38m, // 16000 / 26,
            NetPay = 7076.93m // 7692.31 - 615.38
        };

        var actualPaycheck = await _paycheckService.GetPaycheckByEmployee(expectedEmployee.Id);

        Assert.NotNull(actualPaycheck);
        Assert.Equal(expectedPaycheck.BasePay, actualPaycheck.BasePay);
        Assert.Equal(expectedPaycheck.BenefitCosts, actualPaycheck.BenefitCosts);
        Assert.Equal(expectedPaycheck.NetPay, actualPaycheck.NetPay);
    }
}