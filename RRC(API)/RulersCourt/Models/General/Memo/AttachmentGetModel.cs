using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class AttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "MemoID")]
        public string MemoID { get; set; }
    }
}
