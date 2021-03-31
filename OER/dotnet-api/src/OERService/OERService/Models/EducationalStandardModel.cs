using System;

namespace OERService.Models
{
	public class EducationalStandardModel
    {
        public int Id { get; set; }
        public string Standard { get; set; }
        public string Standard_Ar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool? Active { get; set; }
		public int Weight { get; set; }
	}
    public class EducationalStandardCreate
    {
        public string Standard { get; set; }
        public string Standard_Ar { get; set; }
        public int CreatedBy { get; set; }
		public int Weight { get; set; }
	}
    public class EducationalStandardUpdate
    {
        public int Id { get; set; }
        public string Standard { get; set; }
        public string Standard_Ar { get; set; }
        public int UpdatedBy { get; set; }
        public bool? Active { get; set; }
		public int Weight { get; set; }
	}
}
