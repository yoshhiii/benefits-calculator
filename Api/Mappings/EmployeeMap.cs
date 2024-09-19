using Api.Dtos.Employee;
using Api.Models;
using AutoMapper;

namespace Api.Mappings;

public class EmployeeMap : Profile
{
    public EmployeeMap()
    {
        CreateMap<Employee, GetEmployeeDto>()
            .ForMember(dest => dest.Dependents, opt => opt.MapFrom(src => src.Dependents));
    }
}