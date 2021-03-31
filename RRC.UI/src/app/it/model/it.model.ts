
export class IT {
  Date: Date
  SourceOU: string
  SourceName: string
  Status: string
  RequestorName: string
  RequestorDepartment: string
  Subject: string
  RequestType: string
  RequestDetails: string
  Priority: any
  CreatedBy: Number
  CreatedDateTime: Date
  Action: string
  Comments: string
  Attachments: [{
    AttachmentGuid: string
    AttachmentsName: string
    ITSupportID: Number
  }]
}
