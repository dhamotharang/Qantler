import { Component, OnInit, OnDestroy, Input, ChangeDetectorRef } from '@angular/core';
import { BsDatepickerConfig, BsModalService, BsModalRef, getMonth } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { DatePipe } from '@angular/common';
import { Photographer } from '../../../photographer/model/photographer.model';
import { PhotographerService } from '../../service/photographer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { EscalateModalComponent } from 'src/app/shared/modal/escalate-modal/escalate-modal.component';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { UtilsService } from 'src/app/shared/service/utils.service';
import * as _ from 'lodash';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { PhotographerStatusList } from '../../enum/photographer-status-list/photographer-status-list.enum';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-photographer-request-form',
  templateUrl: './photographer-request.component.html',
  styleUrls: ['./photographer-request.component.scss']
})

export class PhotographerRequestComponent implements OnInit, OnDestroy {
  @Input() mode: string;
  date: any;
  editMode = true;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  bsModalRef: BsModalRef;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  photographerModel: Photographer = new Photographer();
  photographerRequestComments: Array<any> = [];
  photographer: any = {
    Date: '',
    SourceOU: '',
    SourceName: '',
    EventName: '',
    EventDate: '',
    Location: '',
    ApproverDepartmentID: '',
    MediaHeadUserId: null,
    ApproverID: '',
    ReferenceNumber: '',
    Status: null,
    CreatedBy: null,
    CreatedDateTime: null,
    UpdatedBy: null,
    UpdatedDateTime: null,
    Action: '',
    Comments: '',
    CurrentApprover: []
  };
  department: [];
  userApproverList: any;
  message: string;
  id: any;
  userId: any;
  reSubmit: boolean = false;
  submitBtn: boolean;
  approveBtn: boolean;
  rejectBtn: boolean;
  escalateBtn: boolean;
  returnForInfoBtn: boolean;
  assingBtn: boolean;
  assignToMeBtn: boolean;
  closeBtnShow: boolean;
  isApiLoading: boolean;
  ApproverId: any;
  inProgress: boolean = false;
  canComment: boolean;
  IsOrgHead: any;
  OrgUnitID: any;
  today: Date;
  dateAddition: Date;
  dateLimit: any;
  month: any;
  dates: string;
  allowedDateLimit: Date;
  lang: string;
  popupMsg: string;
  empProfileImg: string = 'assets/home/user_male.png';
  commentSubscriber: any;
  isFirstApprover:boolean = false;
  constructor(
    public modalService: BsModalService,
    public common: CommonService,
    public router: Router,
    public route: ActivatedRoute,
    public utils: UtilsService,
    public dropdownService: DropdownsService,
    public photographerService: PhotographerService,
    public datePipe: DatePipe,
    private changeDetector: ChangeDetectorRef,
    private commentSectionService: CommentSectionService,
    private utilsService: UtilsService,
    private endpoint: EndPointService
  ) {
    this.photographer.ApproverDepartmentID = this.currentUser.DepartmentID;
  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : 0;

    this.id = this.route.snapshot.paramMap.get("id");
    this.userId = this.currentUser.id;
    this.photographerService.getAllData(0, this.currentUser.id).subscribe((response: any) => {
      this.department = response.M_ApproverDepartmentList;
    });
    this.getApproverUserList(+this.photographer.ApproverDepartmentID);
    if (this.mode === 'create') {
      this.canComment = true;
      this.submitBtn = true;
      this.editMode = true;
      this.isApiLoading = true;
      if (this.lang == 'en') {
        this.common.breadscrumChange('Media', 'Request for Photographer', '');
      } else {
        this.common.breadscrumChange(this.arabic('media'), this.arabic('requestforphotographer'), '');
      }
    } else if (this.mode === 'view' || this.mode === 'edit') {
      this.editMode = false;
      this.getPhotographer(this.id);
      // this.common.breadscrumChange('Photographer', 'View for Photographer', '');
      if (this.lang == 'en') {
        this.common.breadscrumChange('Media', 'Request for Photographer', '');
      } else {
        this.common.breadscrumChange(this.arabic('media'), this.arabic('requestforphotographer'), '');
      }
    }
    this.today = new Date();
    this.today.setDate(this.today.getDate()+3);
    if (this.today.getMonth() < 10) {
      this.month = '0' + (this.today.getMonth() + 1);
    }else{
      this.month = this.today.getMonth() + 1;
    }
    this.dateAddition = new Date();
    this.dateLimit = (this.today.getFullYear()) + '/' + this.month + '/' + (this.today.getDate());
    this.dates = this.datePipe.transform(this.dateLimit, 'yyyy-MM-dd');
    this.allowedDateLimit = new Date(this.dates);

    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if(newComment){
        this.getPhotographer(this.id);
      }
    });

    if (this.currentUser.AttachmentGuid && this.currentUser.AttachmentName) {
      this.empProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.currentUser.AttachmentName + '&guid=' + this.currentUser.AttachmentGuid;
    }
  }

  validate() {
    this.isApiLoading = false;
    if(this.utilsService.isEmptyString(this.photographer.EventName)
      || this.utilsService.isEmptyString(this.photographer.EventDate)
      || this.utilsService.isEmptyString(this.photographer.Location)
      || !this.utilsService.isValidDate(this.photographer.EventDate)
    ) {
      this.isApiLoading = true;
    }
    return this.isApiLoading;
  }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  onChangeApproverDepartment() {
    this.getApproverUserList(+this.photographer.ApproverDepartmentID);
  }

  async getApproverUserList(id) {
    if(this.photographer.ApproverID)
      this.photographer.ApproverID = null;
    let params = [{
      'OrganizationID': id,
      'OrganizationUnits': 'string'
    }];
    let userid = 0;
    if (this.ApproverId == this.currentUser.id) {
      userid = 0;
    } else {
      userid = this.currentUser.id;
    }
    this.common.getmemoUserList(params, userid).subscribe((data: any) => {
      this.userApproverList = data;
      this.photographer.ApproverID = this.ApproverId;
    });
  }

  createPhotographer() {    
    this.inProgress = true;
    this.photographerModel.Date = new Date();
    this.photographerModel.SourceOU = this.currentUser.DepartmentID;
    this.photographerModel.SourceName = this.currentUser.UserID;
    this.photographerModel.EventName = this.photographer.EventName;
    this.photographerModel.EventDate = this.photographer.EventDate;
    this.photographerModel.Location = this.photographer.Location;
    this.photographerModel.ApproverID = this.photographer.ApproverID;
    this.photographerModel.ApproverDepartmentID = this.photographer.ApproverDepartmentID;
    this.photographerModel.CreatedBy = this.currentUser.id;
    this.photographerModel.CreatedDateTime = new Date();
    this.photographerModel.Action = 'Submit';
    this.photographerModel.Comments = this.photographer.Comments;
    this.photographerService.create(this.photographerModel).subscribe(
      (response: any) => {
        if (response.PhotoGrapherID) {
          if (this.lang == 'en') {
            this.message = 'Photographer Request Submitted Successfully';
          } else {
            this.message = this.arabic('photographersubmitmsg');
          }
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            this.inProgress = true;
            newSubscriber.unsubscribe();
            if (this.common.language == 'English')
            this.router.navigate(['/en/app/media/media-protocol-request']);
            else
            this.router.navigate(['/ar/app/media/media-protocol-request']);
          });
        }
        this.inProgress = false;
      }
    );
  }

  getPhotographer(id) {
    this.photographerService.view(id, this.currentUser.id).subscribe((res: any) => {
      if (res.PhotoGrapherID) {
        this.photographer.Date = res.Date;
        this.photographer.ReferenceNumber = res.ReferenceNumber;
        var TempSourceOU = res.OrganizationList.find(x=> x.OrganizationID == res.SourceOU)
        this.photographer.SourceOU= TempSourceOU.OrganizationUnits;
        //this.photographer.SourceName = res.SourceName;
        this.getSouceName(res.SourceName,res.SourceOU);
        this.photographer.EventName = res.EventName;
        this.photographer.EventDate = new Date(res.EventDate);
        this.photographer.Location = res.Location;
        this.photographer.ApproverID = res.ApproverID;
        this.ApproverId = res.ApproverID;
        this.photographer.ApproverDepartmentID = res.ApproverDepartmentID;
        this.photographer.MediaHeadUserId = res.MediaHeadUserId;
        this.photographer.Status = res.Status;
        this.photographer.CreatedBy = res.CreatedBy;
        this.photographer.CreatedDateTime = res.CreatedDateTime;
        this.photographer.CurrentApprover = res.CurrentApprover;
        this.photographer.AssigneeID = res.AssigneeId;
        this.getApproverUserList(+this.photographer.ApproverDepartmentID);
        if (this.photographer && this.id) {
          this.photographerRequestComments = [];
          this.photographerRequestComments = this.setPhotographerRequestComments(res.PhotographerCommunicationHistory);
        }
        this.isFirstApprover = false;
        this.buttonControl();
        this.checkIsReturn();
        this.checkIsApprover();
      }
    });
  }

  async getSouceName(UserID,DepID) {
     let params = [{
       "OrganizationID": DepID,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;
       this.photographer.SourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
     });

   }

  checkIsReturn() {
    if (this.photographer.Status == PhotographerStatusList['Pending for Resubmission']) { //Waiting for Resubmission
      if (this.photographer.CreatedBy == this.currentUser.id) {
        this.editMode = true;
        this.reSubmit = true;
        this.canComment = true;
      }
    }

    if (this.photographer.Status == PhotographerStatusList['Under Process']) { // Under Proccess
      if ((this.IsOrgHead && ((this.OrgUnitID == 4) || (this.OrgUnitID == 17))) || (!this.IsOrgHead && ((this.OrgUnitID == 4) || (this.OrgUnitID == 17)))) {
        this.canComment = true;
      } else {
        this.canComment = false;
      }
    }
    if (this.photographer.Status == PhotographerStatusList['Closed']) { //Closed
      this.canComment = false;
    }
  }

  checkIsApprover() {
    if (this.photographer.CurrentApprover && this.photographer.CurrentApprover.length > 0 && this.photographer.Status == PhotographerStatusList['Waiting for Approval']) {
      this.photographer.CurrentApprover.forEach((assignee: any) => {
        if (assignee.ApproverId == this.currentUser.id) {
          this.canComment = true;
          this.isFirstApprover = true;
        }
      });
    }
  }

  private setPhotographerRequestComments(commentSectionArr: any, parentCommunicationID?: any) {
    let recursiveCommentsArr = [];
    if (!parentCommunicationID) {
      parentCommunicationID = null;
    }
    commentSectionArr.forEach((commObj: any) => {
      if (commObj.ParentCommunicationID == parentCommunicationID) {
        let replies: any = this.setPhotographerRequestComments(commentSectionArr, commObj.CommunicationID);
        if (replies.length > 0) {
          replies.forEach(repObj => {
            repObj.hideReply = true;
          });
          commObj.Replies = replies;
        }
        // commObj.UserProfileImg = 'assets/home/user_male.png';
        // if(commObj.Photo == null){
        //   commObj.Photo = 'assets/home/user_male.png';
        // }
        recursiveCommentsArr.push(commObj);
      }
    });
    return recursiveCommentsArr;
  }

  updateAction(action: string) {
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "Replace"
      }, {
        "value": this.photographer.Comments,
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
    this.update(dataToUpdate, action);
  }

  update(dataToUpdate: any, action: string) {
    this.inProgress = true;
    if (this.lang == 'en') {
      switch (action) {
        case 'Approve':
          this.message = "Photographer Request Approved Successfully";
          break;
        case 'Reject':
          this.message = "Photographer Request Rejected Successfully";
          break;
        case 'Redirect':
          this.message = "Photographer Request Returned For Info Successfully";
          break;
        case 'AssignToMe':
          this.message = "Photographer Request Assigned Successfully";
          break;
        case 'Close':
          this.message = "Photographer Request Closed Successfully";
          break;
      }
    } else {
      switch (action) {
        case 'Approve':
          this.message = this.arabic('photographerapprovedmsg');
          break;
        case 'Reject':
          this.message = this.arabic('photographerrejectedmsg');;
          break;
        case 'Redirect':
          this.message = this.arabic('photographerreturnedmsg');
          break;
        case 'AssignToMe':
          this.message = this.arabic('photographerassignedmsg');
          break;
        case 'Close':
          this.message = this.arabic('photographerclosedmsg');
          break;
      }
    }

    this.photographerService.update(this.id, dataToUpdate)
      .subscribe((response: any) => {
        if (response.PhotoGrapherID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['/app/media/media-protocol-request']);
          });
        }
        this.inProgress = false;
      });
  }

  onEscalate() {
    if (this.lang == 'en') {
      this.popupMsg = "Photographer Request Escalated Successfully";
    } else {
      this.popupMsg = this.arabic('photographerescalatemsg');
    }
    debugger
    let initialState = {
      id: this.id,
      ApiString: "/Photographer",
      message: this.popupMsg,
      Title: "Escalate",
      comments: this.photographer.Comments,
      ApiTitleString: "Assign To",
      redirectPath: '/app/media/media-protocol-request',
      isFirstApprover: this.isFirstApprover
    };
    this.modalService.show(EscalateModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  onAssignTo() {
    if (this.lang == 'en') {
      this.popupMsg = "Photographer Request Assigned Successfully";
    } else {
      this.popupMsg = this.arabic('photographerassignedmsg');
    }
    const initialState = {
      id: this.id,
      ApiString: "/Photographer",
      message: this.popupMsg,
      Title: "Assign To",
      redirectUrl: '/app/media/media-protocol-request',
      ApiTitleString: "Assign To",
      comments: this.photographer.Comments,
    };
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      this.inProgress = false;
    });
  }

  buttonControl() {
    if (this.editMode) {
      this.submitBtn = true;
    } else if (!this.editMode) {
      this.submitBtn = false;
      this.approveBtn = false;
      this.rejectBtn = false;
      this.escalateBtn = false;
      this.returnForInfoBtn = false;
      this.assingBtn = false;
      this.assignToMeBtn = false;
      this.closeBtnShow = false;
      if (this.photographer) {
        if (this.photographer.Status == 74 && (this.photographer.CurrentApprover.findIndex(ca => ca.ApproverId == this.currentUser.id) > -1)) {
          this.approveBtn = true;
          this.rejectBtn = true;
          this.escalateBtn = true;
          this.returnForInfoBtn = true;
        }
        if ((this.OrgUnitID == 17) && this.photographer.Status == 70 && this.photographer.AssigneeID && (this.photographer.AssigneeID.length <= 0)) {
          if (this.currentUser.IsOrgHead) {
            this.assingBtn = true;
          }
          if (!this.currentUser.IsOrgHead) {
            this.assignToMeBtn = true;
          }
          this.changeDetector.detectChanges();
        }
        if ((this.OrgUnitID == 17) && (this.photographer.AssigneeID && this.photographer.AssigneeID.length > 0 && (this.photographer.AssigneeID[0].AssigneeId == this.currentUser.id) && this.photographer.Status != 72)) {
          this.closeBtnShow = true;
          this.changeDetector.detectChanges();
        }
        if (this.photographer.AssigneeID && this.photographer.AssigneeID.length > 0 && (this.photographer.AssigneeID[0].AssigneeId != this.currentUser.id) && this.photographer.Status != 72 && (this.OrgUnitID == 17)) {
          if (!this.currentUser.IsOrgHead) {
            this.assignToMeBtn = true;
          }
          if (this.currentUser.IsOrgHead) {
            this.assingBtn = true;
          }
          this.changeDetector.detectChanges();
        }
        if (this.photographer.Status == 71 && this.photographer.CreatedBy == this.currentUser.id) {
          this.submitBtn = true;
          this.changeDetector.detectChanges();
        }
        this.changeDetector.detectChanges();
      }
    }
  }
  sendMessage() {
    if (this.photographer.Comments && (this.photographer.Comments.trim() != '')) {
      this.isApiLoading = true;
      let chatData: any = {
        Message: this.photographer.Comments,
        ParentCommunicationID: 0,
        CreatedBy: this.currentUser.id,
        CreatedDateTime: new Date(),
        PhotoGrapherID: this.id
      };
      this.commentSectionService.sendComment('Photographer', chatData).subscribe((chatRes: any) => {
        this.commentSectionService.newCommentCreated(true);
        this.photographer.Comments = '';
        this.isApiLoading = false;
      });
    }
  }

  private setTrainingRequestComments(commentSectionArr: any, parentCommunicationID?: any) {
    let recursiveCommentsArr = [];
    if (!parentCommunicationID) {
      parentCommunicationID = 0;
    }
    commentSectionArr.forEach((commObj: any) => {
      if (commObj.ParentCommunicationID == parentCommunicationID) {
        let replies: any = this.setTrainingRequestComments(commentSectionArr, commObj.CommunicationID);
        if (replies.length > 0) {
          replies.forEach(repObj => {
            repObj.hideReply = true;
          });
          commObj.Replies = replies;
        }
        commObj.UserProfileImg = 'assets/home/user_male.png';
        recursiveCommentsArr.push(commObj);
      }
    });
    return recursiveCommentsArr;
  }

  userAction(actionType: string, allowSubmit?: boolean) {
    if (actionType == 'resubmit') {
      this.inProgress = true;
      if (this.photographer.AssigneeID.length == 0) {
        this.photographer.AssigneeID = 0;
      }
      var formData = {
        "PhotoGrapherID": parseInt(this.id),
        "Date": new Date(),
        "SourceOU": this.currentUser.DepartmentID,
        "SourceName": this.currentUser.UserID,
        "EventName": this.photographer.EventName,
        "EventDate": this.photographer.EventDate,
        "Location": this.photographer.Location,
        "ApproverID": this.photographer.ApproverID,
        "ApproverDepartmentID": this.photographer.ApproverDepartmentID,
        "AssigneeID": this.photographer.AssigneeID,
        "UpdatedBy": this.currentUser.id,
        "UpdatedDateTime": new Date(),
        "Comments": this.photographer.Comments,
        "Action": 'Resubmit'
      };
      if (this.lang == 'en') {
        this.message = 'Photographer Request Resubmitted Successfully';
      } else {
        this.message = this.arabic('photographerresubmitmsg');
      }
      this.photographerService.reSubmission(formData).subscribe((res: any) => {
        if (res.PhotoGrapherID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(r => {
            newSubscriber.unsubscribe();
            this.router.navigate(['app/media/media-protocol-request']);
          });
        }
        this.inProgress = false;
      });
    } else {
      let toSendData: any;
      if (((this.photographer.Comments) && (this.photographer.Comments.trim() != ''))) {
        if (actionType == 'reject') {
          if (this.lang == 'en') {
            this.message = 'Photographer Request Rejected Successfully';
          } else {
            this.message = this.arabic('photographerrejectedmsg');
          }
           this.updateAction('Reject');
        }
        if (actionType == 'escalate') {
          if (this.lang == 'en') {
            this.message = 'Photographer Request Escalated Successfully';
          } else {
            this.message = this.arabic('photographerescalatemsg');
          }
          this.utilsService.currentRedirectUrl = '/app/media/media-protocol-request';
          let initialState = {
            id: this.id,
            ApiString: "/Photographer",
            message: this.message,
            Title: "Escalate",
            comments: this.photographer.Comments,
            redirectPath: '/app/media/media-protocol-request',
            isFirstApprover : this.isFirstApprover
          };
          this.modalService.show(EscalateModalComponent, Object.assign({}, {}, { initialState }));
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.inProgress = false;
          });
        }
        if (actionType == 'returnforinfo') {
          if (this.lang == 'en') {
          this.message = 'Photographer Request Returned For Info Successfully';
          } else {
            this.message = this.arabic('photographerreturnedmsg');
          }
          this.updateAction('ReturnForInfo');
        }
      }
      if (actionType == 'assign') {
        this.onAssignTo();
      }
      if (actionType == 'approve') {
        if (this.lang == 'en') {
          this.message = 'Photographer Request Approved Successfully';
        } else {
          this.message = this.arabic('photographerapprovedmsg');
        }
        toSendData = this.updateAction('Approve');
      }
      if (actionType == 'assigntome') {
        if (this.lang == 'en') {
          this.message = 'Photographer Request Assigned Successfully';
        } else {
          this.message = this.arabic('photographerassignedmsg');
        }
        this.updateAction('AssignToMe');
      }
      if (actionType == 'close') {
        this.isApiLoading = true;
        if (this.lang == 'en') {
          this.message = 'Photographer Request Closed Successfully';
        } else {
          this.message = this.arabic('photographerclosedmsg');
        }
        this.updateAction('Close');
      }
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}
