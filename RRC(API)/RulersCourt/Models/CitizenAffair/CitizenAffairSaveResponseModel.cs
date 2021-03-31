using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class CitizenAffairSaveResponseModel
    {
        [DataMember(Name = "CitizenAffairId")]
        public int? CitizenAffairId { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
