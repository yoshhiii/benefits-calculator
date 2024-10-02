using Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Persistence.Repositories;

// Simple caching using Decorator Pattern
// Would replace in memory cache with distributed cache in real solution
public class CachedEmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeRepository _decorated;
    private readonly IMemoryCache _memoryCache;

    public CachedEmployeeRepository(EmployeeRepository decorated, IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
    }

    public Task<Employee?> GetEmployeeByIdAsync(int employeeId)
    {
        string key = $"employee-{employeeId}";
        
        return _memoryCache.GetOrCreateAsync(key, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
            return _decorated.GetEmployeeByIdAsync(employeeId);
        });
    }

    public Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return _decorated.GetEmployeesAsync();
    }
}