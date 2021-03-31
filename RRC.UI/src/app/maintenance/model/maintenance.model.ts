import { Attachment } from './attachment.model';

export class Maintenance {
  Status: string
  Date: Date
  SourceOU: string
  SourceName: string
  RequestorID: Number
  RequestorDepartmentID: Number
  Subject: string
  ApproverID: Number
  ApproverDepartmentID: Number
  RequestDetails: string
  Priority: boolean
  CreatedBy: Number
  CreatedDateTime: Date
  Action: string
  Comments: string
  Attachments: Array<Attachment>
}
