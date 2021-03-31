using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class HRHomeModel
    {
        [DataMember(Name = "MyRequests")]
        public int? MyRequests { get; set; }

        [DataMember(Name = "MyPendingActions")]
        public int? MyPendingActions { get; set; }

        [DataMember(Name = "MyProcessedRequests")]
        public int? MyProcessedRequests { get; set; }

        [DataMember(Name = "LeaveRequests")]
        public int? LeaveRequests { get; set; }

        [DataMember(Name = "SalaryCertificate")]
        public int? SalaryCertificate { get; set; }

        [DataMember(Name = "ExperienceCertificate")]
        public int? ExperienceCertificate { get; set; }

        [DataMember(Name = "NewBabyAddition")]
        public int? NewBabyAddition { get; set; }

        [DataMember(Name = "AnnouncementRequests")]
        public int? AnnouncementRequests { get; set; }

        [DataMember(Name = "TrainingRequests")]
        public int? TrainingRequests { get; set; }

        [DataMember(Name = "OfficialTaskRequests")]
        public int? OfficialTaskRequests { get; set; }

        [DataMember(Name = "RaiseComplaintSuggestions")]
        public int? RaiseComplaintSuggestions { get; set; }

        [DataMember(Name = "CVBank")]
        public int? CVBank { get; set; }

        [DataMember(Name = "EmployeesProfile")]
        public int? EmployeesProfile { get; set; }
    }
}
