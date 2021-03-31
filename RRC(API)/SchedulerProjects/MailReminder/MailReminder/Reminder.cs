using MailReminder;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;

namespace RrcRemainder
{
    class Reminder
    {
        static public string sqlServerConnectionString;
        SqlConnection sqlConnection;
        IConfigurationRoot configuration;

        public Reminder()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json");
            configuration = builder.Build();
            EncryptionService service = new EncryptionService(Convert.FromBase64String(configuration["EncryptionKeys:Key"]), Convert.FromBase64String(configuration["EncryptionKeys:IV"]));
            sqlServerConnectionString = service.Decrypt_Aes(configuration.GetSection("sqlServerConnectionString").Value.ToString());
            sqlConnection = new SqlConnection(sqlServerConnectionString);
        }
       
        public Boolean connectSqlserver()
        {
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                Log.Error(e, e.StackTrace);
            }

            return sqlConnection.State == ConnectionState.Open ? true : false;
        }

        public List<MeetingRrc> getAllActiveMeetings()
        {
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

            List<MeetingRrc> allMeetings = new List<MeetingRrc>();

           // SqlCommand command = new SqlCommand("Select * from [dbo].[Meeting] where StartDateTime <= @startdatetime", sqlConnection);
			SqlCommand command = new SqlCommand("Select * from [dbo].[Meeting]", sqlConnection);

            //command.Parameters.AddWithValue("@startdatetime", sqlFormattedDate);
            Console.WriteLine(sqlFormattedDate);
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {

                    MeetingRrc temp = new MeetingRrc();
                    temp.MeetingID = Convert.ToInt32(reader["MeetingID"]);
                    temp.ReferenceNumber = reader["ReferenceNumber"].ToString();
                    temp.Subject = Convert.IsDBNull(reader["Subject"]) ? null : reader["Subject"].ToString();
                    temp.Location = Convert.IsDBNull(reader["Location"]) ? null : reader["Location"].ToString();
                    temp.StartDateTime = Convert.IsDBNull(reader["StartDateTime"]) ? null : reader["StartDateTime"].ToString();
                    temp.EndDateTime = Convert.IsDBNull(reader["EndDateTime"]) ? null : reader["EndDateTime"].ToString();
                    temp.IsInternalInvitees = Convert.ToBoolean(reader["IsInternalInvitees"]);
                    temp.IsExternalInvitees = Convert.ToBoolean(reader["IsInternalInvitees"]);

                    Console.WriteLine(String.Format("{0}, {1}",
                    reader["StartDateTime"], reader["MeetingID"]));

                    allMeetings.Add(temp);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, e.StackTrace);
            }
            finally
            {
                reader.Close();
                sqlConnection.Close();
            }

            foreach (var meeting in allMeetings)
            {
                if (meeting.IsInternalInvitees)
                {
                    meeting.InternalInvitees = getAllInternalInvitees(meeting.MeetingID);
                }
                if (meeting.IsExternalInvitees)
                {
                    meeting.ExternalInvitees = getAllExternalInvitees(meeting.MeetingID);
                }
                meeting.RemainderTime = getRemainderTime(meeting.MeetingID);
				meeting.Organizer= getOrganizer(meeting.MeetingID);
			}

            return allMeetings;
        }

        private List<MailAddress> getAllInternalInvitees(Int32 meetingId)
        {
            List<MailAddress> mailAddresses = new List<MailAddress>();
            SqlCommand command = new SqlCommand("select u.OfficialMailId as EmailID from [dbo].[MeetingInternalInvitees] mi join UserProfile u on u.UserProfileId = mi.UserID where MeetingID = @meetingId", sqlConnection);

            command.Parameters.AddWithValue("@meetingId", meetingId);

            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    mailAddresses.Add(new MailAddress(reader["EmailID"].ToString()));
                }
            }
            catch (Exception e)
            {
                Log.Error(e, e.StackTrace);
            }
            finally
            {
                reader.Close();
                sqlConnection.Close();

            }

            return mailAddresses;
        }

        private List<MailAddress> getAllExternalInvitees(Int32 meetingId)
        {
            List<MailAddress> mailAddresses = new List<MailAddress>();
            SqlCommand command = new SqlCommand("Select * from [dbo].[MeetingExternalInvitees] where MeetingID = @meetingId", sqlConnection);

            command.Parameters.AddWithValue("@meetingId", meetingId);
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    mailAddresses.Add(new MailAddress(reader["EmailID"].ToString()));
                }
            }
            catch (Exception e)
            {
                Log.Error(e, e.StackTrace);
            }
            finally
            {
                reader.Close();
                sqlConnection.Close();
            }

            return mailAddresses;
        }


		private List<MailAddress> getOrganizer(Int32 meetingId)
		{
			List<MailAddress> mailAddresses = new List<MailAddress>();
			SqlCommand command = new SqlCommand("select u.OfficialMailId as EmailID from [dbo].[Meeting] mi join UserProfile u on u.UserProfileId = mi.CreatedBy where MeetingID = @meetingId", sqlConnection);

			command.Parameters.AddWithValue("@meetingId", meetingId);

			sqlConnection.Open();
			SqlDataReader reader = command.ExecuteReader();
			try
			{
				while (reader.Read())
				{
					mailAddresses.Add(new MailAddress(reader["EmailID"].ToString()));
				}
			}
			catch (Exception e)
			{
				Log.Error(e, e.StackTrace);
			}
			finally
			{
				reader.Close();
				sqlConnection.Close();

			}

			return mailAddresses;
		}

		public List<string> getRemainderTime(Int32 meetingId)
        {
            List<string> remainderDate = new List<string>();
            List<MailAddress> mailAddresses = new List<MailAddress>();
            SqlCommand command = new SqlCommand("Select  *  from [dbo].[MeetingRemindMeAt] where MeetingID = @meetingId", sqlConnection);

            command.Parameters.AddWithValue("@meetingId", meetingId);
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    remainderDate.Add(reader["RemindMeDateTime"].ToString());
                }
            }
            catch (Exception e)
            {
                Log.Error(e, e.StackTrace);
            }
            finally
            {
                reader.Close();
                sqlConnection.Close();
            }

            return remainderDate;
        }

        public List<DutyTask> getAllDutyTasks()
        {
            List<DutyTask> dutytasks = new List<DutyTask>();

            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

            SqlCommand command = new SqlCommand("Select *,(select DisplayName  from M_Lookups where Category='Priority' and Module= Priority)as PriorityName  from [dbo].[DutyTask] where EndDate >= @startdatetime", sqlConnection);
            command.Parameters.AddWithValue("@startdatetime", sqlFormattedDate);
            Console.WriteLine(sqlFormattedDate);
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    DutyTask temp = new DutyTask();

                    temp.Id = reader["TaskID"].ToString();
                    temp.ReferenceNumber = reader["ReferenceNumber"].ToString();
                    temp.Title = Convert.IsDBNull(reader["Title"]) ? "" : reader["Title"].ToString();
                    temp.Priority = Convert.IsDBNull(reader["PriorityName"]) ? "" : reader["PriorityName"].ToString();
                    temp.TaskDetails = Convert.IsDBNull(reader["TaskDetails"]) ? "" : reader["TaskDetails"].ToString();
                    temp.StartDate = Convert.IsDBNull(reader["StartDate"]) ? null : reader["StartDate"].ToString();
                    temp.EndDate = Convert.IsDBNull(reader["EndDate"]) ? null : reader["EndDate"].ToString();
                    temp.AssigneeID = Convert.ToInt32(reader["AssigneeID"]);
                    temp.RemindMeAt = Convert.IsDBNull(reader["RemindMeAt"]) ? null : reader["RemindMeAt"].ToString();
                    Console.WriteLine(String.Format("{0}, {1}",
                    reader["ReferenceNumber"], reader["Title"]));

                    if (temp.RemindMeAt != null)
                    {
                        dutytasks.Add(temp);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e, e.StackTrace);
            }
            finally
            {
                reader.Close();
                sqlConnection.Close();
            }
            foreach (var dt in dutytasks)
            {
                dt.AssigneeMail = getMailId(dt.AssigneeID);
            }

            return dutytasks;
        }

        public MailAddress getMailId(Int32 assigneeID)
        {
            MailAddress mailId = new MailAddress(configuration.GetSection("fromAddress").Value);
            SqlCommand command = new SqlCommand("Select  top(1) * from [dbo].[UserProfile] where UserProfileId = @UserProfileId", sqlConnection);

            command.Parameters.AddWithValue("@UserProfileId", assigneeID);
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    mailId = new MailAddress(reader["OfficialMailId"].ToString());
                }
            }
            catch (Exception e)
            {
                Log.Error(e, e.StackTrace);
            }
            finally
            {
                reader.Close();
                sqlConnection.Close();
            }

            return mailId;
        }

        public float getFrequency()
        {
            float frequency = 0;
            SqlCommand command = new SqlCommand("Select * from [dbo].[M_Lookups] where Category = 'MailRemainder'", sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    frequency = float.Parse(reader["DisplayName"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
                sqlConnection.Close();
            }

            return frequency;
        }

		public List<TrainingRrc> getAllActiveTraining()
		{
            Console.Write("Training Notification started");
			List<TrainingRrc> allTraining = new List<TrainingRrc>();

			// SqlCommand command = new SqlCommand("Select * from [dbo].[Meeting] where StartDateTime <= @startdatetime", sqlConnection);
			SqlCommand sqlCommand = sqlConnection.CreateCommand();
			sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
			sqlCommand.CommandText = "TrainingNotification";
			sqlConnection.Open();
			SqlDataReader reader = sqlCommand.ExecuteReader();
			try
			{
				while (reader.Read())
				{
					TrainingRrc temp = new TrainingRrc();
					temp.TrainingID = Convert.ToInt32(reader["TrainingID"]);
					temp.ReferenceNumber = reader["ReferenceNumber"].ToString();
					temp.FromEmail = reader["FromEmail"].ToString();
					temp.FromName =  reader["FromName"].ToString();
					temp.ToName = reader["ToName"].ToString();
					temp.ToEmail = reader["ToEmail"].ToString();
					temp.WorkflowProcess = reader["WorkflowProcess"].ToString();
					temp.Service = reader["Service"].ToString();
					temp.TrainingName = reader["TrainingName"].ToString();
					temp.TraineeID = Convert.ToInt32(reader["TraineeID"]);
					temp.StartDate = Convert.ToDateTime(reader["StartDate"]);
					temp.EndDate = Convert.ToDateTime(reader["EndDate"]); 
					allTraining.Add(temp);
				}
			}
			catch (Exception e)
			{
                Console.Write("Training Notification Error");
                Log.Error(e, e.StackTrace);
			}
			finally
			{
                Console.Write("Training Notification Closed");
                reader.Close();
				sqlConnection.Close();
			}

			return allTraining;
		}
	}
}