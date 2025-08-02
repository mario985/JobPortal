using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _Context;
    private readonly UserManager<ApplicationUser> _userManager;
    public JobRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _Context = context;
        _userManager = userManager;


    }
    public async Task AddAsync(Job job)
    {        
        await _Context.Jobs.AddAsync(job);
        await _Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var job = await _Context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job != null)
        {
            _Context.Jobs.Remove(job);
           await _Context.SaveChangesAsync();
        }

        
    }

    public async Task EditAsync(Job job)
    {
        var targetJob = await _Context.Jobs.FirstOrDefaultAsync(j => j.Id == job.Id);
        if (targetJob != null)
        {
            targetJob.Title = job.Title;
            targetJob.Type = job.Type;
            targetJob.WorkHours = job.WorkHours;
            targetJob.Salary = job.Salary;
           await _Context.SaveChangesAsync();
            
            
        }
    }
    
    public async Task<List<Job>> GetAllAsync()
    {
        return await _Context.Jobs.Include(j =>j.Company).ToListAsync();
    }

    public async Task<List<Job>> GetByCompanyIdAsync(string id)
    {
        return await _Context.Jobs.Where(j => j.CompanyId == id).Include(j=>j.Company).ToListAsync();
        
    }

    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _Context.Jobs.Include(j=>j.Company).FirstOrDefaultAsync(j => j.Id == id);
    }

    public IEnumerable<Job> Search(string? title, string? location , string? companyId)
    {
        var query = _Context.Jobs.Include(j=>j.Company).AsQueryable();
        if (!string.IsNullOrEmpty(companyId))
        {
            query = query.Where(j => j.CompanyId == companyId);
            
        }
        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(j => j.Title == title).AsQueryable();
        }
        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(j => j.Location == location).AsQueryable();
        }
        return query.ToList();
    }
}