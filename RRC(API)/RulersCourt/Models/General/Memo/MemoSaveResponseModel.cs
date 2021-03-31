using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class MemoSaveResponseModel
    {
        [DataMember(Name = "MemoId")]
        public int? MemoId { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
