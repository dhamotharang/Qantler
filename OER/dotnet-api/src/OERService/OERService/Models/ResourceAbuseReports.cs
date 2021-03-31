namespace OERService.Models
{
	public class CourseAbuseReports
    {
        public decimal? Id { get; set; }
        public decimal? CourseId { get; set; }
        public int? ReportedBy { get; set; }
        public string ReportReasons { get; set; }
        public string Comments { get; set; }
    }
}
