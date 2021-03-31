using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HR.CVBank
{
    [DataContract]
    public class CVBankReportRequestModel
    {
        [DataMember(Name = "CandidateName")]
        public string CandidateName { get; set; }

        [DataMember(Name = "Specializations")]
        public string Specializations { get; set; }

        [DataMember(Name = "EducationalQualification")]
        public string EducationalQualification { get; set; }

        [DataMember(Name = "YearsofExperience")]
        public string YearsofExperience { get; set; }

        [DataMember(Name = "CountryofResidence")]
        public string CountryofResidence { get; set; }

        [DataMember(Name = "DateFrom")]
        public DateTime? DateFrom { get; set; }

        [DataMember(Name = "DateTo")]
        public DateTime? DateTo { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }

        [DataMember(Name = "UserID ")]
        public int? UserID { get; set; }
    }
}
