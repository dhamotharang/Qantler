using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OERService.Models
{
	public class ResourceMaster
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Thumbnail { get; set; }
        public string ProfileDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public int MaterialTypeId { get; set; }
        public int? CopyRightId { get; set; }
        public bool IsDraft { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public double Rating { get; set; }
        public double AlignmentRating { get; set; }
        public int ReportAbuseCount { get; set; }
    }

    public class Resource
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public ShortCategory Category { get; set; }
        public ShortSubCategory SubCategory { get; set; }
        public string Thumbnail { get; set; }
        public string ResourceDescription { get; set; }
        public string Keywords { get; set; }
        public string ResourceContent { get; set; }
        public ShortMaterialType MaterialType { get; set; }
        public ShortCopyright CopyRight { get; set; }
        public bool IsDraft { get; set; }
        public string CreatedBy { get; set; }
        public int CreatedById { get; set; }        
        public DateTime CreatedOn { get; set; }
        public bool? IsApproved { get; set; }
        public double Rating { get; set; }
        public List<Rating> AllRatings { get; set; }
        public double AlignmentRating { get; set; }
        public int? ReportAbuseCount { get; set; }
        public Boolean IsRemix { get; set; }
        public string Standard { get; set; }
        public string Objective { get; set; }
        public string Format { get; set; }
        public List<ReferenceMaster> References { get; set; }
        public List<ResourceFileMaster> ResourceFiles { get; set; }
        public int? ReadingTime { get; set; }
        public List<ResourceComments> ResourceComments { get; set; }
        public Int64 TotalRows { get; set; }
        public Int64 Rownumber { get; set; }
        public int? SharedCount { get; set; }
        public int? ViewCount { get; set; }
        public int? DownloadCount { get; set; }
        public DateTime? LastView { get;  set; }
        public EducationalStandard EducationalStandard { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public EducationalUse EducationalUse { get; set; }
        public bool? CommunityBadge { get; set; }
        public bool? MoEBadge { get; set; }
    }
    

    public class ResourceMasterData
    {
        public List<EducationalStandard> EducationalStandard { get; set; }
        public List<EducationalUse> EducationalUse { get; set; }
        public List<Level> Level { get; set; }
        public List<CategoryMasterData> CategoryMasterData { get; set; }
        public List<SubCategoryMasterData> SubCategoryMasterData { get; set; }
        public List<MaterialTypeMasterData> MaterialTypeMasterData { get; set; }
        public List<EducationMasterData> EducationMasterData { get; set; }
        public List<ProfessionMasterData> ProfessionMasterData { get; set; }
        public List<CopyrightMasterData> CopyrightMasterData { get; set; }
    }
    public class CategoryMasterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }

    }
    public class SubCategoryMasterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
    public class MaterialTypeMasterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }

    }
    public class EducationMasterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
    }
    public class ProfessionMasterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
    }
    public class CopyrightMasterData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Title_Ar { get; set; }
        public string Description_Ar { get; set; }
        public string Media { get; set; }
    }
    public class EducationalStandard
    {
        public int? Id { get; set; }
        public string Standard { get; set; }
        public string Standard_Ar { get; set; }
    }
    public class EducationalUse
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public string Text_Ar { get; set; }
    }
    public class Level
    {
        public int? Id { get; set; }
        public string LevelText { get; set; }
        public string LevelText_Ar { get; set; }
    }
    public class CreateResourceRequest
    {

        [Required(ErrorMessage = "Title Required")]
        [MaxLength(250, ErrorMessage = "Maximum length 250")]
        public string Title { get; set; }

        [Required(ErrorMessage = "CategoryId Required")]
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public string Thumbnail { get; set; }

        [MaxLength(2000, ErrorMessage = "Maximum length 250")]
        public string ResourceDescription { get; set; }

        [MaxLength(1500, ErrorMessage = "Maximum length 250")]
        public string Keywords { get; set; }       
        public string ResourceContent { get; set; }

        public int? MaterialTypeId { get; set; }
        public int? CopyRightId { get; set; }

        [Required(ErrorMessage = "IsDraft Required")]
        public bool IsDraft { get; set; }

        [Required(ErrorMessage = "CreatedBy Required")]
        public int CreatedBy { get; set; }

        public List<Reference> References { get; set; }

        public List<ResourceFile> ResourceFiles { get; set; }        
        public int? ReadingTime { get; set; }
        public string EmailUrl { get; set; }
        public int? ResourceSourceId { get; set; }
        public int? LevelId { get; set; }
        public int? EducationalStandardId { get; set; }
        public int? EducationalUseId { get; set; }
        public string Format { get; set; }
        public string Objective { get; set; }
    }

    public class UpdateResourceRequest
    {
        [Required(ErrorMessage = "Id Required")]
        public decimal Id { get; set; }

        [Required(ErrorMessage = "Title Required")]
        [MaxLength(250, ErrorMessage = "Maximum length 250")]
        public string Title { get; set; }

        [Required(ErrorMessage = "CategoryId Required")]
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public string Thumbnail { get; set; }

        [MaxLength(2000, ErrorMessage = "Maximum length 250")]
        public string ResourceDescription { get; set; }

        [MaxLength(1500, ErrorMessage = "Maximum length 250")]
        public string Keywords { get; set; }
        public string ResourceContent { get; set; }

        //[Required(ErrorMessage = "MaterialTypeId Required")]
        public int? MaterialTypeId { get; set; }
        public int? CopyRightId { get; set; }

        [Required(ErrorMessage = "IsDraft Required")]
        public bool IsDraft { get; set; }

        [Required(ErrorMessage = "CreatedBy Required")]
        public int CreatedBy { get; set; }
        public int? LevelId { get; set; }
        public int? EducationalStandardId { get; set; }
        public int? EducationalUseId { get; set; }
        public string Format { get; set; }
        public string Objective { get; set; }
        public List<Reference> References { get; set; }
        public List<ResourceFile> ResourceFiles { get; set; }
        public int? ReadingTime { get; set; }
    }

    public class ReferenceMaster
    {
        public decimal Id { get; set; }       
        public decimal ResourceId { get; set; }
        public string URLReference { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }

    }
    public class ResourceFileMaster
    {
        public decimal Id { get; set; }
        public decimal ResourceId { get; set; }
        public string AssociatedFile { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedDate { get; set; }

    }

    public class Reference
    {
        [Required(ErrorMessage = "URLReferenceId Required")]     
        public int URLReferenceId { get; set; }
    }
    public class ContentFiles
    {
        public decimal Id { get; set; }
        public decimal ContentId { get; set; }
        public string AssociatedFiles { get; set; }
        public string FileName { get; set; }

    }
    public class ResourceFile
    {
        [Required(ErrorMessage = "AssociatedFile Required")]
        [MaxLength(200, ErrorMessage = "Maximum length 200")]
        public string AssociatedFile { get; set; }
        public string FileName { get; set; }
    }

    public class ResourceApprovals
    {
        public int Id { get; set; }
        public decimal ResourceId { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
    }
    public class ResourceComments
    {
        public decimal Id { get; set; }
        public decimal ResourceId { get; set; }
        public string Comments { get; set; }
        public string CommentedBy { get; set; }
        public int CommentedById { get; set; }
        public string CommentorImage { get; set; }
        public DateTime CommentDate { get; set; }
        public int ReportAbuseCount { get; set; }
    }

     public class DownloadContentInfoCreateResource
    {
        public int ContentId { get; set; }
        public int DownloadedBy { get; set; }
    }

    public class Rating
    {
        public double Star { get; set; }
        public int UserCount { get; set; }
    }

    public class RatingRequest
    {
        public int ContentType { get; set; }
        public int ContentId { get; set; }
    }

    public class RatingResponse
    {
        public int ContentType { get; set; }
        public int ContentId { get; set; }
        public decimal? Rating { get; set; }
        public List<Rating> AllRatings { get; set; }
    }

}
