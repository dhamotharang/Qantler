using Microsoft.AspNetCore.Hosting;
using RulersCourt.Models;
using RulersCourt.Models.Meeting;
using RulersCourt.Repository;
using RulersCourt.Repository.Meeting;
using RulersCourt.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using Workflow;

namespace RulersCourt.Translators.Meeting
{
    public class MeetingWorkFlowTranslator : CommonWorkflow
    {
        public WorkflowBO MeetingGetWorkFlow(MeetingResponseModel responseModel, string conn, EncryptionService encryptionService, IHostingEnvironment environment)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Meeting";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;

            bo.From = GetActor(responseModel.CreatorID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);

            List<Actor> invites = new List<Actor>();
            foreach (MeetingInternalInviteesModel i in responseModel.InternalInvitees)
            {
                invites.Add(GetActor(i.UserID, conn));
            }

            foreach (MeetingExternalInviteesModel i in responseModel.ExternalInvitees)
            {
                invites.Add(new Workflow.Actor() { Name = i.ContactPerson, Email = i.EmailID, CanSendEmail = true, CanSendSMS = true });
            }

            var a = invites;

            switch (responseModel.Action)
            {
                case "Submit":

                    bo.To = invites;
                    bo.WorkflowProcess = Workflow.WorkflowType.MeetingInvitesWorkflow;
                    break;

                case "Reschedule":

                    bo.To = bo.To = invites;
                    bo.WorkflowProcess = Workflow.WorkflowType.MeetingRescheduleWorkflow;
                    break;

                case "cancel":

                    bo.To = invites;
                    bo.WorkflowProcess = Workflow.WorkflowType.MeetingCancelWorkflow;
                    break;

                case "Create":

                    bo.To = invites;
                    bo.WorkflowProcess = Workflow.WorkflowType.MeetingMomCreatedWorkflow;
                    string uploadDir = string.Empty;
                    if (environment.IsDevelopment())
                    {
                        uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                    }
                    else
                    {
                        uploadDir = Path.Combine(environment.ContentRootPath, "Uploads");
                    }

                    List<Attachment> attachment = new List<Attachment>();
                    string reportPath = Directory.GetCurrentDirectory() + @"\Downloads\" + responseModel.ReferenceNumber + ".pdf";
                    if (System.IO.File.Exists(reportPath))
                    {
                        var report = new FileStream(reportPath, FileMode.Open);
                        Attachment reportAttachment = new Attachment(report, MediaTypeNames.Application.Octet);
                        reportAttachment.Name = responseModel.ReferenceNumber + ".pdf";
                        attachment.Add(reportAttachment);
                    }

                    bo.Attachment = attachment;
                    break;

                case "Add Comment":
                    bo.From = GetActor(responseModel.FromID, conn);
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.MeetingChatWorkflow;
                    break;
            }

            return bo;
        }

        public Workflow.WorkflowBO GetDutyTaskCommunicationWorkflow(MeetingCommunicationHistoryModel communicationBoard, string conn, string lang)
        {
            MeetingGetModel dutyTask = new MeetingGetModel();
            dutyTask = new MeetingClient().GetMeetingByID(conn, communicationBoard.MeetingID, int.Parse(communicationBoard.CreatedBy), lang);
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Meeting";
            bo.ReferenceNumber = dutyTask.ReferenceNumber;
            bo.ServiceRequestor = GetActor(int.Parse(communicationBoard.CreatedBy), conn);
            bo.From = GetActor(int.Parse(communicationBoard.CreatedBy), conn);
            List<Workflow.Actor> toActor = new List<Workflow.Actor>();

            bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(dutyTask.CreatedBy, conn) };
            bo.WorkflowProcess = Workflow.WorkflowType.MeetingChatWorkflow;
            return bo;
        }

        private static List<OrganizationModel> GetM_Organisation(string connString)
        {
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList());
            return e;
        }
    }
}
