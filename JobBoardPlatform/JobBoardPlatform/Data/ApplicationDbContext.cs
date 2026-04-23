using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobBoardPlatform.Models; 

namespace JobBoardPlatform.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<JobPosting> JobPostings { get; set; }
    public DbSet<Application> Applications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
       
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<JobPosting>()
            .Property(j => j.Salary)
            .HasColumnType("decimal(18,2)");
    }
}