using JobBoardPlatform.Data;
using JobBoardPlatform.Models;
using JobBoardPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Services
{
    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _context;

        public JobService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobPosting>> GetAllJobsAsync()
        {
            return await _context.JobPostings
                .Include(j => j.Company)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }

        public async Task<JobPosting?> GetJobByIdAsync(int id)
        {
            return await _context.JobPostings
                .Include(j => j.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateJobAsync(JobPosting job)
        {
            _context.Add(job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateJobAsync(JobPosting job)
        {
            _context.Update(job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _context.JobPostings.FindAsync(id);
            if (job != null)
            {
                _context.JobPostings.Remove(job);
                await _context.SaveChangesAsync();
            }
        }

        public bool JobExists(int id)
        {
            return _context.JobPostings.Any(e => e.Id == id);
        }
    }
}