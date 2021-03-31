using System;
using System.Collections.Generic;

namespace OERService.Models
{
	public class Reports
    {
        public int Contributors { get; set; }
        public int Resources { get; set; }
        public int Courses { get; set; }
        public int? TotalVisit { get; set; }
        public List<TopReviewers> TopReviewers { get; set; }
        public List<TopContributors> TopContributors { get; set; }
    }
    public class QrcReportData
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int? UserCount { get; set; }
        public int? ApproveCount { get; set; }
        public int? RejectCount { get; set; }
        public int? PendingAction { get; set; }
        public int? Submission { get; set; }
    }
    public class UserDashboard
    {
        public int DraftCourses { get; set; }
        public int DraftResources { get; set; }
        public int PublishedCourses { get; set; }
        public int PubishedResources { get; set; }
        public int CourseToApprove { get; set; }
        public int ResourceToApprove { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public string ProfileDescription { get; set; }
        public List<LatestCourse> LatestCourse { get;set;}
        public int DownloadedResources { get;  set; }
        public int DownloadedCourses { get;  set; }
        public int SharedCourses { get;  set; }
        public int SharedResources { get;  set; }
    }
    public class SharedContentInfo
    {
        public int? ContentCount { get; set; }
        public int? UserSharedCount { get; set; }
    }
    public class SharedContentInput
    {
        public int? ContentId { get; set; }
        public int ContentTypeId { get; set; }
        public int? UserId { get; set; }
    }
    public class LatestCourse
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ContentType { get; set; }
        public string Thumbnail { get; set; }
    }
    public class RecommendedContent
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ContentType { get; set; }
        public string Thumbnail { get; set; }
        public Int64 TotalRows { get; set; }
        public Int64 Rownumber { get; set; }

        public string CTitle { get; set; }
        public string CTitleAr   {get; set; }
        public string  CDesc  { get; set; }

        public string CDescAr { get; set; }

        public string Media { get; set; }

    }
    public class RejectedContent
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public int ContentType { get; set; }
        public Int64 TotalRows { get; set; }
        public Int64 Rownumber { get; set; }

    }
    public class ReportAbuseContent
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public int? ReportAbuseCount { get; set; }
        public string Description { get; set; }
        public int ContentType { get; set; }
        public string Reason { get; set; }
        public string ReportReasons { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int ContentId { get; set; }
    }
    public class Visiters
    {
        public int? TotalVisits { get; set; }
    }
    public class TopReviewers
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int CourseCount { get; set; }
        public string Photo { get; set; }
    }
    public class TopContributors
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int CourseCount { get; set; }
        public string Photo { get; set; }
    }
    public class Users
    {
        public int Id { get; set; }
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public PortalLanguage PortalLanguage { get; set; }
        public Department Department { get; set; }
        public Designation Designation { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Photo { get; set; }
        public string ProfileDescription { get; set; }
        public string SubjectsInterested { get; set; }
        public bool? ApprovalStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? Active { get; set; }
        public bool? IsContributor { get; set; }
        public bool? IsAdmin { get; set; }
        public List<UserCertification> UserCertifications { get; set; }
        public List<UserEducation> UserEducations { get; set; }
        public List<UserExperience> UserExperiences { get; set; }
        public List<UserLanguage> UserLanguages { get; set; }
        public List<UserSocialMedia> UserSocialMedias { get; set; }
    }
}
