using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobBoardPlatform.Data;
using JobBoardPlatform.Models;

namespace JobBoardPlatform.Controllers
{
    public class JobPostingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobPostingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // JobPostings
        public async Task<IActionResult> Index()
        {
            // We use .Include to join the Company data
            var postings = await _context.JobPostings.Include(j => j.Company).ToListAsync();
            return View(postings);
        }

        //  JobPostings/Create
        public IActionResult Create()
        {
            // This fills a dropdown list with all companies
            ViewBag.CompanyId = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: JobPostings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Location,Salary,CompanyId")] JobPosting jobPosting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobPosting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CompanyId = new SelectList(_context.Companies, "Id", "Name", jobPosting.CompanyId);
            return View(jobPosting);
        }
    }
}