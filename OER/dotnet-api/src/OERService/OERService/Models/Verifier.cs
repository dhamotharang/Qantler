using System;

namespace OERService.Models
{
	public class Verifier
    {
    }
    public class Content
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ContentType { get; set; }
        public Int64 Totalrows { get; set; }
        public Int64 Rownumber { get; set; }
    }
    public class ContentUpdate
    {
        public int UserId { get; set; }
        public int ContentId { get; set; }
        public int ContentType { get; set; }
        public int Status { get; set; }
    }
    public class VerifiersReport
    {
        public string VerifierName { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public Int64 Totalrows { get; set; }
        public Int64 Rownumber { get; set; }
    }
}
