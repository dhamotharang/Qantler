using System.Runtime.Serialization;

namespace RulersCourt.Models.Maintenance
{
    [DataContract]
    public class MaintenanceHomeCountModel
    {
        [DataMember(Name = "New")]
        public int? New { get; set; }

        [DataMember(Name = "NeedMoreInfo")]
        public int? NeedMoreInfo { get; set; }

        [DataMember(Name = "Closed")]
        public int? Closed { get; set; }

        [DataMember(Name = "MyOwnRequest")]
        public int? MyOwnRequest { get; set; }

        [DataMember(Name = "MyPendingRequest")]
        public int? MyPendingRequest { get; set; }

        [DataMember(Name = "MyProcessedRequest")]
        public int? MyProcessedRequest { get; set; }

        [DataMember(Name = "InProgressRequest")]
        public int? InProgressRequest { get; set; }
    }
}
