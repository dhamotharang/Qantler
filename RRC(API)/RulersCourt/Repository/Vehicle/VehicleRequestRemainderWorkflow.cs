using RulersCourt.Models.Vehicle.VehicleFine;
using RulersCourt.Models.Vehicle.VehicleRequest;
using System;
using System.Collections.Generic;
using Workflow;

namespace RulersCourt.Repository.Vehicle
{
    public class VehicleRequestRemainderWorkflow : CommonWorkflow
    {
        public WorkflowBO GetVehicleRemainderWorkflow(VehicleFineGetModel responseModel, string conn, SendRemainderModel remainder, int userID)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "VehicleFine";
            bo.ServiceID = responseModel.VehicleFineID ?? 0;
            bo.Status = responseModel.Status.GetValueOrDefault();
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            bo.From = GetActor(userID, conn);
            bo.ServiceRequestor = GetActor(remainder.EmailCCUserID, conn);
            List<Workflow.Actor> toAct = new List<Workflow.Actor>();
            toAct.Add(GetActor(responseModel.DriverID, conn));
            bo.To = toAct;
            bo.WorkflowProcess = Workflow.WorkflowType.VehicleRemainderWorkflow;
            return bo;
        }

        public WorkflowBO GetVehicleSubmissionWorkflow(VehicleFineGetModel responseModel, string conn, int userID)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "VehicleFine";
            bo.Status = responseModel.Status.GetValueOrDefault();
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            bo.From = GetActor(userID, conn);
            bo.ServiceRequestor = GetActor(userID, conn);
            List<Workflow.Actor> toAct = new List<Workflow.Actor>();
            toAct.Add(GetActor(responseModel.DriverID, conn));
            bo.To = toAct;
            bo.WorkflowProcess = Workflow.WorkflowType.FineSubmissionWorkflow;
            return bo;
        }
    }
}
