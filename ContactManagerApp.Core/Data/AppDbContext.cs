using ContactManagerApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerApp.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().HasData(
            new Contact
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                DateOfBirth = new DateTime(1980, 5, 15),
                Married = true,
                Phone = "+1-555-1234567",
                Salary = 50000m
            },
            new Contact
            {
                Id = Guid.NewGuid(),
                Name = "Jane Smith",
                DateOfBirth = new DateTime(1990, 8, 22),
                Married = false,
                Phone = "+1-555-7654321",
                Salary = 60000m
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}
