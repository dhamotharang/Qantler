export class CreateCircularModal {
    Title: string
    SourceOU: string
    SourceName: string
    DestinationDepartmentID: any = []
    ApproverId: number
    ApproverDepartmentId: number
    Details: string
    Priority: string
    Attachments = [
         {
              AttachmentGuid: "string",
              AttachmentsName: "string",
              MemoID: "string"
         }
    ]
    CreatedBy: number
    CreatedDateTime: Date
    Action: string
    Comments: string
}

