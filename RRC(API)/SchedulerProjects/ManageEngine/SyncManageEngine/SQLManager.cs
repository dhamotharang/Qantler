using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace SyncManageEngine
{
    public class SQLManager
    {
        public List<ITSupportModel> GetNonSyncTickets()
        {
            List<ITSupportModel> lstTickets = new List<ITSupportModel>();
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT it.[ReferenceNumber],[Subject],o.OrganizationUnits as [RequestorDepartment],u.[ADEmployeeName] as RequestorName,l.DisplayName as [RequestType],[RequestDetails],[Priority] " +
                        "FROM [ITSupport] it JOIN [dbo].UserProfile u ON it.[RequestorName] = u.UserProfileId " +
                        "JOIN [dbo].[M_Lookups] l ON l.Module = it.RequestType and l.Category = 'ITRequest' " +
                        "JOIN [dbo].Organization o ON it.[RequestorDepartment] = o.OrganizationID " +
                        "WHERE IsSync = 0";
                    sqlConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        ITSupportModel itModel = new ITSupportModel();
                        itModel.RefNo = reader["ReferenceNumber"] != null ? Convert.ToString(reader["ReferenceNumber"]) : string.Empty;
                        itModel.Subject = reader["Subject"] != null ? Convert.ToString(reader["Subject"]) : string.Empty;
                        itModel.RequestorDepartment = reader["RequestorDepartment"] != null ? Convert.ToString(reader["RequestorDepartment"]) : string.Empty;
                        itModel.RequestorName = reader["RequestorName"] != null ? Convert.ToString(reader["RequestorName"]) : string.Empty;
                        itModel.RequestType = reader["RequestType"] != null ? Convert.ToString(reader["RequestType"]) : string.Empty;
                        itModel.RequestDetails = reader["RequestDetails"] != null ? Convert.ToString(reader["RequestDetails"]) : string.Empty;
                        itModel.Priority = reader["Priority"] != null ? "High" : string.Empty;
                        lstTickets.Add(itModel);
                    }
                }
            }

            return lstTickets;
        }

        public List<ITSupportModel> GetOpenTickets()
        {
            List<ITSupportModel> lstTickets = new List<ITSupportModel>();
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT [ServiceDeskId],it.[ReferenceNumber],[Subject],o.OrganizationUnits as [RequestorDepartment],u.[ADEmployeeName] as RequestorName,l.DisplayName as [RequestType],[RequestDetails],[Priority] " +
                        "FROM [dbo].[ITSupport] it JOIN [dbo].UserProfile u ON it.[RequestorName] = u.UserProfileId " +
                        "JOIN [dbo].[M_Lookups] l ON l.Module = it.RequestType and l.Category = 'ITRequest' " +
                        "JOIN [dbo].Organization o ON it.[RequestorDepartment] = o.OrganizationID " +
                        "WHERE Status = 0";
                    sqlConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        ITSupportModel itModel = new ITSupportModel();
                        itModel.ServiceDeskId = reader["ServiceDeskId"] != null ? Convert.ToString(reader["ServiceDeskId"]) : string.Empty;
                        itModel.RefNo = reader["ReferenceNumber"] != null ? Convert.ToString(reader["ReferenceNumber"]) : string.Empty;
                        itModel.Subject = reader["Subject"] != null ? Convert.ToString(reader["Subject"]) : string.Empty;
                        itModel.RequestorDepartment = reader["RequestorDepartment"] != null ? Convert.ToString(reader["RequestorDepartment"]) : string.Empty;
                        itModel.RequestorName = reader["RequestorName"] != null ? Convert.ToString(reader["RequestorName"]) : string.Empty;
                        itModel.RequestType = reader["RequestType"] != null ? Convert.ToString(reader["RequestType"]) : string.Empty;
                        itModel.RequestDetails = reader["RequestDetails"] != null ? Convert.ToString(reader["RequestDetails"]) : string.Empty;
                        itModel.Priority = reader["Priority"] != null ? Convert.ToString(reader["Priority"]) : string.Empty;
                        lstTickets.Add(itModel);
                    }
                }
            }

            return lstTickets;
        }

        public void UpdateServiceDeskResponseId(string refNo, string responseId)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "UPDATE ITSupport SET ServiceDeskId = '" + responseId + "', IsSync = 1 WHERE ReferenceNumber= '" + refNo + "'";
                    sqlConnection.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void UpdateStatus(string refNo)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SQLConnectionstring"]))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "UPDATE ITSupport SET Status = 1 WHERE ReferenceNumber= '" + refNo + "'";
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
                    sqlCommand.CommandText = "UPDATE ApplicationSyncInfo SET SyncDateTime=GETDATE() WHERE ExternalAppName='ManageEngineModule'";
                    sqlConnection.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}