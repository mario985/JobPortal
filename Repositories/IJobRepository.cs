public interface IJobRepository
{
     Task AddAsync(Job job);
     Task EditAsync(Job job);
     Task DeleteAsync(int id);
     Task<List<Job>> GetAllAsync();
     Task<Job> GetByIdAsync(int id);
     Task<List<Job>> GetByCompanyIdAsync(string id);
}
