using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HR
{
    [DataContract]
    public class HRHomeListModel
    {
        [DataMember(Name = "Collection")]
        public List<HRHomeDashboardListModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> LookupsList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }
    }
}
