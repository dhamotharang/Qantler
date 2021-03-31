using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Vehicle
{
    [DataContract]
    public class M_LogTypeModel
    {
        [DataMember(Name = "LogTypeID")]
        public int? LogTypeID { get; set; }

        [DataMember(Name = "LogTypeName")]
        public string LogTypeName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}
