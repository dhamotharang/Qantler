using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarBulkApprovalModel
    {
        [DataMember(Name = "CalendarID")]
        public List<CalendarBulkID> CalendarID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "ActionBy")]
        public int? ActionBy { get; set; }

        [DataMember(Name = "ActionDateTime")]
        public DateTime ActionDateTime { get; set; }

        [DataMember(Name = "IsApologySent")]
        public bool? IsApologySent { get; set; }
    }
}
