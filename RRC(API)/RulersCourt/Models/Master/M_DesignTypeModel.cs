using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_DesignTypeModel
    {
        [DataMember(Name = "DesignTypeID")]
        public int? DesignTypeID { get; set; }

        [DataMember(Name = "DesignTypeName")]
        public string DesignTypeName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}
