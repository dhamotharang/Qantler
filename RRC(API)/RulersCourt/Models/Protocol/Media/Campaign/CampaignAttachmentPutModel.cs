using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Campaign
{
    [DataContract]
    public class CampaignAttachmentPutModel
    {
        [DataMember(Name = "CampaignAttachmentID")]
        public int? CampaignAttachmentID { get; set; }

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
