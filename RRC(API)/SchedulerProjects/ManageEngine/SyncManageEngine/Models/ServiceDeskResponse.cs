using System;
using System.Collections.Generic;
using System.Text;

namespace SyncManageEngine.Models
{

    public class ServiceDeskResponse
    {
        public Request request { get; set; }
        public Response_Status response_status { get; set; }
    }

    public class Request
    {
        public string subject { get; set; }
        public Resolution resolution { get; set; }
        public object linked_to_request { get; set; }
        public object mode { get; set; }
        public object lifecycle { get; set; }
        public object[] assets { get; set; }
        public bool is_trashed { get; set; }
        public string id { get; set; }
        public object assigned_time { get; set; }
        public object group { get; set; }
        public Requester requester { get; set; }
        public object[] email_to { get; set; }
        public Created_Time created_time { get; set; }
        public object item { get; set; }
        public object level { get; set; }
        public bool has_resolution_attachments { get; set; }
        public object approval_status { get; set; }
        public object impact { get; set; }
        public object service_category { get; set; }
        public object sla { get; set; }
        public object priority { get; set; }
        public Created_By created_by { get; set; }
        public object first_response_due_by_time { get; set; }
        public object last_updated_time { get; set; }
        public bool has_notes { get; set; }
        public object impact_details { get; set; }
        public object subcategory { get; set; }
        public object[] email_cc { get; set; }
        public Status status { get; set; }
        public Template template { get; set; }
        public object[] email_ids_to_notify { get; set; }
        public object request_type { get; set; }
        public string description { get; set; }
        public bool has_dependency { get; set; }
        public bool has_conversation { get; set; }
        public object callback_url { get; set; }
        public bool is_service_request { get; set; }
        public object urgency { get; set; }
        public bool is_shared { get; set; }
        public bool has_request_initiated_change { get; set; }
        public object[] request_template_task_ids { get; set; }
        public object department { get; set; }
        public bool is_reopened { get; set; }
        public bool has_draft { get; set; }
        public bool has_attachments { get; set; }
        public bool has_linked_requests { get; set; }
        public bool is_overdue { get; set; }
        public object technician { get; set; }
        public bool has_request_caused_by_change { get; set; }
        public bool has_problem { get; set; }
        public object due_by_time { get; set; }
        public bool is_first_response_overdue { get; set; }
        public object category { get; set; }
    }

    public class Resolution
    {
        public object[] resolution_attachments { get; set; }
        public object content { get; set; }
    }

    public class Requester
    {
        public string email_id { get; set; }
        public string name { get; set; }
        public bool is_vipuser { get; set; }
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

    public class Template
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Response_Status
    {
        public int status_code { get; set; }
        public string status { get; set; }
    }
}
