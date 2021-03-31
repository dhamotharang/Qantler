export class CreatePhotoModal {
     Attachments = [
          {
               AttachmentGuid: "string",
               AttachmentsName: "string",
               MemoID: "string"
          }
     ]
     PhotoID: number
    Date : any
    Location: string
    Title: string
    SourceOU: string
    SourceName: string
    EventDate : any
    EventName : string
    PhotoDescription : string
    ApproverID: number
    ApproverDepartmentID: number
    Comments: string
    AttachmentName: string
    AssigneeID : [{
         AssigneeId : 'number'
    }]
    DeleteFlag: boolean
    CreatedBy: number
    UpdatedBy: number
    Action : string
    CreatedDateTime: any
    UpdatedDateTime: any
    Status : number
}

