using System.Runtime.Serialization;

namespace RulersCourt.Models.Design
{
    [DataContract]
    public class DesignSaveResponseModel
    {
        [DataMember(Name = "DesignID")]
        public int? DesignID { get; set; }

        [DataMember(Name = "ReferenceNumber")]        public string ReferenceNumber { get; set; }        [DataMember(Name = "status")]        public int Status { get; set; }
    }
}
