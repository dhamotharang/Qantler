using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class CitizenAffairAttachmentPutModel
    {
        [DataMember(Name = "CitizenAffairAttachmentID")]
        public int? CitizenAffairAttachmentID { get; set; }

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
