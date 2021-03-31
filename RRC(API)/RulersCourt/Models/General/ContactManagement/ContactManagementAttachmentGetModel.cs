using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.ContactManagement
{
    [DataContract]
    public class ContactManagementAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "ContactManagementID")]
        public int? ContactManagementID { get; set; }
    }
}
