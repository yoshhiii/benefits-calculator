using Api.Dtos.Dependent;
using Api.Models;
using AutoMapper;

namespace Api.Mappings;

public class DependentMap : Profile
{
    public DependentMap()
    {
        CreateMap<Dependent, GetDependentDto>();
    }
}