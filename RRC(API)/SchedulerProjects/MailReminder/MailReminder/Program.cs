using MailReminder;
using Serilog;
using System;
using System.Collections.Generic;
namespace RrcRemainder
{
    class Program
    {
		static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.File("reminderlog.log")
				.CreateLogger();

			float freq;
			List<MeetingRrc> meetings;
			List<DutyTask> dutytasks;
			List<TrainingRrc> trainingList;

			Reminder r = new Reminder();
			SendMailRrc s_mail = new SendMailRrc();
			SendMailDutyTask d_mail = new SendMailDutyTask();
			SendEmailTraining t_mail = new SendEmailTraining();
		freq = r.getFrequency();

			trainingList = r.getAllActiveTraining();
			dutytasks = r.getAllDutyTasks();
			meetings = r.getAllActiveMeetings();
			//trainingList = r.getAllActiveTraining();
			DateTime currentDateTime = DateTime.UtcNow;

			foreach (var m in meetings)
			{
				foreach (var meetingRemainderTime in m.RemainderTime)
				{

					DateTime meetingTime = DateTime.Parse(meetingRemainderTime);

					if (currentDateTime.Year == meetingTime.Year && currentDateTime.Day == meetingTime.Day && currentDateTime.Month == meetingTime.Month && currentDateTime.Hour == meetingTime.Hour && currentDateTime.Minute == meetingTime.Minute)
					{

						if (m.Organizer.Count > 0)
						{ s_mail.send(m.Organizer, m); }
						//if (m.IsInternalInvitees)
						//{ s_mail.send(m.InternalInvitees, m); }
						//if (m.IsExternalInvitees)
						//{ s_mail.send(m.ExternalInvitees, m); }
					}
				}
			}

			foreach (var dt in dutytasks)
			{
				DateTime currentDateTimeTask = DateTime.UtcNow;
				DateTime dttime = DateTime.Parse(dt.RemindMeAt);
				System.TimeSpan diff = currentDateTimeTask - dttime;
				var diffhrs = Math.Round(diff.TotalHours, 2) / freq;
				if (diffhrs > 0)
				{
					//var t = Math.Round((diffhrs % 1), 2);
					if (Math.Round((diffhrs % 1), 2) == 0 || Math.Round((diffhrs % 1), 2) == 0.99)
					{
						d_mail.send(dt.AssigneeMail, dt);
					}
				}
			}

			foreach (var tr in trainingList)
			{
				t_mail.send(tr);
			}
		}
	}
}