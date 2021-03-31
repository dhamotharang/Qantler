using System.Collections.Generic;
using System.Net.Mail;

namespace Workflow
{
	public class WorkflowBO
	{
		public string ReferenceNumber { get; set; }
		public int ServiceID { get; set; }
		public string Service { get; set; }
		public WorkflowType WorkflowProcess { get; set; }
		public Actor ServiceRequestor { get; set; }
		public Actor From { get; set; }
		public List<Actor> To { get; set; }
		public Actor DelegateFrom { get; set; }
		public Actor DelegateTo { get; set; }
		public int Status { get; set; }
		public List<Attachment> Attachment { get; set; }
		public bool? IsAnonymous { get; set; }
		public List<Actor> cc { get; set; }
		public List<Actor> SMSOrgDepUsers { get; set; }
	}
}
