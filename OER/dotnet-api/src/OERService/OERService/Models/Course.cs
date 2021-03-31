using System;
using System.Collections.Generic;

namespace OERService.Models
{
	public class CourseMaster
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Thumbnail { get; set; }
        public string CourseDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public int? CopyRightId { get; set; }
        public bool IsDraft { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public double Rating { get; set; }
        public int ReportAbuseCount { get; set; }
        public int? EducationId { get; set; }
        public int? ProfessionId { get; set; }
    }

    public class CourseFileInfo
    {
      public List<CourseFiles> CourseFiles { get; set; }
      public List<ResourceFiles> ResourceFiles { get; set; }
    }
    public class CourseFiles
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public string AssociatedFile { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FileName { get; set; }
    }
    public class ResourceFiles
    {
        public decimal Id { get; set; }
        public decimal ResourceId { get; set; }
        public string AssociatedFile { get; set; }
        public DateTime UploadedDate { get; set; }
        public string FileName { get; set; }
    }
    public class Course
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public ShortCategory Category { get; set; }
        public ShortSubCategory SubCategory { get; set; }
        public string Thumbnail { get; set; }
        public string CourseDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public ShortCopyright CopyRight { get; set; }
        public Education Education { get; set; }
        public Profession Profession { get; set; }
        public bool? IsDraft { get; set; }
        public string CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsApproved { get; set; }
        public double Rating { get; set; }
        public List<Rating> AllRatings { get; set; }
        public int? ReportAbuseCount { get; set; }
        public int? ReadingTime { get; set; }
        public Int64 Rownumber { get; set; }
        public Int64 TotalRows { get; set; }
        public int? ViewCount { get; set; }
        public int? DownloadCount { get; set; }
        public int? SharedCount { get; set; }
        public int? EnrollmentCount { get; set; }
        public List<CourseUrlReferences> References { get; set; }
        public List<CourseAssociatedFiles> AssociatedFiles { get; set; }
        public List<CourseComment> CourseComments { get; set; }
        public List<CourseSection> CourseSection { get; set; }
        public DateTime? LastView { get; internal set; }
        public EducationalStandard EducationalStandard { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public EducationalUse EducationalUse { get; set; }
        public string Objective { get; set; }
        public bool? CommunityBadge { get; set; }
        public bool? MoEBadge { get; set; }
        
    }

    public class CourseSection
    {
        public string Name { get; set; }
        public List<Resource> courseResources { get; set; }

    }
    public class Section
    {
        public string Name { get; set; }
        public List<CourseResource> courseResources { get; set; }

    }
    public class CourseResources
    {
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
  
    }
    public class  CourseResource
    {
        public int ResourceId { get; set; }
        public string SectionName { get; set; }
    }
    public class SharedContentInfoCreate
    {
        public int ContentId { get; set; }
        public int ContentTypeId { get; set; }
        public string SocialMediaName { get; set; }
        public int CreatedBy { get; set; }
    }
    public class DownloadContentInfoCreate
    {
        public int ContentId { get; set; }
        public int DownloadedBy { get; set; }
    }
    public class CreateCourseRequest
    {       
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Thumbnail { get; set; }
        public string CourseDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public int? CopyRightId { get; set; }
        public bool IsDraft { get; set; } 
        public int? EducationId { get; set; }
        public int? ProfessionId { get; set; }
        public int CreatedBy { get; set; }
        public int? ReadingTime { get; set; }
        public List<Reference> References { get; set; }
        public List<ResourceFile> ResourceFiles { get; set; }
        public string ResourcesIds { get; set; }
        public List<Test> Tests { get; set; }
        public string EmailUrl { get; set; }
        public List<Section> sections { get; set; }
        public int? LevelId { get; set; }
        public int? EducationalStandardId { get; set; }
        public int? EducationalUseId { get; set; }

    }
    public class CourseEnrollmentStatus
    {
        public int Id { get; set; }
        public bool Active { get; set; }
    }
    public class CourseEnrollmentCreate
    {
        public decimal CourseId { get; set; }
        public int UserId { get; set; }
    }
    public class Test
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public List<Questions> Questions { get; set; }
        public int CourseId { get; set; }
    }
    public class CourseTestsResponse
    {
       public List<Test> tests { get; set; }
       public List<Questions> questions  { get; set; }
       public List<AnswerOptions> answers { get; set; }
    }
    public class Questions
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public int TestId { get; set; }
        public List<AnswerOptions> AnswerOptions { get; set; }
        public string Media { get; set; }
        public string FileName { get; set; }
    }
    public class AnswerOptions
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; }
        public bool CorrectAnswer { get; set; }

    }
    public class Answers
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
       

    }
    public class UserAnswersOptions
    {
        public List<Answers> answers { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
    public class UpdateCourseRequest
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Thumbnail { get; set; }
        public string CourseDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public int? CopyRightId { get; set; }
        public bool IsDraft { get; set; }
        public int? EducationId { get; set; }
        public int? ProfessionId { get; set; }
        public int CreatedBy { get; set; }
        public List<Reference> References { get; set; }
        public List<ResourceFile> ResourceFiles { get; set; }
        public int? ReadingTime { get; set; }
        public int? LevelId { get; set; }
        public int? EducationalStandardId { get; set; }
        public int? EducationalUseId { get; set; }
        public List<Section> sections { get; set; }
        public List<Test> Tests { get; set; }

    }
    public class CourseUrlReferences
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public string URLReference { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
    }

    public class CourseApprovals
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
    }

    public class CourseAssociatedFiles
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public string AssociatedFile { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CourseComments
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }
        public DateTime CommentDate { get; set; }
        public int ReportAbuseCount { get; set; }
    }

}
