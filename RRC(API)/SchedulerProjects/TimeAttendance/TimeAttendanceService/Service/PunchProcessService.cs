using Newtonsoft.Json;
using Serilog;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeAttendanceService.Models;

namespace TimeAttendanceService
{
    public static class PunchProcessService
    {
        static SQLManager sqlManager = new SQLManager();
        static HttpClient client = new HttpClient();
        private static readonly string TimeAttendanceUrl = ConfigurationManager.AppSettings["TASPunchUrl"];
        private static readonly int InTimeReasonCode = Convert.ToInt32(ConfigurationManager.AppSettings["InTimeReasonCode"]);
        private static readonly int OutTimeReasonCode = Convert.ToInt32(ConfigurationManager.AppSettings["OutTimeReasonCode"]);

        public static void ProcessPunchTimings()
        {           
            var employeeList = sqlManager.GetEmployeePunchDates();
            foreach (var employee in employeeList)
            {
                if (!string.IsNullOrEmpty(employee.EmployeeCode))
                {
                    EmployeeRequestPunchInfoByDate employeeData1 = new EmployeeRequestPunchInfoByDate() { EmpNo = employee.EmployeeCode, fromDate = DateTime.Now.ToString("MM-dd-yyyy"), toDate = DateTime.Now.ToString("MM-dd-yyyy") };
                    GetPunchRequestAsync(employeeData1).Wait();
                }
            }
        }

        static async Task<string> GetPunchRequestAsync(EmployeeRequestPunchInfoByDate employeeData)
        {
            var strJson = JsonConvert.SerializeObject(employeeData);
            try
            {
                HttpResponseMessage response = await client.PostAsync(TimeAttendanceUrl, new StringContent(strJson, Encoding.UTF8, "application/json"));
                var resp = JsonConvert.DeserializeObject<PunchInfoResponse>(response.Content.ReadAsStringAsync().Result);
                PunchResponseModel modelData = new PunchResponseModel();
                modelData.EmployeeCode = employeeData.EmpNo;
                foreach (var dt in resp.Data)
                {
                    if (dt.REASON_CODE == InTimeReasonCode)
                        modelData.InTime = DateTime.ParseExact(string.Format("{0} {1}", dt.TRN_DATE, dt.TRN_TIME), "MM/dd/yyyy HH:mm", null);
                    else if (dt.REASON_CODE == OutTimeReasonCode)
                        modelData.OutTime = DateTime.ParseExact(string.Format("{0} {1}", dt.TRN_DATE, dt.TRN_TIME), "MM/dd/yyyy HH:mm", null);
                }

                //Update punch in/out time
                sqlManager.UpdatePunchTiming(modelData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.StackTrace);
            }
            return "";
        }
    }
}
