using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.ContactManagement
{
    [DataContract]
    public class ContactManagementPutModel
    {
        [DataMember(Name = "ContactManagementID")]
        public int? ContactManagementID { get; set; }

        [DataMember(Name = "Department")]
        public string Department { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "EntityName")]
        public string EntityName { get; set; }

        [DataMember(Name = "EmailId")]
        public string EmailId { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "OfficialEntity")]
        public bool OfficialEntity { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Attachments")]
        public List<ContactManagementAttachmentGetModel> Attachments { get; set; }
    }
}
