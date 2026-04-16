using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Models;

public class Company : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

    public string? Description { get; set; }

    // Navigation property: One Company can have Many JobPostings (1-to-Many relationship)
    public virtual ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
}