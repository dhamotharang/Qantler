using System;
using System.Collections.Generic;
using System.Net.Mail;
namespace RrcRemainder
{
    class TrainingRrc
	{
        public Int32 TrainingID { get; set; }
        public String ReferenceNumber { get; set; }
        public String FromEmail { get; set; }
        public String FromName { get; set; }
        public String ToEmail { get; set; }
        public String ToName { get; set; }
		public String WorkflowProcess { get; set; }
		public String Service { get; set; }
		public String TrainingName { get; set; }
		public int? TraineeID { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
}