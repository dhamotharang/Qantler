export class CreateLetterModal {
     LetterID: number
     LetterReferenceNumber: string
     Title: string
     SourceOU: string
     SourceName: string
     DestinationUserId: any = []
     DestinationDepartmentId: any = []
     DestinationEntity: any = []
     ApproverId: number
     ApproverDepartmentId: number
     ReceivingDate: any
     ReceivedFromGovernmentEntity: string
     ReceivedFromName: string
     RelatedToIncomingLetter: string
     IsGovernmentEntity: boolean
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
     RelatedIncomingLetters = []
     Attachments = [
          {
               AttachmentGuid: "string",
               AttachmentsName: "string",
               MemoID: "string"
          }
     ]
     CreatedBy: number
     CreatedDateTime: Date
     UpdatedBy: number
     UpdatedDateTime: Date
     Action: string
     IsRedirect:Boolean
     Comments: string
}

