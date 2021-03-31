using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_ModuleListModel
    {
        [DataMember(Name = "ModuleList")]
        public List<M_ModuleModel> ModuleList { get; set; }
    }
}