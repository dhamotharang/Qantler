using System;

namespace TimeAttendanceService.Models
{
    public class PunchInfo
    {
        public string EMP_NO { get; set; }
        public string TRN_DATE { get; set; }
        public string TRN_TIME { get; set; }
        public int REASON_CODE { get; set; }
        public string TRN_REASON_DESC { get; set; }
        public string DEVICE_ID { get; set; }
        public string DESCRIPTION { get; set; }
    }

    public class EmployeeRequestPunchInfoByDate
    {
        public string EmpNo { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

    public class PunchInfoResponse
    {
        public string Success { get; set; }
        public PunchInfo[] Data { get; set; }
    }

    public class PunchResponseModel
    {
        public string EmployeeCode { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
    }
}
