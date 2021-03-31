using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class GlobalSearchListModel
    {
        [DataMember(Name = "Collection")]
        public List<GlobalSearchModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public int? Count { get; set; }

        [DataMember(Name = "ModulesCount")]
        public GlobalSearchCountModel ModulesCount { get; set; }

        [DataMember(Name = "ModuleList")]
        public List<M_ModuleModel> ModuleList { get; set; }
    }
}
