using System.Runtime.Serialization;

namespace RulersCourt.Models.PressRelease
{
    [DataContract]
    public class PressReleaseSaveResponseModel
    {
        [DataMember(Name = "PressReleaseID")]
        public int? PressReleaseID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
