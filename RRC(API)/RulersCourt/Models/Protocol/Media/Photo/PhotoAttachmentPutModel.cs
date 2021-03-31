using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Photo
{
    [DataContract]
    public class PhotoAttachmentPutModel
    {
        [DataMember(Name = "PhotoAttachmentID")]
        public int? PhotoAttachmentID { get; set; }

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
