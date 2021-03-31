using System;

namespace RulersCourt.Models.Maintenance
{
    public class MaintenanceReportRequestModel
    {
        public string Status { get; set; }

        public string SourceOU { get; set; }

        public string Subject { get; set; }

        public DateTime? RequestDateRangeFrom { get; set; }

        public DateTime? RequestDateRangeTo { get; set; }

        public string AttendedBy { get; set; }

        public string Priority { get; set; }

        public string SmartSearch { get; set; }

        public int UserID { get; set; }
    }
}
