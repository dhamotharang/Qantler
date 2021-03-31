using System;

namespace TimeAttendanceService.Models
{
    public class MasterList
    {
        public LeaveMaster[] LeaveMasterList { get; set; }
    }

    public class LeaveMaster
    {
        public string Rec_id { get; set; }
        public string lev_id { get; set; }
        public string desc_en { get; set; }
        public string desc_ar { get; set; }
        public string CreatedDate { get; set; }
    }
}
