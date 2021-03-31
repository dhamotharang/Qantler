using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeAttendanceService.Models;
using System.Linq;

namespace TimeAttendanceService
{
    public static class LeaveProcessService
    {
        static SQLManager sqlManager = new SQLManager();
        static HttpClient client = new HttpClient();
        private static readonly string TimeAttendanceLeaveMasterUrl = ConfigurationManager.AppSettings["TASLeaveMasterUrl"];
        private static readonly string TimeAttendancePostLeaveUrl = ConfigurationManager.AppSettings["TASPostLeaveUrl"];

        public static void ProcessLeaveRequests()
        {
            var employeeList = sqlManager.GetApprovedLeaves();
            //Log.Information(JsonConvert.SerializeObject(employeeList));
            if (employeeList.Where(x => !string.IsNullOrEmpty(x.EmployeeCode)).Any())
            {
                Log.Information("-----Processing Leave Master-----");
                //Post Leave Master
                List<LeaveMaster> masterDataList = new List<LeaveMaster>();
                foreach (var employee in employeeList)
                {
                    var masterData = new LeaveMaster() { lev_id = employee.LeaveTypeCode, desc_en = employee.LeaveType_EN, desc_ar = employee.LeaveType_AR, Rec_id = employee.LeaveTypeCode, CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm") };
                    masterDataList.Add(masterData);
                }
                PostMasterRequestAsync(masterDataList.ToArray()).Wait();

                Log.Information("-----Processing Leave requests - Total: " + employeeList.Count + " -----");

                //Post Leave Online
                foreach (var employee in employeeList)
                {
                    var leaveRequest = new LeaveRequest() { Emp_Id = employee.EmployeeCode, leaveFromDate = employee.StartDate, leaveToDate = employee.EndDate, leaveTypeId = employee.LeaveTypeCode, Rec_Id = employee.ReferenceNo, remarks = employee.Remarks };
                    PostLeaveRequestAsync(leaveRequest).Wait();

                    //Update Sync Status
                    sqlManager.UpdateLeaveRequestSync(employee.ReferenceNo);
                }
            }
        }

        public static void ProcessOfficialLeaveRequests()
        {
            var employeeList = sqlManager.GetOfficialTaskLeaves();
            //Log.Information(JsonConvert.SerializeObject(employeeList));
            if (employeeList.Where(x => !string.IsNullOrEmpty(x.EmployeeCode)).Any())
            {
                Log.Information("-----Processing Leave Master-----");
                //Post Leave Master
                List<LeaveMaster> masterDataList = new List<LeaveMaster>();
                foreach (var employee in employeeList)
                {
                    var masterData = new LeaveMaster() { lev_id = employee.LeaveTypeCode, desc_en = employee.LeaveType_EN, desc_ar = employee.LeaveType_AR, Rec_id = employee.LeaveTypeCode, CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm") };
                    masterDataList.Add(masterData);
                }
                PostMasterRequestAsync(masterDataList.ToArray()).Wait();

                Log.Information("-----Processing Official Leave requests - Total: " + employeeList.Count + " -----");
                //Post Official Leave
                foreach (var employee in employeeList)
                {
                    var leaveRequest = new LeaveRequest() { Emp_Id = employee.EmployeeCode, leaveFromDate = employee.StartDate, leaveToDate = employee.EndDate, leaveTypeId = employee.LeaveTypeCode, Rec_Id = employee.ReferenceNo, remarks = employee.LeaveType_EN };
                    PostLeaveRequestAsync(leaveRequest).Wait();

                    //Update Sync Status
                    sqlManager.UpdateOfficialTaskRequestSync(employee.ReferenceNo);
                }
            }
        }

        static async Task<string> PostMasterRequestAsync(LeaveMaster[] masterData)
        {
            var strJson = JsonConvert.SerializeObject(masterData);
            try
            {
                HttpResponseMessage response = await client.PostAsync(TimeAttendanceLeaveMasterUrl, new StringContent(strJson, Encoding.UTF8, "application/json"));
                Log.Information("POST Leave Master request: " + strJson);
                Log.Information("POST Leave Master response: " + response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.StackTrace);
            }
            return "";
        }

        static async Task<string> PostLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            var strJson = JsonConvert.SerializeObject(leaveRequest);
            try
            {
                HttpResponseMessage response = await client.PostAsync(TimeAttendancePostLeaveUrl, new StringContent(strJson, Encoding.UTF8, "application/json"));
                Log.Information("POST Leave request: " + strJson);
                Log.Information("POST Leave response: " + response.Content.ReadAsStringAsync().Result);                
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.StackTrace);
            }
            return "";
        }
    }
}