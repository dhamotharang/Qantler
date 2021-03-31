using System;

namespace OERService.Models
{
	public class SensoryCheck
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public int ContentType { get; set; }
        public Int64 Totalrows { get; set; }
        public Int64 Rownumber { get; set; }
        public string Category { get; set; }
    }
    public class SensoryContentStatus
    {
        public int UserId { get; set; }
        public int ContentId { get; set; }
        public int ContentType { get; set; }
        public int Status { get; set; }
        public string comments { get; set; }
        public string EmailUrl { get; set; }
    }
}
