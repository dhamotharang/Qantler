using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarcountModel
    {
        [DataMember(Name = "MyEvents")]
        public int? MyEvents { get; set; }

        [DataMember(Name = "MyPendingRequest")]
        public int? MyPendingRequest { get; set; }

        [DataMember(Name = "Approved")]
        public int? Approved { get; set; }

        [DataMember(Name = "AllEvents")]
        public int? AllEvents { get; set; }
    }
}
