using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_News
{
    [DataContract]
    public class NewsResponseModel
    {
        [DataMember(Name = "NewsID")]
        public int? NewsID { get; set; }
    }
}
