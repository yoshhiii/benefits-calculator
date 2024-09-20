namespace Api.Models;

public class Paycheck
{
    public decimal BasePay { get; set; }
    public decimal BenefitCosts { get; set; }
    public decimal NetPay { get; set; }
}