using System;

namespace RulersCourt.Models.CitizenAffair
{
    public class CitizenAffairReportRequestModel
    {
        public string Status { get; set; }

        public string RequestType { get; set; }

        public string ReferenceNumber { get; set; }

        public DateTime? RequestDateRangeFrom { get; set; }

        public DateTime? RequestDateRangeTo { get; set; }

        public string PersonalLocationName { get; set; }

        public string PhoneNumber { get; set; }

        public string SmartSearch { get; set; }

        public int UserID { get; set; }

        public int? Sourcename { get; set; }
    }
}
