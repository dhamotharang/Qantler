export class masterData {
    department = [
        {
            id: 1,
            name: 'dept1'
        },
        {
            id: 2,
            name: 'dept2'
        },
        {
            id: 3,
            name: 'dept3'
        }
    ];
    memoList = [
        {LinktoMemos:'one'},
        {LinktoMemos:'onetwo'},
        {LinktoMemos:'onethree'},
        {LinktoMemos:'onefour'},
        {LinktoMemos:'onefive'},
    ]
    letterList = [
        {LinktoLetter:'one'},
        {LinktoLetter:'onetwo'},
        {LinktoLetter:'onethree'},
        {LinktoLetter:'onefour'},
        {LinktoLetter:'onefive'},
    ]
    viewData = [{
        TaskID:12,
        status:'dsd',
        SourceOU: 1,
        Source: 1,
        Tilte: "sample",
        StartDate: new Date,
        EndDate: new Date,
        Status:'dfgf',
        TaskDetails: 'sample test',
        Priority: "High",
        RemindMeAt: new Date,
        Labels: ['key'],
        CreatedBy: 1,
        CreatedDateTime: new Date,
        Action: '',
        Comments: '',
        AssigneeUserId: 2,
        AssigneeDepartmentId: 2,
        ResponsibleUserId: [
            {
                TaskResponsibleUserId: 1,
                TaskResponsibleUserName: ''
            }
        ],
        ResponsibleDepartmentId: [
            {
                TaskResponsibleDepartmentId: 2,
                TaskResponsibleDepartmentName: ''
            }
        ],
        LinktoLetterName: [
            {
                LinktoLetter: ''
            }
        ],
        LinktoMemoName: [
            {
                LinktoMemos: ''
            }
        ],
        Attachments: [
            {
                AttachmentGuid: '',
                AttachmentsName: '',
                TaskID: ''
            }
        ]
    }
    ]

}