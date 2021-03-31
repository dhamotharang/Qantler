using System;

namespace OERService.Models
{
	public class WebContentPages
    {
        public int Id { get; set; }
        public int PageID { get; set; }
        public string PageContent { get; set; }
        public string PageContent_Ar { get; set; }
		public string WebPage { get; set; }
		public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
		public string VideoLink { get; set; }
	}
    public class WebPages
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string PageName_Ar { get; set; }
    }
}
