using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobBoardPlatform.Data;
using JobBoardPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobBoardPlatform.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // take login id
            var userEmail = User.Identity?.Name;

            // take all candidatures with info
            IQueryable<Application> query = _context.Applications.Include(a => a.JobPosting);

            if (User.IsInRole("Admin"))
            {
                return View(await query.ToListAsync());
            }
            
            if (User.IsInRole("Employer"))
            {
                // filter for Employyer:
                query = query.Where(a => a.JobPosting.PublisherId == userId);
            }
            else if (User.IsInRole("Candidate"))
            {
                // filter for candidate 
                query = query.Where(a => a.CandidateEmail == userEmail);
            }

            var results = await query.ToListAsync();
            return View(results);
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var application = await _context.Applications
                .Include(a => a.JobPosting)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (application == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity?.Name;

            bool isAdmin = User.IsInRole("Admin");
            bool isEmployerOwner = User.IsInRole("Employer") && application.JobPosting?.PublisherId == userId;
            bool isTheCandidate = User.IsInRole("Candidate") && application.CandidateEmail == userEmail;

            if (!isAdmin && !isEmployerOwner && !isTheCandidate)
            {
                return Forbid();
            }

            return View(application);
        }

        [Authorize(Roles = "Candidate")]
        public IActionResult Create(int jobPostingId)
        {
            var job = _context.JobPostings.Find(jobPostingId);
            if (job == null) return NotFound();

            ViewBag.JobTitle = job.Title;
            var model = new Application { JobPostingId = jobPostingId };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Candidate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateName,MotivationLetter,JobPostingId")] Application application)
        {
            application.CandidateEmail = User.Identity?.Name ?? "unknown@user.com";
            application.CreatedAt = DateTime.Now;

            ModelState.Remove("CandidateEmail");
            ModelState.Remove("JobPosting");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(application);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var application = await _context.Applications.Include(a => a.JobPosting).FirstOrDefaultAsync(m => m.Id == id);
            if (application == null) return NotFound();
            return View(application);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application != null) _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}