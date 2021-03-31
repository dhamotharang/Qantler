import { Attachment } from './attachment.model';

export class Campaign {
  Date: Date
  SourceOU: string
  SourceName: string
  InitiativeProjectActivity: string
  CampaignStartDate: Date
  CampaignPeriod: string
  DiwansRole: string
  OtherEntities: string
  TargetAudience: string
  Location: string
  Languages: string
  MediaChannels: string
  Notes: string
  RequestDetails: string
  GeneralInformation: string
  MainObjective: string
  MainIdea: string
  StrategicGoals: string
  ApproverID: Number
  ApproverDepartmentID:Number
  CreatedBy: Number
  CreatedDateTime: Date
  Action: string
  Comments: string
  Attachments: Array<Attachment>
  CampaignID: Number
  AssigneeID: any
  UpdatedBy: Number
  UpdatedDateTime: Date
  DeleteFlag: Boolean
}