using System.Runtime.Serialization;

namespace RulersCourt.Models.BabyAddition
{
    [DataContract]
    public class BabyAdditionAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "BabyAdditionID")]
        public int? BabyAdditionID { get; set; }
    }
}
