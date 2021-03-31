using System.Runtime.Serialization;

namespace RulersCourt.Models.UserProfile
{
    [DataContract]
    public class UserProfileAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentType")]
        public string AttachmentType { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "UserProfileId")]
        public int? UserProfileId { get; set; }
    }
}
