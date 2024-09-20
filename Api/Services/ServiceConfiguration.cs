namespace Api.Services;

public static class ServiceConfiguration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDependentService, DependentService>();
        services.AddScoped<IPaycheckService, PaycheckService>();
    }
}