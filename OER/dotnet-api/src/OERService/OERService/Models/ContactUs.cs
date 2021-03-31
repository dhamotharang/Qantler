using System;
using System.Collections.Generic;

namespace OERService.Models
{
	public class ContactUs
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Int64 TotalRows { get; set; }
        public int IsReplied { get; set; }
        public string RepliedText { get; set; }
        public string RepliedBy { get; set; }
        public int? RepliedById { get; set; }
        public DateTime? RepliedOn { get; set; }

    }
    public class ContactUsCreate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }

    }
    public class ReviewerComments
    {
        public string Reviewer { get; set; }
        public string Reasons { get; set; }
    }
    public class Notification
    {
        public List<ReviewerComments> reviewerComments { get; set; }
        public int Id { get; set; }
        public int Total { get; set; }
        public int ReferenceId { get; set; }
        public int ReferenceTypeId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int MessageTypeId { get; set; }
        public string MessageType { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public Int64 Totalrows { get; set; }
        public Int64 Rownumber { get; set; }
        public string Reviewer { get; set; }
        public string Comment { get; set; }
        public string EmailUrl { get; set; }
        public int Status { get; set; }
        public Int64 TotalUnRead { get; set; }
    }
    public class QueryUpdate
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string ReplyText { get; set; }
        public int QueryId { get; set; }
        public int RepliedBy { get; set; }
        public string Url { get; set; }

    }
}
