using System.Runtime.Serialization;

namespace RulersCourt.Models.Design
{
    [DataContract]
    public class DesignAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "DesignID")]
        public int? DesignID { get; set; }
    }
}
