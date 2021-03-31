using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HR.CVBank
{
    [DataContract]
    public class CVBankReport
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("اسم المستخدم")]
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DisplayName("اسم المرشح")]
        [DataMember(Name = "CandidateName")]
        public string CandidateName { get; set; }

        [DisplayName("البريد الالكتروني")]
        [DataMember(Name = "EmailId")]
        public string EmailId { get; set; }

        [DisplayName("المنصب")]
        [DataMember(Name = "JobTitle")]
        public string JobTitle { get; set; }

        [DisplayName("التخصص")]
        [DataMember(Name = "Specializations")]
        public string Specializations { get; set; }

        [DisplayName("المؤهل العلمي")]
        [DataMember(Name = "EducationalQualification")]
        public string EducationalQualification { get; set; }

        [DisplayName("سنوات الخبرة")]
        [DataMember(Name = "YearsofExperience")]
        public string YearsofExperience { get; set; }

        [DisplayName("بلد")]
        [DataMember(Name = "Country")]
        public string Country { get; set; }
    }
}
