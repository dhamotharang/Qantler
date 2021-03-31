using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingReportModel
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("الموضوع")]
        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DisplayName("الموقع")]
        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DisplayName("نوع الاجتماع")]
        [DataMember(Name = "MeetingType")]
        public string MeetingType { get; set; }

        [DisplayName("المدعوون")]
        [DataMember(Name = "Invitees")]
        public string Invitees { get; set; }

        [DisplayName("اسم المستخدم")]
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
    }
}
