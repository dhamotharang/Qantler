using System.Runtime.Serialization;

namespace RulersCourt.Models.DiwanIdentity
{
    [DataContract]
    public class DiwanIdentitySaveResponseModel
    {
        [DataMember(Name = "DiwanIdentityID")]
        public int? DiwanIdentityID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
