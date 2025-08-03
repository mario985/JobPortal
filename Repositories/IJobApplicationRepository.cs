public interface IJobApplicationRepository
{
    Task AddAsync(JobApplication job);
    Task<List<JobApplication>> CompanyApplications(string companyId);
    Task<List<JobApplication>> UserApplications(string userId);
    Task<JobApplication> Details(int jobId, string userId);
    Task UpdateAsync(JobApplication job);



}