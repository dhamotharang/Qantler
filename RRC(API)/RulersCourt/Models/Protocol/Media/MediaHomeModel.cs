using System.Runtime.Serialization;

namespace RulersCourt.Models.Protocol.Media
{
    [DataContract]
    public class MediaHomeModel
    {
        [DataMember(Name = "RequestforPhoto")]
        public int? RequestforPhoto { get; set; }

        [DataMember(Name = "RequestforDesign")]
        public int? RequestforDesign { get; set; }

        [DataMember(Name = "RequestforPressRelease")]
        public int? RequestforPressRelease { get; set; }

        [DataMember(Name = "RequestforCampaign")]
        public int? RequestforCampaign { get; set; }

        [DataMember(Name = "RequestforPhotographer")]
        public int? RequestforPhotographer { get; set; }

        [DataMember(Name = "RequesttouseDiwanIdentity")]
        public int? RequesttouseDiwanIdentity { get; set; }

        [DataMember(Name = "MyPendingRequest")]
        public int? MyPendingRequest { get; set; }

        [DataMember(Name = "MyOwnRequest")]
        public int? MyOwnRequest { get; set; }

        [DataMember(Name = "MyProcessedRequest")]
        public int? MyProcessedRequest { get; set; }
    }
}
