import { LegalCommunicationHistory } from '../legal-communication-history/legal-communication-history.model';

export class LegalRequest {
    LegalID?:number;
    ReferenceNumber?:number;
    // LegalStatus:string;
    SourceOU:string;
    SourceName:string;
    Subject:string;
    RequestDetails:string;
    Attachments :[];
    Keywords?:Array<{"Keywords":string}>;  
    Action?: string;
    CreatedBy?: any;
    UpdatedBy?:any;
    CreatedDateTime?:any;
    UpdatedDateTime?: any;
    Status?: number;
    DeleteFlag?:boolean;    
    Comments?:string;
    LegalCommunicationHistory?:Array<LegalCommunicationHistory>;
    AssigneeID?:any;
}
