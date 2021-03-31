using System.Runtime.Serialization;

namespace RulersCourt.Models.Gift
{
    [DataContract]
    public class GiftAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "GiftID")]
        public int? GiftID { get; set; }
    }
}
