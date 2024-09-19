namespace Api;

public static class AutoMapperConfiguration
{
    public static void AutoMapperInit(this IServiceCollection services)
    {
        var assembly = typeof(AutoMapperConfiguration).Assembly;
        services.AddAutoMapper(assembly);
    }
}