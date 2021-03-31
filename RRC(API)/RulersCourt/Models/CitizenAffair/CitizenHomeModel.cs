using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class CitizenHomeModel
    {
        [DataMember(Name = "New")]
        public int? New { get; set; }

        [DataMember(Name = "NeedMoreInfo")]
        public int? NeedMoreInfo { get; set; }

        [DataMember(Name = "Closed")]
        public int? Closed { get; set; }

        [DataMember(Name = "MyOwnRequest")]
        public int? MyOwnRequest { get; set; }

        [DataMember(Name = "InProgressRequest")]
        public int? InProgressRequest { get; set; }
    }
}
