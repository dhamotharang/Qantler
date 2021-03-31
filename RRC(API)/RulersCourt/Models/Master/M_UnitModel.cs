using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_UnitModel
    {
        [DataMember(Name = "UnitID")]
        public int? UnitID { get; set; }

        [DataMember(Name = "UnitName")]
        public string UnitName { get; set; }
    }
}
