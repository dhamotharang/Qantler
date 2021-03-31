using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Photos
{
    [DataContract]
    public class M_BannerGetModel
    {
        [DataMember(Name = "BannerID")]
        public int? BannerID { get; set; }

        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentName")]
        public string AttachmentName { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
