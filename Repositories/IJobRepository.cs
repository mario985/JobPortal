public interface IJobRepository
{
     Task AddAsync(Job job);
     Task EditAsync(Job job);
     Task DeleteAsync(int id);
     Task<List<Job>> GetAllAsync();
     Task<Job> GetByIdAsync(int id);
     Task<List<Job>> GetByCompanyIdAsync(string id);
     IEnumerable<Job> Search(string? title, string? location , string? companyId);
}
