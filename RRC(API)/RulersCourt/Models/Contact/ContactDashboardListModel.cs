using System.Runtime.Serialization;

namespace RulersCourt.Models.Contact
{
    [DataContract]
    public class ContactDashboardListModel
    {
        [DataMember(Name = "ContactID")]
        public int? ContactID { get; set; }

        [DataMember(Name = "Department")]
        public string Department { get; set; }

        [DataMember(Name = "Section")]
        public int? Section { get; set; }

        [DataMember(Name = "Unit")]
        public int? Unit { get; set; }

        [DataMember(Name = "PhoneNumberExtension")]
        public string PhoneNumberExtension { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "EntityName")]
        public string EntityName { get; set; }

        [DataMember(Name = "Designation")]
        public string Designation { get; set; }

        [DataMember(Name = "EmailId")]
        public string EmailId { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "Photo")]
        public string Photo { get; set; }

        [DataMember(Name = "ProfilePhotoID")]
        public string ProfilePhotoID { get; set; }

        [DataMember(Name = "ProfilePhotoName")]
        public string ProfilePhotoName { get; set; }
    }
}
