import { Component, OnInit, Input, OnDestroy, Inject, Renderer2 } from '@angular/core';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { MediapressService } from '../../service/mediapress.service';
import { CommonService } from 'src/app/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DOCUMENT, DatePipe } from '@angular/common';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { EscalateModalComponent } from 'src/app/shared/modal/escalate-modal/escalate-modal.component';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-media-press-release',
  templateUrl: './media-press-release.component.html',
  styleUrls: ['./media-press-release.component.scss']
})
export class MediaPressReleaseComponent implements OnInit,OnDestroy {
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  formData = {
    Date: new Date(),
    Action: "Submit",
    SourceOU: "",
    SourceName: "",
    Subject: "",
    Type: null,
    EventName: "",
    Location: "",
    AttendedBy: "",
    Partners: "",
    ApproverID: null,
    ApproverDepartmentID: null,
    CreatedBy: 0,
    CreatedDateTime: new Date(),
    Comments: "",
    ReferenceNumber: "",
    PressReleaseID: 0,
    Status: 0
  }
  @Input() screenStatus: String;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  approverDeptList: any = [];
  approverDept: any;
  approverList: any = [];
  typeList = [
    {"value": '1', "label": "Social"},
    {"value": '2', "label": "Formal"}
  ];
  PressReleaseID: number;
  editMode: boolean = true;
  isApprover: boolean = false;
  AssigneeID: any;
  isAssigned: boolean = false;
  isAssignedToMe: boolean = false;
  inProgress: boolean = false;
  comment: any;
  bsModalRef: BsModalRef;
  message: string;
  IsOrgHead: any;
  OrgUnitID: any;
  RequestComments: any;
  valid: boolean = false;
  CurrentApproverID: any = [];
  reSubmit: boolean = false;
  isComment: boolean = false;
  canComment: boolean = false;
  isSameApprover: boolean = false;
  ApproverID: any;
  isFirstApprover:boolean = false;
  popupMsg: string;
  lang: string;
  screenTitle: any;
  userProfileImg: string;
  commentSubscriber: any;
  constructor(private commentSectionService: CommentSectionService,
    public modalService: BsModalService,
    public router: Router,
    private route: ActivatedRoute,
    public service: MediapressService,
    public common: CommonService,
    public utils: UtilsService,
    private endpoint:EndPointService ) {
      this.formData.ApproverDepartmentID = this.currentUser.DepartmentID;
      this.onDepartmentSelect();
      this.currentUser = JSON.parse(localStorage.getItem('User'));
  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    if (this.lang == 'ar') {
      this.screenTitle = this.arabic('requestforpressrelease');
      this.common.breadscrumChange('الإعلام', this.arabic('requestforpressrelease'), '');
    } else {
      this.screenTitle = 'Request For Press Release';
      this.common.breadscrumChange('Media', 'Request for Press Release', '');
    }

    if (this.currentUser.AttachmentGuid && this.currentUser.AttachmentName) {
      this.userProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.currentUser.AttachmentName + '&guid=' + this.currentUser.AttachmentGuid;
    }else{
      this.userProfileImg = 'assets/home/user_male.png';
    }

    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : 0;

    this.route.params.subscribe(param => {
      this.PressReleaseID = +param.id;
      if (this.PressReleaseID > 0) {
        this.loadPressRelease(this.PressReleaseID);
        if (this.screenStatus == 'View') {
          if (this.lang == 'ar') {
            this.common.breadscrumChange('الإعلام', this.arabic('requestforpressreleaseview'), '');
            this.screenTitle = this.arabic('requestforpressreleaseview');
          }
          if (this.lang == 'en') {
            this.screenTitle = 'Request For Press Release View';
            this.common.breadscrumChange('Media', 'Request for Press Release View', '');
          }
          this.editMode = false;
        }
        if (this.screenStatus == "edit") {
          if (this.lang == 'ar') {
            this.screenTitle = this.arabic('requestforpressrelease');
            this.common.breadscrumChange('الإعلام', this.arabic('requestforpressrelease'), '');
          }
          if (this.lang == 'en') {
            this.screenTitle = 'Request For Press Release';
            this.common.breadscrumChange('Media', 'Request For Press Release', '');
          }
          this.editMode = true;
        }
      }
    });
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    if (this.screenStatus == 'Create') {
      this.editMode = true;
      this.initPage();
    }
    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if (newComment) {
        this.loadPressRelease(this.PressReleaseID);
      }
    });
  }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  private setRequestComments(commentSectionArr: any, parentCommunicationID?: any) {
    let recursiveCommentsArr = [];
    if (!parentCommunicationID) {
      parentCommunicationID = null;
    }
    commentSectionArr.forEach((commObj: any) => {
      if (commObj.ParentCommunicationID == parentCommunicationID) {
        let replies: any = this.setRequestComments(commentSectionArr, commObj.CommunicationID);
        if (replies.length > 0) {
          replies.forEach(repObj => {
            repObj.hideReply = true;
          });
          commObj.Replies = replies;
        }
        // commObj.UserProfileImg = commObj.Photo;
        // if(!commObj.Photo){
        //   commObj.Photo = 'assets/home/user_male.png';
        // }
        recursiveCommentsArr.push(commObj);
      }
    });
    return recursiveCommentsArr;
  }

  initPage() {
    this.canComment = true;
    this.loadApproverList();
    this.formData.SourceOU = this.currentUser.DepartmentID;
    this.formData.SourceName = this.currentUser.UserID;
    this.validate();
  }

  loadPressRelease(PressReleaseID) {
    this.service.getRequestById(PressReleaseID, this.currentUser.id)
      .subscribe((response: any) => {
        this.formData = response;
        this.approverDeptList = response.OrganizationList;
        this.formData.Type = response.Type;
        this.formData.CreatedDateTime = new Date(this.formData.CreatedDateTime);
        this.AssigneeID = response.AssigneeID;
        this.CurrentApproverID = response.CurrentApprover;
        this.RequestComments = this.setRequestComments(response.PressReleaseCommunicationHistory);
        this.ApproverID = response.ApproverID;
        this.isFirstApprover = false;
        this.onDepartmentSelect();
        this.checkIsAssignedToMe();
        this.checkIsReturn();
        this.validate();
        this.getSouceName(response.SourceName,response.SourceOU);
        this.getSouceOU(response.OrganizationList,response.SourceOU);
      });
  }

  async getSouceOU(Departments,SourceOU) {
     let params = [{
       "OrganizationID": 0,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;
       this.formData.SourceOU= Departments.find(x=> x.OrganizationID==SourceOU).OrganizationUnits;
      });

   }

  async getSouceName(UserID,DepID) {
     let params = [{
       "OrganizationID": DepID,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;
       this.formData.SourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
     });

   }

  checkIsReturn() {
    if (this.formData.Status == 77) {
      if (this.formData.CreatedBy == this.currentUser.id) {
        this.editMode = true;
        this.reSubmit = true;
        this.canComment = true;
      }
    }
    if (this.formData.Status == 76) {
      if ((this.IsOrgHead && ((this.OrgUnitID == 4) || (this.OrgUnitID == 17))) || (!this.IsOrgHead && ((this.OrgUnitID == 4) || (this.OrgUnitID == 17)))) {
        this.canComment = true;
      }
    }
    if (this.formData.Status == 78 || this.formData.Status == 81) {
      this.canComment = false;
    }
    this.checkIsApprover();
  }

  checkIsApprover() {
    if (this.CurrentApproverID && this.CurrentApproverID.length > 0 && this.formData.Status == 80) {
      this.CurrentApproverID.forEach((assignee: any) => {
        if (assignee.ApproverId == this.currentUser.id) {
          this.isApprover = true;
          this.canComment = true;
          this.isFirstApprover = true;
          if (this.formData.Status == 77 || this.formData.Status == 76) {
            this.canComment = false;
          }
        }
      });
    }
  }

  checkIsAssignedToMe() {
    if (this.AssigneeID && this.AssigneeID.length > 0) {
      this.isAssigned = true;
      this.AssigneeID.forEach((assignee: any) => {
        if (assignee.AssigneeId == this.currentUser.id) {
          this.isAssignedToMe = true;
        }
      });
    }
  }

  validate() {
    this.valid = true;
    if (this.utils.isEmptyString(this.formData.Subject)
      || this.utils.isEmptyString(this.formData.EventName)
      || this.utils.isEmptyString(this.formData.Location)
      || this.utils.isEmptyString(this.formData.AttendedBy)
      || this.utils.isEmptyString(this.formData.Partners)
      || this.utils.isEmptyString(this.formData.Type)) {
      this.valid = false;
    }
    return this.valid;
  }

  commentValid() {
    this.isComment = true;
    if (this.utils.isEmptyString(this.formData.Comments)) {
      this.isComment = false;
    }
  }

  onAssignTo() {
    if (this.lang == 'en') {
      this.popupMsg = "Press Release Request Assigned Successfully";
    } else {
      this.popupMsg = this.arabic('pressreleaseassignedmsg');
    }
    this.inProgress = true;
    const initialState = {
      id: this.PressReleaseID,
      ApiString: "/PressRelease",
      message: this.popupMsg,
      Title: "Assign To",
      redirectUrl: '/app/media/protocol-home-page',
      ApiTitleString: "Assign To",
      comments: this.formData.Comments,
    };
    if(this.common.currentLang == 'ar'){
      initialState.ApiTitleString = this.arabic('assignto');
    }
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  onEscalate() {
    if (this.lang == 'en') {
      this.popupMsg = "Press Release Request Escalated Successfully";
    } else {
      this.popupMsg = this.arabic('pressreleaseescalatemsg');
    }
    this.inProgress = true;
    let initialState = {
      id: this.PressReleaseID,
      ApiString: "/PressRelease",
      message: this.popupMsg,
      Title: "Escalate",
      comments: this.formData.Comments,
      redirectPath: '/app/media/protocol-home-page' ,
      isFirstApprover:this.isFirstApprover
    };
    this.modalService.show(EscalateModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  updateAction(action: string) {
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "Replace"
      }, {
        "value": this.formData.Comments,
        "path": "Comments",
        "op": "Replace",
      }, {
        "value": this.currentUser.id,
        "path": "UpdatedBy",
        "op": "Replace"
      }, {
        "value": new Date(),
        "path": "UpdatedDateTime",
        "op": "Replace"
      }
    ];
    this.updateRequest(dataToUpdate, action);
  }

  onDepartmentSelect() {
    if (this.formData.ApproverID)
      this.formData.ApproverID = null;
    if (!this.formData.ApproverDepartmentID)
      return;
    let params = [{
      "OrganizationID": this.formData.ApproverDepartmentID,
      "OrganizationUnits": ""
    }];
    let id = 0;
    if (this.ApproverID == this.currentUser.id) {
      id = 0;
    } else {
      id = this.currentUser.id;
    }
    this.common.getmemoUserList(params, id).subscribe((data: any) => {
      this.approverList = data;
      this.formData.ApproverID = this.ApproverID;
    });
  }

  loadApproverList() {
    this.service.getRequestById(0, this.currentUser.id)
      .subscribe((response: any) => {
        this.approverDeptList = response.M_ApproverDepartmentList;
      });
  }

  sendRequest() {
    this.inProgress = true;
    this.formData.CreatedBy = this.currentUser.id;
    this.formData.ApproverID = this.formData.ApproverID ? this.formData.ApproverID : 0;
    this.formData.ApproverDepartmentID = this.formData.ApproverDepartmentID ? this.formData.ApproverDepartmentID : 0;
    this.service.savePressRelease(this.formData)
      .subscribe((response: any) => {
        if (response.PressReleaseID) {
          if (this.lang == 'en') {
            this.message = "Press Release Request Submitted Successfully";
          } else {
            this.message = this.arabic('pressreleasesubmitmsg');
          }
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            if (this.common.language == 'English')
              this.router.navigate(['/en/app/media/protocol-home-page']);
            else
              this.router.navigate(['/ar/app/media/protocol-home-page']);
          });
        }
        this.inProgress = false;
      });
  }

  updateRequest(dataToUpdate: any, action: string) {
    this.inProgress = true;
    if (this.lang == 'en') {
      switch (action) {
        case 'Approve':
          this.message = "Press Release Request Approved Successfully";
          break;
        case 'Reject':
          this.message = "Press Release Request Rejected Successfully";
          break;
        case 'ReturnForInfo':
          this.message = "Press Release Request Returned For Info Successfully";
          break;
        case 'AssignToMe':
          this.message = "Press Release Request Assigned Successfully";
          break;
        case 'Close':
          this.message = "Press Release Request Closed Successfully";
          break;
        case 'resubmit':
          this.message = "Press Release Request Resubmitted successfully";
          break;
      }
    } else {
      switch (action) {
        case 'Approve':
          this.message = this.arabic('pressreleaseapprovedmsg');
          break;
        case 'Reject':
          this.message = this.arabic('pressreleaserejectdmsg');
          break;
        case 'ReturnForInfo':
          this.message = this.arabic('pressreleasereturnedmsg');
          break;
        case 'AssignToMe':
          this.message = this.arabic('pressreleaseassignedmsg');
          break;
        case 'Close':
          this.message = this.arabic('pressreleaseclosedmsg');
          break;
        case 'resubmit':
          this.message = this.arabic('pressreleasesubmitmsg');
          break;
      }
    }

    this.service.updateRequest(this.PressReleaseID, dataToUpdate)
      .subscribe((response: any) => {
        if (response.PressReleaseID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            if (this.common.language == 'English')
              this.router.navigate(['/en/app/media/protocol-home-page']);
            else
              this.router.navigate(['/ar/app/media/protocol-home-page']);
          });
        }
        this.inProgress = false;
      });
  }

  ReSubmit() {
    this.inProgress = true;
    if (this.lang == 'en') {
      this.message = "Press Release Request Resubmitted Successfully";
    } else {
      this.message = this.arabic('pressreleasesubmitmsg');
    }

    this.formData.PressReleaseID = this.PressReleaseID;
    if (this.AssigneeID.length == 0) {
      this.AssigneeID = 0;
    }
    var formData = {
      "PressReleaseID": this.formData.PressReleaseID,
      "Date": new Date(),
      "SourceOU": this.currentUser.DepartmentID,
      "SourceName": this.currentUser.UserID,
      "Subject": this.formData.Subject,
      "Type": this.formData.Type,
      "EventName": this.formData.EventName,
      "Location": this.formData.Location,
      "AttendedBy": this.formData.AttendedBy,
      "Partners": this.formData.Partners,
      "ApproverID": this.formData.ApproverID ? this.formData.ApproverID : 0,
      "ApproverDepartmentID": this.formData.ApproverDepartmentID ? this.formData.ApproverDepartmentID : 0,
      "AssigneeID": this.AssigneeID,
      "UpdatedBy": this.currentUser.id,
      "UpdatedDateTime": new Date(),
      "Action": "Resubmit",
      "Comments": this.formData.Comments,
    };
    this.service.resubmitRequest(formData)
      .subscribe((response: any) => {
        if (response.PressReleaseID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            if (this.common.language == 'English')
              this.router.navigate(['/en/app/media/protocol-home-page']);
            else
              this.router.navigate(['/ar/app/media/protocol-home-page']);
          });
        }
        this.inProgress = false;
      });
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}
