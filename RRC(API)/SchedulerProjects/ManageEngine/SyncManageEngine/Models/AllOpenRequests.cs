namespace SyncManageEngine
{

    public class AllOpenRequests
    {
        public Request[] requests { get; set; }
    }

    public class Request
    {
        public Requester requester { get; set; }
        public Template template { get; set; }
        public string short_description { get; set; }
        public Created_Time created_time { get; set; }
        public string subject { get; set; }
        public object time_elapsed { get; set; }
        public bool is_overdue { get; set; }
        public object technician { get; set; }
        public object priority { get; set; }
        public Created_By created_by { get; set; }
        public object due_by_time { get; set; }
        public object site { get; set; }
        public bool is_service_request { get; set; }
        public string id { get; set; }
        public Status status { get; set; }
        public object group { get; set; }
    }

    public class Requester
    {
        public string email_id { get; set; }
        public string name { get; set; }
        public bool is_vipuser { get; set; }
        public string id { get; set; }
    }

    public class Template
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Created_Time
    {
        public string display_value { get; set; }
        public string value { get; set; }
    }

    public class Created_By
    {
        public string email_id { get; set; }
        public string name { get; set; }
        public bool is_vipuser { get; set; }
        public string id { get; set; }
    }

    public class Status
    {
        public string color { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }
}
