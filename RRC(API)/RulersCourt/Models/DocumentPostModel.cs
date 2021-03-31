using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class DocumentPostModel
    {
        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
