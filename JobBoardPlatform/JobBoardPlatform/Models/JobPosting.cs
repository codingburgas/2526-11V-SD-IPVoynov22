using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Models
{
    public class JobPosting : BaseEntity
    {
        [Required(ErrorMessage = "Title is required")]
        [RegularExpression(@"^[a-zA-Z\s\-]*$", ErrorMessage = "Title can only contain letters")]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [RegularExpression(@"^[a-zA-Z\s\.\,\!\?]*$", ErrorMessage = "Description can only contain letters and basic punctuation")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [RegularExpression(@"^[a-zA-Z\s\-]*$", ErrorMessage = "Location can only contain letters")]
        [StringLength(50, MinimumLength = 2)]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary is required")]
        [Range(0, 1000000, ErrorMessage = "Please enter a valid salary amount")]
        [DataType(DataType.Currency)]
      
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Please select a company")]
        public int CompanyId { get; set; }
        
        public virtual Company? Company { get; set; }
    }
}