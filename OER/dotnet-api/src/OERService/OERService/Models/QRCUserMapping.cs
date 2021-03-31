using System;

namespace OERService.Models
{
	public class QrcUserMapping
    {
        public int? Id { get; set; }
        public int? QRCId { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Active { get; set; }
        public string EmailUrl { get; set; }
    }
    public class QrcUsers
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ResourceContributed { get; set; }
        public int CourseCreated { get; set; }
        public int CurrentQRCS { get; set; }
        public int NoOfReviews { get; set; }
        public Int64 Rownumber { get; set; }
        public Int64 TotalRows { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
    }
}
