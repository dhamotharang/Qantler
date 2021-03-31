using System;

namespace RulersCourt.Models.Circular
{
    public class CircularReportRequestModel
    {
        public string Status { get; set; }

        public string SourceOU { get; set; }

        public string DestinationOU { get; set; }

        public DateTime? DateRangeFrom { get; set; }

        public DateTime? DateRangeTo { get; set; }

        public string Priority { get; set; }

        public string SmartSearch { get; set; }

        public int UserID { get; set; }
    }
}
