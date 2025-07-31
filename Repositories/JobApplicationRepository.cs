
using Microsoft.EntityFrameworkCore;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext _context;
    public JobApplicationRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(JobApplication job)
    {
        var exits = await _context.jobApplications.AnyAsync(ja => ja.JobId == job.JobId && ja.UserId == job.UserId);
        if (!exits &&job!=null)
        {
            await _context.jobApplications.AddAsync(job);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<JobApplication>> CompanyApplications(string companyId)
    {
        var Applications =
        await _context
        .jobApplications
        .Include(ja => ja.Job)
        .Where(ja =>ja.Job.CompanyId==companyId)
        .ToListAsync();
        return Applications;
    }

    public async Task<JobApplication?> Details(int jobId, string userId)
    {
        var application =
        await _context.jobApplications
        .Include(ja => ja.Job)
        .Include(ja =>ja.User)
        .FirstOrDefaultAsync(ja => ja.JobId == jobId && ja.UserId == userId);
      
        return application;

        
    }

    public async Task<List<JobApplication>> UserApplications(string userId)
    {

        var userApplications =
        await _context
        .jobApplications.Where(ja => ja.UserId == userId)
        .Include(ja =>ja.Job)
        .ToListAsync();
        return userApplications;
    }
}