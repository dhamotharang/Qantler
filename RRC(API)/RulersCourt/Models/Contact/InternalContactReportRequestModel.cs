using System.Runtime.Serialization;

namespace RulersCourt.Models.Contact
{
    [DataContract]
    public class InternalContactReportRequestModel
    {
        [DataMember(Name = "Department")]
        public string Department { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "Designation")]
        public string Designation { get; set; }

        [DataMember(Name = "EmailId")]
        public string EmailId { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }

        [DataMember(Name = "UserID ")]
        public int UserID { get; set; }
    }
}
