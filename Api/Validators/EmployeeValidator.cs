using Api.Models;
using FluentValidation;

namespace Api.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(employee => employee.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(1, 50).WithMessage("First name must be between 1 and 50 characters long.");

        RuleFor(employee => employee.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(1, 50).WithMessage("Last name must be between 1 and 50 characters long.");

        RuleFor(employee => employee.Salary)
            .GreaterThan(0).WithMessage("Salary must be greater than 0.");

        RuleFor(employee => employee.DateOfBirth)
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

        RuleFor(employee => employee.Dependents)
            .NotNull().WithMessage("Dependents collection cannot be null.")
            .Must(CheckSpouseOrDomesticPartner)
            .WithMessage("An employee may only have one dependent that is a spouse or domestic partner.");
    }

    private bool CheckSpouseOrDomesticPartner(ICollection<Dependent> dependents)
    {
        var spouseCount = dependents.Count(d => d.Relationship == Relationship.Spouse);
        var domesticPartnerCount = dependents.Count(d => d.Relationship == Relationship.DomesticPartner);

        return spouseCount <= 1 && domesticPartnerCount <= 1;
    }
}