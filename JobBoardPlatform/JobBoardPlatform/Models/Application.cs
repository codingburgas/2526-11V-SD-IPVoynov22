using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Models
{
    public class Application : BaseEntity
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Only letters and spaces are allowed")]
        public string CandidateName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CandidateEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a short introduction or CV link")]
        [StringLength(500)]
        public string MotivationLetter { get; set; } = string.Empty;

        // Foreign Key to JobPosting
        [Required]
        public int JobPostingId { get; set; }
        public virtual JobPosting? JobPosting { get; set; }
    }
}