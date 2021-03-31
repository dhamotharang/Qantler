using System;

namespace RulersCourt.Models
{
    public class LetterReportRequestModel
    {
        public string Status { get; set; }

        public string SourceOU { get; set; }

        public string Destination { get; set; }

        public DateTime? DateRangeForm { get; set; }

        public DateTime? DateRangeTo { get; set; }

        public string Priority { get; set; }

        public string SmartSearch { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }
    }
}
