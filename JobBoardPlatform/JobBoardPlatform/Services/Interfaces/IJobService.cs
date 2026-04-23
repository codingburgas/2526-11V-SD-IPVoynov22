using JobBoardPlatform.Models;

namespace JobBoardPlatform.Services.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<JobPosting>> GetAllJobsAsync();
        Task<JobPosting?> GetJobByIdAsync(int id);
        Task CreateJobAsync(JobPosting job);
        Task UpdateJobAsync(JobPosting job);
        Task DeleteJobAsync(int id);
        bool JobExists(int id);
    }
}