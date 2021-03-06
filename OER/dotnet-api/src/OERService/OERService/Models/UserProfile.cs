using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OERService.Models
{
	public class UserProfile
    {
        public UserProfile()
        {
            PortalLanguage = new PortalLanguage();
        }
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
        public string Theme { get; set; }
        public List<UserCertification> UserCertifications { get; set; }
        public List<UserEducation> UserEducations { get; set; }
        public List<UserExperience> UserExperiences { get; set; }
        public List<UserLanguage> UserLanguages { get; set; }
        public List<UserSocialMedia> UserSocialMedias { get; set; }
    }

    public class CreateUserProfileRequest
    {       
        public int? TitleId { get; set; }

        [Required(ErrorMessage = "FirstName Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 250")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public int Gender { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
        public int? PortalLanguageId { get; set; }

        [Required(ErrorMessage = "DateOfBirth Required")]
        public DateTime DateOfBirth { get; set; }
        public string Photo { get; set; }
        public string ProfileDescription { get; set; }
        public string SubjectsInterested { get; set; }

        [Required(ErrorMessage = "IsContributor Required")]
        public bool IsContributor { get; set; }
        public List<CreateUserCertification> UserCertifications { get; set; }
        public List<CreateUserEducation> UserEducations { get; set; }
        public List<CreateUserExperience> UserExperiences { get; set; }
        public List<CreateUserLanguage> UserLanguages { get; set; }
        public List<CreateUserSocialMedia> UserSocialMedias { get; set; }
    }
       
    public class CreateInitialProfileRequest
    {        
        [Required(ErrorMessage = "FirstName Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 250")]
        public string FirstName { get; set; }       
        public string LastName { get; set; }
        
        [EmailAddress]
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }      
        public bool IsContributor { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class UserNotification
    {
        public int UserId { get; set; }
        public bool IsEmailNotificaiton { get; set; }
    }
    public class UpdateUserProfileRequest
    {
        [Required(ErrorMessage = "Id")]
        public int Id { get; set; }
        public int? TitleId { get; set; }

        [Required(ErrorMessage = "FirstName Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 250")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public int Gender { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
        public int? PortalLanguageId { get; set; }

        [Required(ErrorMessage = "DateOfBirth Required")]
        public DateTime DateOfBirth { get; set; }
        public string Photo { get; set; }
        public string ProfileDescription { get; set; }
        public string SubjectsInterested { get; set; }

        [Required(ErrorMessage = "IsContributor Required")]
        public bool IsContributor { get; set; }
        public List<CreateUserCertification> UserCertifications { get; set; }
        public List<CreateUserEducation> UserEducations { get; set; }
        public List<CreateUserExperience> UserExperiences { get; set; }
        public List<CreateUserLanguage> UserLanguages { get; set; }
        public List<CreateUserSocialMedia> UserSocialMedias { get; set; }
        public string EmailUrl { get; set; }
    }

    public class UserBookMarks
    {
        public int ContentType { get; set; }
        public int ContentId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
    }

    public class AddUserBookMarks
    {
        public int ContentType { get; set; }
        public int ContentId { get; set; }
        public int UserId { get; set; }
    }

    public class DeleteUserBookMarks
    {
        public int ContentType { get; set; }
        public int ContentId { get; set; }
        public int UserId { get; set; }
    }


}
