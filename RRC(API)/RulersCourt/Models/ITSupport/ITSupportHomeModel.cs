using System.Runtime.Serialization;

namespace RulersCourt.Models.ITSupport
{
    [DataContract]
    public class ITSupportHomeModel
    {
        [DataMember(Name = "ServicesNew")]
        public int? ServicesNew { get; set; }

        [DataMember(Name = "ServicesInProgress")]
        public int? ServicesInProgress { get; set; }

        [DataMember(Name = "ServicesClose")]
        public int? ServicesClose { get; set; }

        [DataMember(Name = "SupportNew")]
        public int? SupportNew { get; set; }

        [DataMember(Name = "SupportInprogress")]
        public int? SupportInprogress { get; set; }

        [DataMember(Name = "SupportClose")]
        public int? SupportClose { get; set; }

        [DataMember(Name = "ComponentsNew")]
        public int? ComponentsNew { get; set; }

        [DataMember(Name = "ComponentsInprogress")]
        public int? ComponentsInprogress { get; set; }

        [DataMember(Name = "ComponentsClose")]
        public int? ComponentsClose { get; set; }

        [DataMember(Name = "MyClosedRequest")]
        public int? MyClosedRequest { get; set; }

        [DataMember(Name = "MyOwnRequest")]
        public int? MyOwnRequest { get; set; }
    }
}
