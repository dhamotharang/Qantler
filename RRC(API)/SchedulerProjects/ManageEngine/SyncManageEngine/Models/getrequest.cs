namespace SyncManageEngine
{
    public class GetRequest
    {
        public List_Info list_info { get; set; }
    }

    public class List_Info
    {
        public Search_Fields search_fields { get; set; }
        public Filter_By filter_by { get; set; }
    }

    public class Search_Fields
    {
        public string statusname { get; set; }
    }

    public class Filter_By
    {
        public string name { get; set; }
    }
}
