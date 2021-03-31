using System.Runtime.Serialization;

namespace RulersCourt.Models.Photo
{
    [DataContract]
    public class PhotoSaveResponseModel
    {
        [DataMember(Name = "PhotoID")]
        public int? PhotoID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
