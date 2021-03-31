using System;

namespace OERService.Models
{
	public class QrcManagement
    {
        public Int64 RowNumber { get; set; }
        public int ContentId { get; set; }
        public int ContentApprovalId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ContentType { get; set; }
        public int CategoryId { get; set; }
        public Int64 TotalRows { get; set; }
    }
    public class ContentApprovalRequest
    {
        public int? QrcId { get; set; }
        public int? CategoryId { get; set; }
        public int UserId { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
    public class ContentApproveRequest
    {

        public int ContentApprovalId { get; set; }
        public int ContentType { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public int CreatedBy { get; set; }
        public int ContentId { get; set; }
        public string EmailUrl { get; set; }
    }

}
