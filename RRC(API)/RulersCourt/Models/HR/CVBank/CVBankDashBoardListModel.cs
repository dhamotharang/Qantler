using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HR.CVBank
{
    [DataContract]
    public class CVBankDashBoardListModel
    {
        [DataMember(Name = "CVBankId")]
        public int? CVBankId { get; set; }

        [DataMember(Name = "CandidateName")]
        public string CandidateName { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Position")]
        public string Position { get; set; }

        [DataMember(Name = "YearsofExperience")]
        public string YearsofExperience { get; set; }

        [DataMember(Name = "Specialization")]
        public string Specialization { get; set; }

        [DataMember(Name = "Date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "CountryofResidence")]
        public string CountryofResidence { get; set; }
    }
}
