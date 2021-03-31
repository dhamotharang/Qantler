export class DiwanIdentityRequest {
    ReferenceNumber?:any;
    DiwanIdentityID?:number;
    SourceOU: string;
    SourceName: string;
    ApproverID?: number;
    ApproverDepartmentID?: number;
    PurposeofUse: string;
    CreatedBy?: number;
    UpdatedBy?: number;
    CreatedDateTime?: Date;
    UpdatedDateTime?: Date;
    Status?: number;
    Action?:string;
    Comments?:string;
    CurrentApprover?:any;
    MediaManagerUserID?:any;
    AssigneeId?:any;
    DiwanIdentityCommunicationHistory?:any[];
}
