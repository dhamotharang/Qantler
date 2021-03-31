using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Contact
{
    [DataContract]
    public class ContactAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "ContactID")]
        public int? ContactID { get; set; }
    }
}
