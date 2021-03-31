using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.ContactManagement
{
    [DataContract]
    public class ContactManagementSaveResponseModel
    {
        [DataMember(Name = "ContactManagemnetID")]
        public int? ContactManagemnetID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }
    }
}
