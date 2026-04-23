using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobBoardPlatform.Models;
using JobBoardPlatform.Services.Interfaces;
using JobBoardPlatform.Data;

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

        // GET: JobPostings
        public async Task<IActionResult> Index()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return View(jobs);
        }

        // GET: JobPostings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jobPosting = await _jobService.GetJobByIdAsync(id.Value);
            if (jobPosting == null) return NotFound();

            // Fixed: changed JobPosting to jobPosting
            return View(jobPosting);
        }

        // GET: JobPostings/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobPosting jobPosting)
        {
            if (ModelState.IsValid)
            {
                await _jobService.CreateJobAsync(jobPosting);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobPosting.CompanyId);
            return View(jobPosting);
        }

        // GET: JobPostings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jobPosting = await _jobService.GetJobByIdAsync(id.Value);
            if (jobPosting == null) return NotFound();

            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobPosting.CompanyId);
            
            // Fixed: changed JobPosting to jobPosting
            return View(jobPosting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobPosting jobPosting)
        {
            if (id != jobPosting.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _jobService.UpdateJobAsync(jobPosting);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobPosting.CompanyId);
            return View(jobPosting);
        }

        // GET: JobPostings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var jobPosting = await _jobService.GetJobByIdAsync(id.Value);
            if (jobPosting == null) return NotFound();

            return View(jobPosting);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _jobService.DeleteJobAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}