export class CreateMemoModal {
     MemoID:number
     Title: string
     SourceOU: string
     SourceName: string
     DestinationUserId: any = []
     DestinationDepartmentId: any = []
     ApproverId: number
     ApproverDepartmentId: number
     Details: string
     Private: string
     Priority: string
     Keywords = []
     Attachments = [
          {
               AttachmentGuid: "string",
               AttachmentsName: "string",
               MemoID: "string"
          }
     ]
     CreatedBy: number
     CreatedDateTime: Date
     UpdatedBy: number
     UpdatedDateTime: Date
     Action: string
     Comments: string
}

