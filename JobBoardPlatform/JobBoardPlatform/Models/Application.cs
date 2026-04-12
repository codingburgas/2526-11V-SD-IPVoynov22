using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Models;

public class Application : BaseEntity
{
    [Required]
    public string ApplicantName { get; set; }

    [Required]
    [EmailAddress]
    public string ApplicantEmail { get; set; }

    public string CvContent { get; set; }

    // Foreign key to link the Application to a specific Job Posting
    public int JobPostingId { get; set; }
    public virtual JobPosting JobPosting { get; set; }
}