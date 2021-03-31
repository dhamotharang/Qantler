using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Circular
{
    [DataContract]
    public class CircularAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "CircularID")]
        public int? CircularID { get; set; }
    }
}
