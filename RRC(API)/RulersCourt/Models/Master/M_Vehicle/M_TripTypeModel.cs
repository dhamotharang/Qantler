using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Vehicle
{
    [DataContract]
    public class M_TripTypeModel
    {
        [DataMember(Name = "TripTypeID")]
        public int? TripTypeID { get; set; }

        [DataMember(Name = "TripTypeName")]
        public string TripTypeName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}
