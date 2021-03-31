export class SampleData{
    DBData={
    approval:[
        {
             MemoID: 0,
             ReferenceNumber: '123123',
             Title: 'approval',
             SourceOU: 'dept1',
             SourceName: 'testUser3',
             DestinationOU: 'dept2',
             DestinationUsername: 'testUser3',
             ApproverName: 'testUser2',
             ApproverDepartment: 'dept2',
             Details: 'hi',
             Private: 'yes',
             Priority: 'high',
             Keywords: '[{"display":"df","value":"df"},{"display":"s","value":"s"}]',
             Comment: '',
             Attachments: '["Penguins.jpg","Tulips.jpg","Hydrangeas.jpg"]',
             AttachmentName: '',
             DeleteFlag: '',
             CreatedBy: 'testUser1',
             UpdatedBy: '',
             CreatedDateTime: new Date,
             UpdatedDateTime: '',
             Status:'Waiting for Approval',
          }
    ],
    draft:[
        {
             MemoID : 0,
             ReferenceNumber :  123123 ,
             Title :  'approval' ,
             SourceOU :  'dept1' ,
             SourceName :  'testUser1' ,
             DestinationOU :  'dept2' ,
             DestinationUsername :  'testUser2' ,
             ApproverName :  'testUser2' ,
             ApproverDepartment :  'dept2' ,
             Details :  'hi' ,
             Private :  'true' ,
             Priority :  'high' ,
             Keywords :  'sdf' ,
             Comment :  '' ,
             Attachments :  '' ,
             AttachmentName :  '' ,
             DeleteFlag : false,
             CreatedBy :  'testUser1' ,
             UpdatedBy :  '' ,
             CreatedDateTime :  '2019-05-31T06:57:30.866Z' ,
             UpdatedDateTime :  '2019-05-31T06:57:30.866Z' ,
             Status :  'Draft' 
          }
    ]
    // draft:[
    //     {
    //          MemoID : 0,
    //          ReferenceNumber :  123123 ,
    //          Title :  'approval' ,
    //          SourceOU :  'dept1' ,
    //          SourceName :  'testUser1' ,
    //          DestinationOU :  'dept2' ,
    //          DestinationUsername :  'testUser2' ,
    //          ApproverName :  'testUser2' ,
    //          ApproverDepartment :  'dept2' ,
    //          Details :  'hi' ,
    //          Private :  'true' ,
    //          Priority :  'high' ,
    //          Keywords :  'sdf' ,
    //          Comment :  '' ,
    //          Attachments :  '' ,
    //          AttachmentName :  '' ,
    //          DeleteFlag : false,
    //          CreatedBy :  'testUser1' ,
    //          UpdatedBy :  '' ,
    //          CreatedDateTime :  '2019-05-31T06:57:30.866Z' ,
    //          UpdatedDateTime :  '2019-05-31T06:57:30.866Z' ,
    //          Status :  'Draft' Under Progress
    //       }
    // ]
}
}