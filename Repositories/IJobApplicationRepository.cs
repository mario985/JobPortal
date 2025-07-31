public interface IJobApplicationRepository
{
    Task AddAsync(JobApplication job);
    Task<List<JobApplication>> CompanyApplications(string companyId);
    Task<JobApplication> Details(int jobId, string userId);
    Task<List<JobApplication>> UserApplications(string userId);


}