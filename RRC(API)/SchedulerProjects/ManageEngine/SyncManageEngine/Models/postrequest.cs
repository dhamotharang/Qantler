using System;
using System.Collections.Generic;
using System.Text;

namespace SyncManageEngine
{
    public class PostRequest
    {
        public request request { get; set; }
    }

    public class request
    {
        public string subject { get; set; }
        public string description { get; set; }
        public requester requester { get; set; }
        //public string impact_details { get; set; }
        public resolution resolution { get; set; }
        public status status { get; set; }
    }

    public class requester
    {
        public string name { get; set; }
    }

    public class resolution
    {
        public string content { get; set; }
    }

    public class status
    {
        public string name { get; set; }
    }
}
