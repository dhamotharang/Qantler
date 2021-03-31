using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class NotificationModel
    {
        [DataMember(Name = "Collection")]
        public List<NotificationListModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public int? Count { get; set; }

        [DataMember(Name = "NotificationCount")]
        public int? NotificationCount { get; set; }
    }
}