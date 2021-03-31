export class TrainingRequest {
  SourceOU: string;
  SourceName: string;
  TrainingFor: boolean;
  TraineeName: any;
  TrainingName: string;
  StartDate: Date;
  EndDate: Date;
  ApproverID: Number;
  ApproverDepartmentID: Number;
  CreatedBy: Number;
  CreatedDateTime: Date;
  Action: string;
  Comments: string;
  CurrentApprover:any[];
  TrainingID?:any;
  UpdatedBy?:any;
  UpdatedDateTime?:any;
}

