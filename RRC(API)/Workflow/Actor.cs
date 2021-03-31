using System;
namespace Workflow
{
    public class Actor
    {
        public string Name { get; set; }
        public string ARName { get; set; }
        public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public bool CanSendSMS { get; set; }
		public bool CanSendEmail { get; set; }
	}
}
