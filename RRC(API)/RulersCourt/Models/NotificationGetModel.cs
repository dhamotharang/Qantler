using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class NotificationGetModel
    {
        [DataMember(Name = "ID")]
        public int? ID { get; set; }

        [DataMember(Name = "ServiceID")]
        public int? ServiceID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "ToName")]
        public string ToName { get; set; }

        [DataMember(Name = "Service")]
        public string Service { get; set; }

        [DataMember(Name = "EnFromName")]
        public string EnFromName { get; set; }

        [DataMember(Name = "ArFromName")]
        public string ArFromName { get; set; }

        [DataMember(Name = "DelegateToName")]
        public string DelegateToName { get; set; }

        [DataMember(Name = "EnDelegateFromName")]
        public string EnDelegateFromName { get; set; }

        [DataMember(Name = "ArDelegateFromName")]
        public string ArDelegateFromName { get; set; }

        [DataMember(Name = "Process")]
        public string Process { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "PlateNumber")]
        public string PlateNumber { get; set; }

        [DataMember(Name = "Content")]
        public string Content { get; set; }

        [DataMember(Name = "IsAnonymous")]
        public bool? IsAnonymous { get; set; }
    }
}