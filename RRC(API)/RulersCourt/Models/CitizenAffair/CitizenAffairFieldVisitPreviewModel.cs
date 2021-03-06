using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    public class CitizenAffairFieldVisitPreviewModel
    {
        [DataMember(Name = "CitizenAffairID")]
        public int? CitizenAffairID { get; set; }

        [DataMember(Name = "Date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "RequetsedBy")]
        public string RequetsedBy { get; set; }

        [DataMember(Name = "VisitObjective")]
        public string VisitObjective { get; set; }

        [DataMember(Name = "FindingNotes")]
        public string FindingNotes { get; set; }

        [DataMember(Name = "ForWhom")]
        public string ForWhom { get; set; }

        [DataMember(Name = "EmiratesID")]
        public string EmiratesID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "LocationName")]
        public string LocationName { get; set; }

        [DataMember(Name = "LocationID")]
        public int? LocationID { get; set; }

        [DataMember(Name = "LocationEmirites")]
        public string LocationEmirites { get; set; }
    }
}
