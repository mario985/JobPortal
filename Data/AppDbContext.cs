using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public AppDbContext() { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure if options haven't already been set (e.g. from Program.cs)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<JobApplication>()
        .HasKey(ja => new { ja.JobId, ja.UserId });
        modelBuilder.Entity<JobApplication>()
        .HasOne(ja => ja.Job)
        .WithMany(ja => ja.JobApplications)
        .HasForeignKey(ja => ja.JobId);
        modelBuilder.Entity<JobApplication>()
        .HasOne(ja => ja.User)
        .WithMany(ja => ja.JobApplications)
        .HasForeignKey(ja => ja.UserId);

    }
    public DbSet<Job> Jobs { set; get; }
    public DbSet<JobApplication> jobApplications { set; get; }
    public DbSet<ApplicationUser> applicationUsers { set; get; }
    
}