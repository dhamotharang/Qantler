using System.Runtime.Serialization;

namespace RulersCourt.Models.Protocol
{
    [DataContract]
    public class ProtocolHomeModel
    {
        [DataMember(Name = "Calendar")]
        public int? Calendar { get; set; }

        [DataMember(Name = "Gift")]
        public int? Gift { get; set; }

        [DataMember(Name = "Media")]
        public int? Media { get; set; }
    }
}
