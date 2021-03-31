using System;

namespace RulersCourt.Models.Protocol.Media
{
    public class MediaExportModel
    {
        public int? RequestType { get; set; }

        public string Status { get; set; }

        public string SourceOU { get; set; }

        public string SourceName { get; set; }

        public string SmartSearch { get; set; }

        public DateTime? ReqDateFrom { get; set; }

        public DateTime? ReqDateTo { get; set; }

        public int UserID { get; set; }
    }
}
