using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using TimeAttendanceService.Models;

namespace TimeAttendanceService
{
    public class SQLManager
    {
        public List<EmployeeData> GetEmployeePunchDates()
        {
            List<EmployeeData> employeeList = new List<EmployeeData>();
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT EmployeeCode from UserProfile WHERE InTime is null or OutTime is null or PunchSyncDate <> CONVERT(varchar, getdate(), 1)";
                    sqlConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        EmployeeData employeeData = new EmployeeData();
                        employeeData.EmployeeCode = reader["EmployeeCode"] != null ? Convert.ToString(reader["EmployeeCode"]) : string.Empty;
                        employeeList.Add(employeeData);
                    }
                }
            }

            return employeeList;
        }

        public void UpdatePunchTiming(PunchResponseModel employeeData)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    var inTime = employeeData.InTime != null ? "'" + employeeData.InTime?.ToString("yyyy-MM-dd HH:mm") + "'" : "null";
                    var OutTime = employeeData.OutTime != null ? "'" + employeeData.OutTime?.ToString("yyyy-MM-dd HH:mm") + "'" : "null";
                    sqlCommand.CommandText = "UPDATE UserProfile SET InTime = " + inTime + ", OutTime = " + OutTime + ", PunchSyncDate = CONVERT(varchar, getdate(), 1) WHERE EmployeeCode = '" + employeeData.EmployeeCode + "'";
                    sqlConnection.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public List<EmployeeData> GetApprovedLeaves()
        {
            List<EmployeeData> employeeList = new List<EmployeeData>();
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT l.ReferenceNumber, u.EmployeeCode, StartDate, EndDate, " +
                        "CASE WHEN LeaveType = 0 THEN 'ANNUAL_LEAVE' ELSE 'LEAVE_' + CAST(lt.LeaveTypeID AS VARCHAR) END AS LeaveType, " +
                        "lt.LeaveTypeName, lt.ArLeaveTypeName, Reason from Leave l JOIN UserProfile u ON l.SourceOU = u.UserProfileId LEFT JOIN " +
                        "M_LeaveType lt ON lt.LeaveTypeID = LeaveTypeOther " +
                        "Where IsSync = 0 and l.IsHrHeadApproved = 1 and l.DeleteFlag = 0";
                    sqlConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        EmployeeData employeeData = new EmployeeData();
                        employeeData.ReferenceNo = reader["ReferenceNumber"] != null ? Convert.ToString(reader["ReferenceNumber"]) : string.Empty;
                        employeeData.EmployeeCode = reader["EmployeeCode"] != null ? Convert.ToString(reader["EmployeeCode"]) : string.Empty;
                        employeeData.StartDate = !DBNull.Value.Equals(reader["StartDate"]) ? Convert.ToDateTime(reader["StartDate"]).ToString("yyyy-MM-dd HH:mm:ss") : ""; ;
                        employeeData.EndDate = !DBNull.Value.Equals(reader["EndDate"]) ? Convert.ToDateTime(reader["EndDate"]).ToString("yyyy-MM-dd HH:mm:ss") : ""; ;
                        var leaveType = reader["LeaveType"] != null ? Convert.ToString(reader["LeaveType"]) : string.Empty;
                        employeeData.LeaveTypeCode = leaveType;
                        if (leaveType.Equals("ANNUAL_LEAVE"))
                        {
                            employeeData.LeaveType_AR = "إجازة سنوية";
                            employeeData.LeaveType_EN = "Annual Leave";
                        }
                        else
                        {
                            employeeData.LeaveType_AR = reader["ArLeaveTypeName"] != null ? Convert.ToString(reader["ArLeaveTypeName"]) : string.Empty;
                            employeeData.LeaveType_EN = reader["LeaveTypeName"] != null ? Convert.ToString(reader["LeaveTypeName"]) : string.Empty;
                        }
                        employeeData.Remarks = reader["Reason"] != null ? Convert.ToString(reader["Reason"]) : string.Empty;
                        employeeList.Add(employeeData);
                    }
                }
            }

            return employeeList;
        }

        public List<EmployeeData> GetOfficialTaskLeaves()
        {
            List<EmployeeData> employeeList = new List<EmployeeData>();
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT o.ReferenceNumber, u.EmployeeCode, StartDate, EndDate, 'OFFICIAL_LEAVE' as LeaveType from OfficialTask o " +
                        "JOIN UserProfile u ON o.SourceName = u.UserProfileId " +
                        "Where o.IsSync = 0 and o.DeleteFlag = 0";
                    sqlConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        EmployeeData employeeData = new EmployeeData();
                        employeeData.ReferenceNo = reader["ReferenceNumber"] != null ? Convert.ToString(reader["ReferenceNumber"]) : string.Empty;
                        employeeData.EmployeeCode = reader["EmployeeCode"] != null ? Convert.ToString(reader["EmployeeCode"]) : string.Empty;
                        employeeData.StartDate = !DBNull.Value.Equals(reader["StartDate"]) ? Convert.ToDateTime(reader["StartDate"]).ToString("yyyy-MM-dd HH:mm:ss") : ""; ;
                        employeeData.EndDate = !DBNull.Value.Equals(reader["EndDate"]) ? Convert.ToDateTime(reader["EndDate"]).ToString("yyyy-MM-dd HH:mm:ss") : ""; ;
                        employeeData.LeaveTypeCode = reader["LeaveType"] != null ? Convert.ToString(reader["LeaveType"]) : string.Empty;
                        employeeData.LeaveType_EN = "Official Leave";
                        employeeData.LeaveType_AR = "";
                        employeeData.Remarks = "Official Leave";
                        employeeList.Add(employeeData);
                    }
                }
            }

            return employeeList;
        }

        public void UpdateLeaveRequestSync(string referenceNumber)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "UPDATE Leave SET IsSync = 1 WHERE ReferenceNumber = '" + referenceNumber + "'";
                    sqlConnection.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOfficialTaskRequestSync(string referenceNumber)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "UPDATE OfficialTask SET IsSync = 1 WHERE ReferenceNumber = '" + referenceNumber + "'";
                    sqlConnection.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void UpdateSyncTime()
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "UPDATE ApplicationSyncInfo SET SyncDateTime=GETDATE() WHERE ExternalAppName='TimeAttendanceModule'";
                    sqlConnection.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}