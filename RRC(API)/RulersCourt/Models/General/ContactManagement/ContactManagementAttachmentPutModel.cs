using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.ContactManagement
{
    [DataContract]
    public class ContactManagementAttachmentPutModel
    {
        [DataMember(Name = "ContactManagmentAttachmentID")]
        public int? ContactManagmentAttachmentID { get; set; }

        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public string UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}
