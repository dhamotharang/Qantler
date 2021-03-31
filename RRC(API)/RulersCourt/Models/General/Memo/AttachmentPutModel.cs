using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class AttachmentPutModel
    {
        [DataMember(Name = "MemoAttachmentID")]
        public int? MemoAttachmentID { get; set; }

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
