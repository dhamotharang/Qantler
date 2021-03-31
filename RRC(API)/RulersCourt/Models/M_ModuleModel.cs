using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_ModuleModel
    {
        [DataMember(Name = "ModuleID")]
        public int? ModuleID { get; set; }

        [DataMember(Name = "ModuleName")]
        public string ModuleName { get; set; }
    }
}