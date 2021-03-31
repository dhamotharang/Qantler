using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class ContactModel
    {
        [DataMember(Name = "ContactID")]
        public int? ContactID { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "UserID")]
        public string UserID { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "JobTitle")]
        public string JobTitle { get; set; }

        [DataMember(Name = "SocialStatus")]
        public string SocialStatus { get; set; }

        [DataMember(Name = "Photos")]
        public string Photos { get; set; }

        [DataMember(Name = "DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [DataMember(Name = "ContactType")]
        public string ContactType { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Phone")]
        public string Phone { get; set; }

        [DataMember(Name = "OfficialEntity")]
        public string OfficialEntity { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }

        [DataMember(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public string UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}