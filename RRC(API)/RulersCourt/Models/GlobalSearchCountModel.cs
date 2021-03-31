using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class GlobalSearchCountModel
    {
        [DataMember(Name = "Memos")]
        public int? Memos { get; set; }

        [DataMember(Name = "Letters")]
        public int? Letters { get; set; }

        [DataMember(Name = "DutyTask")]
        public int? DutyTask { get; set; }

        [DataMember(Name = "Meetings")]
        public int? Meetings { get; set; }

        [DataMember(Name = "Circulars")]
        public int? Circulars { get; set; }

        [DataMember(Name = "Legal")]
        public int? Legal { get; set; }

        [DataMember(Name = "Protocol")]
        public int? Protocol { get; set; }

        [DataMember(Name = "HR")]
        public int? HR { get; set; }

        [DataMember(Name = "CitizenAffair")]
        public int? CitizenAffair { get; set; }

        [DataMember(Name = "Maintenance")]
        public int? Maintenance { get; set; }

        [DataMember(Name = "IT")]
        public int? IT { get; set; }
    }
}