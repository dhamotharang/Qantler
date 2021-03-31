using System.Runtime.Serialization;

namespace RulersCourt.Models.HR.CVBank
{
    [DataContract]
    public class CVBankCandidateModel
    {
        [DataMember(Name = "CandidateName")]
        public string CandidateName { get; set; }
    }
}
