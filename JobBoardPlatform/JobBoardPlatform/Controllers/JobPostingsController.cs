using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobBoardPlatform.Models;
using JobBoardPlatform.Services.Interfaces;
using JobBoardPlatform.Data;
using Microsoft.AspNetCore.Authorization; 
using System.Security.Claims;

namespace JobBoardPlatform.Controllers
{
    public class JobPostingsController : Controller
    {
        private readonly IJobService _jobService;
        private readonly ApplicationDbContext _context;

        public JobPostingsController(IJobService jobService, ApplicationDbContext context)
        {
            _jobService = jobService;
            _context = context;
        }

        // GET: Displays all job postings
        public async Task<IActionResult> Index()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return View(jobs);
        }

        // GET: Displays details for a specific job posting
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jobPosting = await _jobService.GetJobByIdAsync(id.Value);
            if (jobPosting == null) return NotFound();

            return View(jobPosting);
        }

        // GET: Create form - Authorized for Admins and Employers
        [Authorize(Roles = "Admin,Employer")]
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: Create a new job posting
        [HttpPost]
        [Authorize(Roles = "Admin,Employer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobPosting jobPosting)
        {
            // Set the current user as the publisher
            jobPosting.PublisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            jobPosting.CreatedAt = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                await _jobService.CreateJobAsync(jobPosting);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobPosting.CompanyId);
            return View(jobPosting);
        }

        // GET: Edit form - Only for Admins or the original Publisher
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jobPosting = await _jobService.GetJobByIdAsync(id.Value);
            if (jobPosting == null) return NotFound();

            // Authorization check: Only Admin or the owner can edit
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && jobPosting.PublisherId != userId)
            {
                return Forbid();
            }

            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobPosting.CompanyId);
            return View(jobPosting);
        }

        // POST: Update an existing job posting
        [HttpPost]
        [Authorize(Roles = "Admin,Employer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobPosting jobPosting)
        {
            if (id != jobPosting.Id) return NotFound();

            // Fix for "InvalidOperationException": Use AsNoTracking to avoid object tracking conflicts
            var existingJob = await _context.JobPostings
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == id);

            if (existingJob == null) return NotFound();

            // Check authorization again before saving
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && existingJob.PublisherId != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                // Preserve sensitive data that shouldn't be edited via the form
                jobPosting.PublisherId = existingJob.PublisherId;
                jobPosting.CreatedAt = existingJob.CreatedAt;

                await _jobService.UpdateJobAsync(jobPosting);
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobPosting.CompanyId);
            return View(jobPosting);
        }

        // GET: Delete confirmation - Restricted to Admin or Owner
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var jobPosting = await _jobService.GetJobByIdAsync(id.Value);
            if (jobPosting == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && jobPosting.PublisherId != userId)
            {
                return Forbid();
            }

            return View(jobPosting);
        }

        // POST: Confirmed deletion of a job posting
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Employer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existingJob = await _context.JobPostings.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && existingJob?.PublisherId != userId)
            {
                return Forbid();
            }

            await _jobService.DeleteJobAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}