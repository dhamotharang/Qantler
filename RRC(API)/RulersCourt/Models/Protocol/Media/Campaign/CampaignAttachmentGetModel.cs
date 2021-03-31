using System.Runtime.Serialization;

namespace RulersCourt.Models.Campaign
{
    [DataContract]
    public class CampaignAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "CampaignID")]
        public int? CampaignID { get; set; }
    }
}
