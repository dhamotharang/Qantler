using System.Runtime.Serialization;

namespace RulersCourt.Models.Photo
{
    [DataContract]
    public class PhotoAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "PhotoID")]
        public int? PhotoID { get; set; }
    }
}
