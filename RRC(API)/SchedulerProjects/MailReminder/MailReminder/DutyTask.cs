using System;
using System.Net.Mail;
namespace RrcRemainder
{
    class DutyTask
    {
        public string Id;
        public string ReferenceNumber;
        public string Title;
        public string Priority;
        public string TaskDetails;
        public string StartDate;
        public string EndDate;
        public string RemindMeAt;
        public Int32  AssigneeID;
        public MailAddress AssigneeMail;
    }
}