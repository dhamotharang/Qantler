using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class GlobalSearchModel
    {
        [DataMember(Name = "ID")]
        public int? ID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "Type")]
        public int? Type { get; set; }

        [DataMember(Name = "CanEdit")]
        public bool? CanEdit { get; set; }
    }
}