using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficialTaskCompensation.OfficialTask
{
    [DataContract]
    public class OfficialTaskReportRequestModel
    {
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SubjectName")]
        public string SubjectName { get; set; }

        [DataMember(Name = "AttendedBy")]
        public string AttendedBy { get; set; }

        [DataMember(Name = "OfficialTaskType")]
        public string OfficialTaskType { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }

        [DataMember(Name = "UserID")]
        public int UserID { get; set; }
    }
}
