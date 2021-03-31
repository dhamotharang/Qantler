using System;
using System.Collections.Generic;
using System.Net.Mail;
namespace RrcRemainder
{
    class MeetingRrc
    {
        public Int32 MeetingID { get; set; }
        public String ReferenceNumber { get; set; }
        public String Subject { get; set; }
        public String Location { get; set; }
        public String StartDateTime { get; set; }
        public String EndDateTime { get; set; }
        public Boolean IsInternalInvitees { get; set; }
        public Boolean IsExternalInvitees { get; set; }
        public List<string> RemainderTime { get; set; }

        public List<MailAddress> InternalInvitees { get; set; }
        public List<MailAddress> ExternalInvitees { get; set; }
		public List<MailAddress> Organizer { get; set; }
	}
}