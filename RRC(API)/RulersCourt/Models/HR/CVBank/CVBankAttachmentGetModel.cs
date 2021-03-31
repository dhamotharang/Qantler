using System.Runtime.Serialization;

namespace RulersCourt.Models.CVBank
{
    [DataContract]
    public class CVBankAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "CVBankId")]
        public int? CVBankId { get; set; }
    }
}
