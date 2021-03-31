export class NewBabyAddition {
  SourceOU: string
  SourceName: string
  BabyName: string
  Gender: string
  Birthday: Date
  HospitalName: string
  CountryOfBirth: number
  CityOfBirth: number
  CreatedBy: number
  CreatedDateTime: Date
  Action: string
  Comments: string
  Attachments: [
    {
      AttachmentGuid: string,
      AttachmentsName: string,
      BabyAdditionID: number
    }
  ]
}
