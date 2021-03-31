using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskReportListModel
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "TaskReferenceNumber")]
        public string TaskReferenceNumber { get; set; }

        [DisplayName("الموضوع")]
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DisplayName("المنشئ")]
        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DisplayName("المكلف")]
        [DataMember(Name = "Assignee")]
        public string Assignee { get; set; }

        [DisplayName("المشاركون في المهام")]
        [DataMember(Name = "Participations")]
        public string Participations { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("الأولويات")]
        [DataMember(Name = "Priority")]
        public string Priority { get; set; }
    }
}
