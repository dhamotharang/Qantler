export class CreateLetterModal {
     LetterID: number
     LetterReferenceNumber: string
     Title: string
     SourceOU: string
     SourceName: string
     DestinationUserId: any = []
     DestinationDepartmentId: any = []
     ApproverId: number
     ApproverDepartmentId: number
     ReceivingDate: any
     ReceivedFromGovernmentEntity: string
     ReceivedFromName: string
     ReceivedFromEntityID:number
     IsGovernmentEntity:boolean
     RelatedIncomingLetters = []
     LetterDetails: string
     Notes: string
     DocumentClassification: string
     NeedReply: string
     LetterPhysicallySend: string
     LetterType: string
     Details: string
     Private: string
     Priority: string
     Keywords = []
     RelatedOutgoingLetters = []

     Attachments = [
          {
               AttachmentGuid: "string",
               AttachmentsName: "string",
               MemoID: "string"
          }
     ]
     CreatedBy: number
     UpdatedBy: number
     UpdatedDateTime: Date
     CreatedDateTime: Date
     Action: string
     Comments: string
}

