using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Photos
{
    [DataContract]
    public class M_PhotoGetModel
    {
        [DataMember(Name = "PhotoID")]
        public int? PhotoID { get; set; }

        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentName")]
        public string AttachmentName { get; set; }

        [DataMember(Name = "ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}
