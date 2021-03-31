using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_MediaChannelModel
    {
        [DataMember(Name = "MediaChannelID")]
        public int? MediaChannelID { get; set; }

        [DataMember(Name = "MediaChannelName")]
        public string MediaChannelName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}