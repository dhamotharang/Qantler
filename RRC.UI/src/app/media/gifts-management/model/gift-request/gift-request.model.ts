export class GiftRequest {
    GiftID?:number;
    ReferenceNumber?:any;
    SourceOU?: string;
    SourceName?: string;
    CreatedBy?: number;
    UpdatedBy?: number;
    CreatedDateTime?: Date;
    UpdatedDateTime?: Date;
    Status?: number;
    Action?:string;
    MediaManagerUserID?:any;
    Attachments?:Array<any>;
    GiftType:any;
    PurchasedBy?:any;
    PurchasedToName?:any;
    PurchasedToOrganisation?:any;
    RecievedDate?:any;
    RecievedFromName?:any;
    RecievedFromOrganization?:any;
    IsSend?:any;
    GiftPhotos?:any;
    HistoryLog?:any;
}
