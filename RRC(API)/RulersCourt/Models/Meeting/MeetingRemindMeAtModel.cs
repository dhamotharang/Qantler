using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingRemindMeAtModel
    {
        [DataMember(Name = "MeetingRemindID")]
        public int? MeetingRemindID { get; set; }

        [DataMember(Name = "RemindMeDateTime")]
        public DateTime? RemindMeDateTime { get; set; }
    }
}
