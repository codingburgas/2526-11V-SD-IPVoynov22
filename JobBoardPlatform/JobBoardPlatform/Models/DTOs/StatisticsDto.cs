namespace JobBoardPlatform.Models.DTOs
{
    public class StatisticsDto
    {
        // Top 3 categories with most jobs
        public Dictionary<string, int> TopCategories { get; set; } = new();

        // Average applications per job
        public double AverageApplicationsPerJob { get; set; }

        // Companies with highest number of postings
        public List<CompanyStatsDto> TopCompanies { get; set; } = new();
    }

    public class CompanyStatsDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public int JobCount { get; set; }
    }
}