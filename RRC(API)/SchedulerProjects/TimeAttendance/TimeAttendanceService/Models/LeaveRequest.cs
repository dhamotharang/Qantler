using System;

namespace TimeAttendanceService.Models
{
    public class LeaveRequest
    {
        public string Rec_Id { get; set; }
        public string Emp_Id { get; set; }
        public string leaveFromDate { get; set; }
        public string leaveToDate { get; set; }
        public string leaveTypeId { get; set; }
        public string remarks { get; set; }
    }
}