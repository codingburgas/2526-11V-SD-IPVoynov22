using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Models;

public class JobPosting : BaseEntity
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    public string Category { get; set; }

    [Range(0, 100000)]
    public decimal Salary { get; set; }

    // Foreign key to link the Job Posting to a specific Company
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; }

    // Navigation property: One JobPosting can have Many Applications
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}