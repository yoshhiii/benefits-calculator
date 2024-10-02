using Api.Dtos.Dependent;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;

    public DependentsController(IDependentService dependentService)
    {
        _dependentService = dependentService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _dependentService.GetDependentById(id);
        if (dependent is null)
            return NotFound(new ApiResponse<GetDependentDto>
            {
                Message = "Employee not found",
                Error = "Employee not found",
                Success = false
            });

        return Ok(new ApiResponse<GetDependentDto>
        {
            Data = dependent,
            Success = true
        });
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentService.GetAllDependents();

        if (dependents.Count == 0) return NoContent();

        return Ok(new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents.ToList(),
            Success = true
        });
    }
}