export class CreateDesignModal{
    Attachments = [
         {
              AttachmentGuid: "string",
              AttachmentsName: "string",
              MemoID: "string"
         }
    ]
    DesignID: number
   Date : any
   DiwansRole: string
   Title: string
   SourceOU: string
   SourceName: string
   Project : string
   DateofDeliverable : any
   OtherParties : string
   TargetGroups : string
   ApproverID: number
   ApproverDepartmentID: number
   Comments: string
   AttachmentName: string
   AssigneeID : [{
        AssigneeId : 'number'
   }]
   MainObjective : string
   GeneralObjective : string
   StrategicObjective : string
   TypeofDesignRequired : string
   Languages : string
   DeleteFlag: boolean
   CreatedBy: number
   UpdatedBy: number
   Action : string
   CreatedDateTime: any
   UpdatedDateTime: any
   Status : number
}