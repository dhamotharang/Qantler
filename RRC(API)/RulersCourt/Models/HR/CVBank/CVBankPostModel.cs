using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CVBank
{
    [DataContract]
    public class CVBankPostModel
    {
        [DataMember(Name = "CandidateName")]
        public string CandidateName { get; set; }

        [DataMember(Name = "EmailId")]
        public string EmailId { get; set; }

        [DataMember(Name = "JobTitle")]
        public string JobTitle { get; set; }

        [DataMember(Name = "Specialization")]
        public int? Specialization { get; set; }

        [DataMember(Name = "EducationalQualification")]
        public int? EducationalQualification { get; set; }

        [DataMember(Name = "Gender")]
        public string Gender { get; set; }

        [DataMember(Name = "YearsofExperience")]
        public string YearsofExperience { get; set; }

        [DataMember(Name = "AreaofExpertise")]
        public string AreaofExpertise { get; set; }

        [DataMember(Name = "CountryofResidence")]
        public int? CountryofResidence { get; set; }

        [DataMember(Name = "CityofResidence")]
        public int? CityofResidence { get; set; }

        [DataMember(Name = "Address")]
        public string Address { get; set; }

        [DataMember(Name = "Attachments")]
        public List<CVBankAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
