using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Photos
{
    [DataContract]
    public class PhotoResponseModel
    {
        [DataMember(Name = "PhotoID")]
        public int? PhotoID { get; set; }
    }
}
