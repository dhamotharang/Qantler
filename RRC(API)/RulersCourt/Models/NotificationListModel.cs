using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class NotificationListModel
    {
        [DataMember(Name = "ID")]
        public int? ID { get; set; }

        [DataMember(Name = "ServiceID")]
        public int? ServiceID { get; set; }

        [DataMember(Name = "Service")]
        public string Service { get; set; }

        [DataMember(Name = "Process")]
        public string Process { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "IsRead")]
        public bool? IsRead { get; set; }

        [DataMember(Name = "LastUpdateDatetime")]
        public DateTime? LastUpdateDatetime { get; set; }

        [DataMember(Name = "FromName")]
        public string FromName { get; set; }

        [DataMember(Name = "IsAnonymous")]
        public bool? IsAnonymous { get; set; }
    }
}