using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IPaycheckService _paycheckService;

    public EmployeesController(IEmployeeService employeeService, IPaycheckService paycheckService)
    {
        _employeeService = employeeService;
        _paycheckService = paycheckService;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _employeeService.GetEmployeeById(id);
        if (employee is null)
            return NotFound(new ApiResponse<GetEmployeeDto>
            {
                Message = "Employee not found",
                Error = "Employee not found",
                Success = false
            });

        return Ok(new ApiResponse<GetEmployeeDto>
        {
            Data = employee,
            Success = true
        });
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await _employeeService.GetAllEmployees();

        if (employees.Count == 0) return NoContent();

        return Ok(new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees.ToList(),
            Success = true
        });
    }

    [SwaggerOperation(Summary = "Get paycheck for an employee")]
    [HttpGet("{id}/paycheck")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<GetEmployeePaycheckDto>>> GetPaycheck(int id)
    {
        var paycheck = await _paycheckService.GetPaycheckByEmployee(id);

        return Ok(new ApiResponse<GetEmployeePaycheckDto>
        {
            Data = paycheck,
            Success = true
        });
    }
}