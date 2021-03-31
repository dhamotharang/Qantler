using System;

namespace RulersCourt.Models.Legal
{
    public class LegalReportRequestModel
    {
        public string Status { get; set; }

        public string SourceOU { get; set; }

        public string Subject { get; set; }

        public DateTime? RequestDateFrom { get; set; }

        public DateTime? RequestDateTo { get; set; }

        public string Label { get; set; }

        public string AttendedBy { get; set; }

        public string SmartSearch { get; set; }

        public int UserID { get; set; }

        public string CreatedBy { get; set; }

        public string ApprovedBy { get; set; }
    }
}
