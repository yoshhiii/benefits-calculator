using Api.Dtos.Dependent;
using Api.Models;
using Api.Persistence.Repositories;
using Api.Services;
using AutoMapper;
using NSubstitute;

namespace UnitTests;

public class DependentServiceTests
{
    private readonly MapperConfiguration _config;
    private readonly IDependentService _dependentService;

    private readonly List<Dependent> _expectedDependents = new()
    {
        new Dependent
        {
            Id = 1,
            FirstName = "Dependent",
            LastName = "One",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        },
        new Dependent
        {
            Id = 2,
            FirstName = "Dependent",
            LastName = "Two",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(2020, 6, 23)
        },
        new Dependent
        {
            Id = 3,
            FirstName = "Dependent",
            LastName = "Three",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(2021, 5, 18)
        }
    };

    private readonly IMapper _mapper;

    private readonly IDependentRepository _mockDependentRepository = Substitute.For<IDependentRepository>();

    public DependentServiceTests()
    {
        _mockDependentRepository.GetDependentByIdAsync(Arg.Is<int>(id => id <= 3 && id > 0))
            .Returns(i => _expectedDependents.First(d => d.Id == i.Arg<int>()));
        _mockDependentRepository.GetDependentByIdAsync(Arg.Is<int>(id => id > 3)).Returns((Dependent?)null);
        _mockDependentRepository.GetDependentsAsync().Returns(_expectedDependents);

        _config = new MapperConfiguration(cfg => { cfg.CreateMap<Dependent, GetDependentDto>(); });
        _mapper = _config.CreateMapper();

        _dependentService = new DependentService(_mapper, _mockDependentRepository);
    }

    [Fact]
    public async Task GetDependentById_WithValidId_ReturnsDependent()
    {
        var expectedDependent = _expectedDependents.First();

        var actualDependent = await _dependentService.GetDependentById(expectedDependent.Id);

        Assert.NotNull(actualDependent);
        if (actualDependent != null)
        {
            Assert.Equal(expectedDependent.Id, actualDependent.Id);
            Assert.Equal(expectedDependent.FirstName, actualDependent.FirstName);
            Assert.Equal(expectedDependent.LastName, actualDependent.LastName);
            Assert.Equal(expectedDependent.DateOfBirth, actualDependent.DateOfBirth);
            Assert.Equal(expectedDependent.Relationship, actualDependent.Relationship);
        }
    }

    [Fact]
    public async Task GetDependentById_WithInvalidId_ReturnsNull()
    {
        var invalidId = 10;

        var actualDependent = await _dependentService.GetDependentById(invalidId);

        Assert.Null(actualDependent);
    }

    [Fact]
    public async Task GetAllDependents_ReturnsAllDependents()
    {
        var expectedCount = 3;

        var actualDependents = await _dependentService.GetAllDependents();

        Assert.Equal(expectedCount, actualDependents.Count);
    }
}