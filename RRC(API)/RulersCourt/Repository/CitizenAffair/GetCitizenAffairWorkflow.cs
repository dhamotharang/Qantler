using Microsoft.AspNetCore.Hosting;
using RulersCourt.Models.CitizenAffair;
using RulersCourt.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Net.Mime;
using Workflow;

namespace RulersCourt.Repository.CitizenAffair
{
    public class GetCitizenAffairWorkflow : CommonWorkflow
    {
        public Workflow.WorkflowBO GetWorkflow(CitizenAffairWorkflowModel responseModel, string conn, string apiUrl, EncryptionService encryptionService, IHostingEnvironment environment)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            string aesEncryptedFileExtension = ".aes";
            bo.Service = "Citizen Affair";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            string deletgateUser;
            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);
            switch (responseModel.Action)
            {
                case "Save":
                    if (responseModel.ApproverID != 0 && responseModel.ApproverID != null)
                    {
                        bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.DraftWorkflow;
                    break;

                case "Submit":

                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parameters = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID)
                    };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameters);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);

                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "Resubmit":

                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parama = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID)
                    };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parama);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);

                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "Approve":
                    if (responseModel.NotifyUpon.Equals("1") && responseModel.InternalRequestorID != null)
                    {
                        List<Workflow.Actor> toActor = new List<Workflow.Actor>();
                        toActor.Add(GetActor(responseModel.InternalRequestorID, conn));
                        bo.To = toActor;
                        if (responseModel.CurrentStatus == 59 && responseModel.PreviousApproverID != responseModel.FromID)
                        {
                            bo.From = GetActor(responseModel.PreviousApproverID, conn);
                            bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                        }

                        toActor.Add(GetActor(responseModel.CreatorID, conn));
                        bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
                    }
                    else if (responseModel.NotifyUpon.Equals("2") && responseModel.ExternalRequestEmailID != null)
                    {
                        List<Workflow.Actor> toActor = new List<Workflow.Actor>();
                        toActor.Add(new Workflow.Actor() { Name = "ExternalRequestor", Email = responseModel.ExternalRequestEmailID, CanSendEmail = true });
                        bo.To = toActor;
                        if (responseModel.CurrentStatus == 59 && responseModel.PreviousApproverID != responseModel.FromID)
                        {
                            bo.From = GetActor(responseModel.PreviousApproverID, conn);
                            bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                        }

                        bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
                        toActor.Add(GetActor(responseModel.CreatorID, conn));
                    }

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

                    List<CitizenAffairAttachmentGetModel> photos = new CitizenAffairAttachmentClient().GetAttachmentById(conn, responseModel.CitizenAffairID, "Photo");
                    foreach (CitizenAffairAttachmentGetModel photo in photos)
                    {
                        var path = Path.Combine(uploadDir, photo.AttachmentGuid, photo.AttachmentsName);
                        var encFilePath = Path.Combine(uploadDir, photo.AttachmentGuid, photo.AttachmentsName + aesEncryptedFileExtension);
                        if (System.IO.File.Exists(encFilePath))
                        {
                            var stream = new FileStream(encFilePath, FileMode.Open);
                            var decStream = encryptionService.Decrypt(stream);
                            decStream.Position = 0;
                            Attachment uploadAttachment = new Attachment(decStream, MediaTypeNames.Application.Octet);
                            uploadAttachment.Name = photo.AttachmentsName;
                            attachment.Add(uploadAttachment);
                        }
                    }

                    bo.Attachment = attachment;
                    break;

                case "Reject":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 59 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.RejectWorkflow;
                    break;

                case "ReturnForInfo":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 59 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.ReturnWorkflow;
                    break;

                case "Escalate":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parameter1 = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID)
                    };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameter1);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);
                    if (responseModel.CurrentStatus == 59 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.EscalateWorkflow;
                    break;

                case "Close":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.CloseWorkflow;
                    break;
            }

            return bo;
        }
    }
}
