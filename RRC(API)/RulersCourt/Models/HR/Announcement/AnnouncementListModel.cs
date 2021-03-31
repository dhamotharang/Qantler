using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Announcement
{
    public class AnnouncementListModel
    {
        [DataMember(Name = "Collection")]
        public List<AnnouncementGetModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}
