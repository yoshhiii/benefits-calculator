using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence;

public class BenefitsCalculatorDbContext : DbContext
{
    public BenefitsCalculatorDbContext(DbContextOptions<BenefitsCalculatorDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Dependent> Dependents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Dependents)
            .WithOne(d => d.Employee)
            .HasForeignKey(d => d.EmployeeId);

        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new Employee
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10)
            },
            new Employee
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17)
            }
        );

        modelBuilder.Entity<Dependent>().HasData(
            new Dependent
            {
                Id = 1,
                EmployeeId = 2,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3)
            },
            new Dependent
            {
                Id = 2,
                EmployeeId = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            },
            new Dependent
            {
                Id = 3,
                EmployeeId = 2,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18)
            },
            new Dependent
            {
                Id = 4,
                EmployeeId = 3,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2)
            }
        );
    }
}