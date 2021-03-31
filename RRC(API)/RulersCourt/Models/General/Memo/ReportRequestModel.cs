using System;

namespace RulersCourt.Models
{
    public class ReportRequestModel
    {
        public string Status { get; set; }

        public string SourceOU { get; set; }

        public string DestinationOU { get; set; }

        public DateTime? DateRangeForm { get; set; }

        public DateTime? DateRangeTo { get; set; }

        public string Private { get; set; }

        public string Priority { get; set; }

        public string SmartSearch { get; set; }

        public int UserID { get; set; }
    }
}
