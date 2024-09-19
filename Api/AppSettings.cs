namespace Api;

public class AppSettings
{
    public ConnectionString? ConnectionStrings { get; init; }
}

public class ConnectionString
{
    public string? BenefitsCalculatorDb { get; set; }
}