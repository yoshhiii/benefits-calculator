using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Persistence.Repositories;
using Api.Services;
using AutoMapper;
using NSubstitute;

namespace UnitTests;

public class EmployeeServiceTests
{
    private readonly MapperConfiguration _config;
    private readonly IEmployeeService _employeeService;

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

    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _mockEmployeeRepository = Substitute.For<IEmployeeRepository>();

    public EmployeeServiceTests()
    {
        _mockEmployeeRepository.GetEmployeeByIdAsync(Arg.Is<int>(id => id <= 3 && id > 0))
            .Returns(i => _expectedEmployees.First(d => d.Id == i.Arg<int>()));
        _mockEmployeeRepository.GetEmployeeByIdAsync(Arg.Is<int>(id => id > 3)).Returns((Employee?)null);
        _mockEmployeeRepository.GetEmployeesAsync().Returns(_expectedEmployees);


        _config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Employee, GetEmployeeDto>();
            cfg.CreateMap<Dependent, GetDependentDto>();
        });
        _mapper = _config.CreateMapper();

        _employeeService = new EmployeeService(_mapper, _mockEmployeeRepository);
    }

    [Fact]
    public async Task GetEmployeeById_WithValidId_ReturnsEmployee()
    {
        var expectedEmployee = _expectedEmployees.First();

        var actualEmployee = await _employeeService.GetEmployeeById(expectedEmployee.Id);

        Assert.NotNull(actualEmployee);
        Assert.Equal(expectedEmployee.Id, actualEmployee.Id);
        Assert.Equal(expectedEmployee.FirstName, actualEmployee.FirstName);
        Assert.Equal(expectedEmployee.LastName, actualEmployee.LastName);
        Assert.Equal(expectedEmployee.DateOfBirth, actualEmployee.DateOfBirth);
        Assert.Equal(expectedEmployee.Salary, actualEmployee.Salary);
        Assert.Equal(expectedEmployee.Dependents.Count, actualEmployee.Dependents.Count);
    }

    [Fact]
    public async Task GetEmployeeById_WithInvalidId_ReturnsNull()
    {
        var invalidId = 10;

        var actualEmployee = await _employeeService.GetEmployeeById(invalidId);

        Assert.Null(actualEmployee);
    }

    [Fact]
    public async Task GetAllEmployees_ReturnsAllEmployees()
    {
        var expectedEmployeeCount = 3;

        var actualEmployees = await _employeeService.GetAllEmployees();

        Assert.Equal(expectedEmployeeCount, actualEmployees.Count);
    }
}