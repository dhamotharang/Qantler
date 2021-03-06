using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    public class CitizenAffairPersonalReportPreviewModel
    {
        [DataMember(Name = "CitizenAffairID")]
        public int? CitizenAffairID { get; set; }

        [DataMember(Name = "ProfilePhotoID")]
        public string ProfilePhotoID { get; set; }

        [DataMember(Name = "PersonalProfileName")]
        public string PersonalProfileName { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Employer")]
        public string Employer { get; set; }

        [DataMember(Name = "Destination")]
        public string Destination { get; set; }

        [DataMember(Name = "MonthlySalary")]
        public string MonthlySalary { get; set; }

        [DataMember(Name = "EmiratesID")]
        public string EmiratesID { get; set; }

        [DataMember(Name = "MaritalStatus")]
        public string MaritalStatus { get; set; }

        [DataMember(Name = "NoOfChildrens")]
        public string NoOfChildrens { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "Emirates")]
        public string Emirates { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "Age")]
        public string Age { get; set; }

        [DataMember(Name = "ReportObjectives")]
        public string ReportObjectives { get; set; }

        [DataMember(Name = "FindingNotes")]
        public string FindingNotes { get; set; }

        [DataMember(Name = "Recommendation")]
        public string Recommendation { get; set; }
    }
}
