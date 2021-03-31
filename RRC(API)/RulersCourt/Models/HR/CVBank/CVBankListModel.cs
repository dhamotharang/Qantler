using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HR.CVBank
{
    [DataContract]
    public class CVBankListModel
    {
        [DataMember(Name = "Collection")]
        public List<CVBankDashBoardListModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "CandidateName")]
        public List<CVBankCandidateModel> CandidateName { get; set; }
    }
}
