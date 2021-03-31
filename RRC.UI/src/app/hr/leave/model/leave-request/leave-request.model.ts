import { Attachment } from '../attachment/attachment.model';
import { CommunicationHistory } from '../communication-history/communication-history.model';

export class LeaveRequest {
    LeaveID?:number;
    SourceOU:any;
    SourceName:any;
    ApproverID: any;
    ApproverDepartmentID: any;
    DOANameID:any;
    DOADepartmentID:any;
    Reason: string;
    Attachments: Array<Attachment>;
    Action?: string;    
    CreatedBy?: any;
    UpdatedBy?:any;
    CreatedDateTime?:any;
    UpdatedDateTime?: any;    
    StartDate:any;
    EndDate:any;
    BalanceLeave:string;
    LeaveType:string;
    Status?: number;
    DeleteFlag?:boolean;    
    Comments?:string;
    AssigneeID?:number;
    ReferenceNumber?:string;
    LeaveCommunicationHistory?:Array<CommunicationHistory>;
    CurrentApprover?:Array<any> = [];
    HRManagerUserID?:number;
    LeaveTypeOther?:string;
}
