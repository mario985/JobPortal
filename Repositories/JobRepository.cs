using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; 

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _Context;
    public JobRepository(AppDbContext context)
    {
        _Context = context;
        
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
        return await _Context.Jobs.ToListAsync();
    }

    public async Task<List<Job>> GetByCompanyIdAsync(string id)
    {
        return await _Context.Jobs.Where(j => j.CompanyId == id).ToListAsync();
        
    }

    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _Context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
    }

    public IEnumerable<Job> Search(string? title, string? location , string? companyId)
    {
        var query = _Context.Jobs.AsQueryable();
        if (!string.IsNullOrEmpty(companyId))
        {
            query = query.Where(j => j.CompanyId == companyId);
            
        }
        if (string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(j => j.Title == title).AsQueryable();
        }
        if (string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(j => j.Location == location).AsQueryable();
        }
        return query.ToList();
    }
}