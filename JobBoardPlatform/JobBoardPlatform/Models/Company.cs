using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Models;

public class Company : BaseEntity
{
    [Required(ErrorMessage = "Please enter a company name")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please provide a description")]
    public string Description { get; set; } = string.Empty;

   

    // Navigation property: One Company can have Many JobPostings (1-to-Many relationship)
    public virtual ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
}