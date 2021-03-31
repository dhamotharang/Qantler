using System.Runtime.Serialization;

namespace RulersCourt.Models.ITSupport
{
    [DataContract]
    public class ITSupportSaveResponseModel
    {
        [DataMember(Name = "ITSupportID")]
        public int? ITSupportID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
