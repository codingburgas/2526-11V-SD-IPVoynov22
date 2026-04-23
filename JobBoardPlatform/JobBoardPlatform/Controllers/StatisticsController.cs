using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobBoardPlatform.Data;
using JobBoardPlatform.Models.DTOs;

namespace JobBoardPlatform.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stats = new StatisticsDto();

            // 1. LINQ Fix: Get grouped data first, then convert to Dictionary in memory
            var groupedLocations = await _context.JobPostings
                .GroupBy(j => j.Location)
                .Select(g => new { 
                    Location = g.Key ?? "Remote/Unknown", 
                    Count = g.Count() 
                })
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToListAsync(); // Pulls data from SQL to C# memory

            stats.TopCategories = groupedLocations.ToDictionary(x => x.Location, x => x.Count);

            // 2. LINQ: Average applications per job
            var totalJobs = await _context.JobPostings.CountAsync();
            var totalApps = await _context.Applications.CountAsync();
            stats.AverageApplicationsPerJob = totalJobs > 0 ? (double)totalApps / totalJobs : 0;

            // 3. LINQ: Top Companies by postings count
            stats.TopCompanies = await _context.Companies
                .Select(c => new CompanyStatsDto
                {
                    CompanyName = c.Name,
                    JobCount = c.JobPostings.Count()
                })
                .OrderByDescending(c => c.JobCount)
                .Take(5)
                .ToListAsync();

            // Still using Json for testing until we create the View
            return View(stats);
        }
    }
}