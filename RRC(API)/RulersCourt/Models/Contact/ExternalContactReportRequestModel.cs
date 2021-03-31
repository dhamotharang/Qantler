using System.Runtime.Serialization;

namespace RulersCourt.Models.Contact
{
    [DataContract]
    public class ExternalContactReportRequestModel
    {
        [DataMember(Name = "EntityName")]
        public string EntityName { get; set; }

        [DataMember(Name = "ContactName")]
        public string ContactName { get; set; }

        [DataMember(Name = "EmailId")]
        public string EmailId { get; set; }

        [DataMember(Name = "IsGovernmentEntity")]
        public int IsGovernmentEntity { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }

        [DataMember(Name = "UserID")]
        public int UserID { get; set; }
    }
}
