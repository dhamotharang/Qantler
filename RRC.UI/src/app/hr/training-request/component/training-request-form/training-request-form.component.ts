import { Component, OnInit, OnDestroy, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { TrainingRequestService } from '../../service/training-request.service';
import { TrainingRequest } from '../../model/training-request.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { EscalateModalComponent } from 'src/app/shared/modal/escalate-modal/escalate-modal.component';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpEventType } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-training-request-form',
  templateUrl: './training-request-form.component.html',
  styleUrls: ['./training-request-form.component.scss']
})
export class TrainingRequestFormComponent implements OnInit,OnDestroy {
  editMode: boolean = true;
  id: string = '';
  userId: string = '';
  trainingRequestComments: Array<any> = [];
  trainingRequest: any = {
    RefNo: "",
    SourceOU: "",
    SourceName: "",
    Date: null,
    TrainingFor: null,
    WorkshopName: "",
    TraineeName: "",
    TrainingName: "",
    StartDate: null,
    EndDate: null,
    ApproverID: null,
    ApproverDepartmentID: null,
    HRManagerUserID: null,
    HRTeamUserID: null,
    Status: null,
    CreatedBy: null,
    CreatedDateTime: null,
    UpdatedBy: null,
    UpdatedDateTime: null,
    Action: '',
    Comments: '',
    CurrentApprover: [],
    Attachments:[]
  };
  historyLogs: [] = [];
  userList: any;
  attachment: string;
  trainingRequestData: TrainingRequest = new TrainingRequest();

  bsModalRef: BsModalRef;
  message: string = '';
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  screenStatus: string;
  userDOAList: any;
  department = [];
  approverDepartment = [];
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  userApproverList: any;
  submitBtn: boolean;
  approveBtn: boolean;
  rejectBtn: boolean;
  escalateBtn: boolean;
  returnForInfoBtn: boolean;
  assingBtn: boolean;
  assignToMeBtn: boolean;
  closeBtnShow: boolean;
  isApiLoading: boolean;
  popupvar :boolean=false;
  lang: string;
  isHRDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 9;
  isHRDepartmentTeamUserID = this.currentUser.OrgUnitID == 9 && !this.currentUser.IsOrgHead;
  isFirstApprover:boolean = false;
  commentSubscriber: any;
  userDetail: any;
  cur_user: string;
  uploadProcess: boolean;
  uploadPercentage: number;
  attachments: any = [];
  attachmentDownloadUrl = environment.AttachmentDownloadUrl;
  isHrHead:boolean=false;
  @ViewChild('variable') myInputVariable: ElementRef;

  constructor(private route: ActivatedRoute,
    public datePipe: DatePipe,
    public modalService: BsModalService,
    private trainingRequestService: TrainingRequestService,
    public common: CommonService, public router: Router,
    private endpoint: EndPointService,
    private changeDetector: ChangeDetectorRef, private commentSectionService: CommentSectionService,
    private utilsService: UtilsService, public dropdownService: DropdownsService) {
    this.currentUser.userProfileImg = '/../assets/home/user_male.png';
    this.currentUser.userName = this.currentUser.user;
    this.trainingRequest.ApproverDepartmentID = this.currentUser.DepartmentID;
  }

  ngOnInit() {
    debugger;
    this.lang = this.common.currentLang;
   
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.id = this.route.snapshot.paramMap.get("id");
    this.userId = this.currentUser.id;
    if (this.id) {
      if (this.lang == 'en') {
        this.common.breadscrumChange('HR', 'Training Request', '');
      } else {
        this.common.breadscrumChange(this.arabic('humanresource'), this.arabic('trainingrequest'), '');
      }
      this.editMode = false;
      this.getTrainingRequest();
    } else {
      if (this.lang == 'en') {
        this.common.breadscrumChange('HR', 'Training Request', '');
      } else {
        this.common.breadscrumChange(this.arabic('humanresource'), this.arabic('trainingrequest'), '');
      }
      this.editMode = true;
      this.initiateForm();
      this.isApiLoading = true;
    }
    this.loadEmpList();
    this.trainingRequestService.getTraining(0, this.currentUser.id).subscribe((data: any) => {
      this.department = data.OrganizationList;
      this.approverDepartment = data.M_ApproverDepartmentList;
    });
    this.getApproverUserList(+this.trainingRequest.ApproverDepartmentID);
    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if(newComment){
        this.getTrainingRequest();
      }
    });

    // getting user profile picture
    this.userDetail = JSON.parse(localStorage.getItem('User'));
    this.cur_user = this.userDetail.username;
    if (this.userDetail.AttachmentGuid && this.userDetail.AttachmentName) {
      this.currentUser.userProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.userDetail.AttachmentName + '&guid=' + this.userDetail.AttachmentGuid;
    }
  }

  initiateForm() {
    this.trainingRequest.Action = 'Submit';
    this.buttonControl();
  }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  loadEmpList() {
    debugger;
    let orgData;
    if(this.id)
    orgData = { OrganizationID: this.trainingRequest.TrainingNameDepartmentID, OrganizationUnits: this.currentUser.department };
    else
    orgData = { OrganizationID: this.currentUser.DepartmentID, OrganizationUnits: this.currentUser.department };
    
    this.dropdownService.getUserList(orgData)
      .subscribe((userList: any) => {
        if (userList != null) {
          this.userList = userList;
        }
         if(this.id !='0')
         {
           this.trainingRequest.TraineeName=Number(this.trainingRequest.TraineeName);
         }
      });
  }

  validate(test?:any) {
    this.isApiLoading = false;
    if (this.utilsService.isEmptyString(this.trainingRequest.TrainingFor)
      || this.utilsService.isEmptyString(this.trainingRequest.TraineeName)
      || (this.utilsService.isEmptyString(this.trainingRequest.StartDate)
      || this.utilsService.isEmptyString(this.trainingRequest.EndDate) || !this.checkStartEndDiff())
      || this.utilsService.isEmptyString(this.trainingRequest.ApproverDepartmentID)
      || this.utilsService.isEmptyString(this.trainingRequest.ApproverID)
    ) {
      this.isApiLoading = true;
    }
  }

  resetTraineeName() {
    if (this.trainingRequest.TrainingFor == "myself") {
      this.trainingRequest.TraineeName = this.currentUser.id;
    } else if (this.trainingRequest.TrainingFor == "other employee") {
      this.trainingRequest.TraineeName = '';
    }
    this.validate();
  }

  getTrainingRequest() {
    this.trainingRequestService.getTraining(this.id, this.userId).subscribe((data: any) => {
      if (data.TrainingID) {
        this.trainingRequest.RefNo = data.ReferenceNumber;
        this.trainingRequest.Date = new Date(data.CreatedDateTime);
        this.getSouceName(data.SourceName,data.SourceOU);
        this.trainingRequest.SourceOU = data.OrganizationList.find(x=> x.OrganizationID == data.SourceOU).OrganizationUnits
        // this.trainingRequest.SourceOU = data.SourceOU;
        // this.trainingRequest.SourceName = data.SourceName;
        this.trainingRequest.TrainingFor = data.TrainingFor ? "myself" : "other employee";
        this.trainingRequest.TraineeName = data.TraineeName ? data.TraineeName : '';
        this.trainingRequest.TrainingName = data.TrainingName;
        this.trainingRequest.TrainingNameDepartmentID = data.TraineeDepartmentID;
        this.trainingRequest.StartDate = new Date(data.StartDate);
        this.trainingRequest.EndDate = new Date(data.EndDate);
        this.trainingRequest.ApproverID = data.ApproverID;
        this.trainingRequest.ApproverDepartmentID = data.ApproverDepartmentID;
        this.trainingRequest.HRManagerUserID = data.HRManagerUserID;
        this.trainingRequest.HRTeamUserID = data.HRTeamUserID;
        this.trainingRequest.Status = data.Status;
        this.trainingRequest.CreatedBy = data.CreatedBy;
        this.trainingRequest.CreatedDateTime = data.CreatedDateTime;
        this.trainingRequest.UpdatedBy = data.UpdatedBy;
        this.trainingRequest.UpdatedDateTime = data.UpdatedDateTime;
        this.trainingRequest.CurrentApprover = data.CurrentApprover;
        this.trainingRequest.AssigneeID = data.AssigneeID;
        this.trainingRequest.Attachments = data.Attachments;
        this.trainingRequest.IsNotificationReceived = data.IsNotificationReceived;
        this.attachments = data.Attachments;
        this.historyLogs = data.HistoryLog;
        this.getApproverUserList(+this.trainingRequest.ApproverDepartmentID);
        this.buttonControl();
        this.loadEmpList();
        if (this.trainingRequest && this.id) {
          this.trainingRequestComments = [];
          this.trainingRequestComments = this.setTrainingRequestComments(data.TrainingCommunicationHistory);
        }
        console.log(this.currentUser.UserID);
        console.log(this.trainingRequest.TraineeName);
        if(this.currentUser.OrgUnitID == 9 && this.currentUser.HOU == true && this.currentUser.UserID != this.trainingRequest.TraineeName)
        this.isHrHead=true;

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
       this.trainingRequest.SourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
     });
   }

  createTrainingRequst() {
    this.isApiLoading = true;
    this.trainingRequestData.SourceOU = this.currentUser.DepartmentID;
    this.trainingRequestData.SourceName = this.currentUser.UserID;
    this.trainingRequestData.TrainingFor = this.trainingRequest.TrainingFor == "myself" ? true : false;
    this.trainingRequestData.TraineeName = this.trainingRequestData.TrainingFor ? this.currentUser.id : this.trainingRequest.TraineeName;
    this.trainingRequestData.TrainingName = this.trainingRequest.TrainingName;
    this.trainingRequestData.StartDate = this.trainingRequest.StartDate;
    this.trainingRequestData.EndDate = this.trainingRequest.EndDate;
    this.trainingRequestData.ApproverID = this.trainingRequest.ApproverID;
    this.trainingRequestData.ApproverDepartmentID = this.trainingRequest.ApproverDepartmentID;
    this.trainingRequestData.CreatedBy = this.currentUser.id;
    this.trainingRequestData.CreatedDateTime = new Date();
    this.trainingRequestData.Action = this.trainingRequest.Action;
    this.trainingRequestData.Comments = this.trainingRequest.Comments;
    this.trainingRequestService.create(this.trainingRequestData).subscribe((created: any) => {
      if (created.TrainingID) {
        this.message = "Training Request Submitted Successfully.";
        if (this.lang == 'ar') {
          this.message = this.arabic('trainingreqcreatemsg');
        }
        this.bsModalRef = this.modalService.show(SuccessComponent);
        this.bsModalRef.content.message = this.message;
        let newSubscriber = this.modalService.onHide.subscribe(() => {
          newSubscriber.unsubscribe();
          this.router.navigate(['app/hr/dashboard']);
        });
      }
    });
  }

  onClose() {
    this.message = (this.lang == 'en' ? 'Training Request Closed Successfully' : this.arabic('trainingreqclosemsg'));
    this.updateAction('Close');
  }

  onAssigneTo() {
    let initialState;
    if(this.popupvar)
     {
      this.message = (this.lang == 'en' ? "Training Request Approved and Assigned Successfully" : this.arabic('trainingreqapproveassignmsg'));
      initialState = {
        id: this.id,
        ApiString: "/Training",
        message: this.message,
        ApiTitleString: "Assign To",
        redirectUrl: '/app/hr/dashboard',
        comments: this.trainingRequest.Comments
      };
     }
     else{
      this.message = (this.lang == 'en' ? 'Training Request Assigned Successfully' : this.arabic('trainingreqassignmsg'));
      initialState = {
        id: this.id,
        ApiString: "/Training",
        message: this.message,
        ApiTitleString: "Assign To",
        redirectUrl: '/app/hr/dashboard',
        comments: this.trainingRequest.Comments
      };
     }
    this.modalService.show(AssignModalComponent, Object.assign({}, {}, { initialState }));
    let newSubscriber = this.modalService.onHide.subscribe(() => {
      newSubscriber.unsubscribe();
      // if(this.popupvar) {
      //   this.router.navigate(['app/hr/dashboard']);
      // }
    });
    this.isApiLoading = false;
  }

  onAssigneToMe() {
    this.message = (this.lang == 'en' ? 'Training Request Assigned Successfully' : this.arabic('trainingreqassignmsg'));
    this.updateAction('AssignToMe');
  }

  updateTrainingAttendance(action: string, comments?: string)
  {

    this.trainingRequestService.AddTrainingAttendance(this.id,this.userId, this.attachments)
    .subscribe((response: any) => {
      if(!this.popupvar)
          {this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;}
          this.getTrainingRequest();
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
              this.router.navigate(['app/hr/dashboard']);
          });          
    });
  }

  updateAction(action: string, comments?: string) {
    const dataToUpdate = [{
      "value": action,
      "path": "Action",
      "op": "replace"
    }, {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "replace"
    }, {
      "value": comments,
      "path": "Comments",
      "op": "replace"
    }];
    this.update(dataToUpdate);
  }

  update(dataToUpdate: any) {
    this.trainingRequestService.update(this.id, dataToUpdate)
      .subscribe((response: any) => {
        if (response.TrainingID) {
          if(this.trainingRequest.Status == 43 &&dataToUpdate[0].value == 'Approve')
          {
            this.popupvar=true;
            this.onAssigneTo();
          }

          if(!this.popupvar)
          {
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['app/hr/dashboard']);
             });
          }
          this.getTrainingRequest();        
            // if(dataToUpdate[0].value == 'Approve' && response.HRManagerUserID == this.currentUser.id){
            //   this.onAssigneTo();
            // }
        }
      });
  }

  onChangeApproverDepartment() {
    this.getApproverUserList(+this.trainingRequest.ApproverDepartmentID);
    this.validate();
  }

  async getApproverUserList(id) {
    let params = [{
      "OrganizationID": id,
      "OrganizationUnits": "string"
    }];
    if(!this.editMode)
    {
      this.common.getmemoUserList(params, 0).subscribe((data: any) => {
      this.userApproverList = data;
    });}
    else
    {
      this.common.getmemoUserList(params,this.currentUser.id).subscribe((data: any) => {
        this.userApproverList = data;
      });
    }
  }

  buttonControl() {
    if (this.editMode) {
      this.submitBtn = true;
    } else if (!this.editMode) {
      this.changeDetector.detectChanges();
      this.submitBtn = false;
      this.approveBtn = false;
      this.rejectBtn = false;
      this.escalateBtn = false;
      this.returnForInfoBtn = false;
      this.assingBtn = false;
      this.assignToMeBtn = false;
      this.closeBtnShow = false;
      this.isFirstApprover = false;
      if (this.trainingRequest) {
        if (this.trainingRequest.Status == 42 && (this.trainingRequest.CurrentApprover.findIndex(ca => ca.ApproverId == this.currentUser.id) > -1)) {
          this.approveBtn = true;
          this.rejectBtn = true;
          this.escalateBtn = true;
          this.returnForInfoBtn = true;
          this.isFirstApprover = true;
        }
        if (this.trainingRequest.Status == 43 && !this.trainingRequest.HRManagerUserID && (this.currentUser.OrgUnitID == 9 && this.currentUser.IsOrgHead)) {
          this.approveBtn = true;
          this.rejectBtn = true;
          this.escalateBtn = false;
          this.returnForInfoBtn = true;
        }
        if (this.trainingRequest.Status == 43 && !this.trainingRequest.AssigneeID && this.trainingRequest.HRManagerUserID) {
          if (this.isHRDepartmentHeadUserID) {
            this.assingBtn = true;
          } else if(this.isHRDepartmentTeamUserID) {
            this.assignToMeBtn = true;
          }
          this.changeDetector.detectChanges();
        }
        if((this.trainingRequest.AssigneeID && this.trainingRequest.AssigneeID === this.currentUser.id && this.trainingRequest.Status != 45)) {
          this.closeBtnShow = true;
          this.changeDetector.detectChanges();
        }
        if((this.trainingRequest.AssigneeID && this.trainingRequest.AssigneeID !== this.currentUser.id && this.trainingRequest.Status != 45)) {
          if (this.isHRDepartmentHeadUserID) {
            this.assingBtn = true;
          } else if(this.isHRDepartmentTeamUserID) {
            this.assignToMeBtn = true;
          }
          this.changeDetector.detectChanges();
        }
        if (this.trainingRequest.Status == 44 && this.trainingRequest.CreatedBy == this.currentUser.id) {
          this.submitBtn = true;
          this.changeDetector.detectChanges();
        }
        this.changeDetector.detectChanges();
      }
    }
  }
  sendMessage() {
    if (this.trainingRequest.Comments && (this.trainingRequest.Comments.trim() != '')) {
      this.isApiLoading = true;
      let chatData: any = {
        Message: this.trainingRequest.Comments,
        ParentCommunicationID: 0,
        CreatedBy: this.currentUser.id,
        CreatedDateTime: new Date(),
        TrainingID: this.id
      };
      this.commentSectionService.sendComment('Training', chatData).subscribe((chatRes: any) => {
        this.commentSectionService.newCommentCreated(true);
        this.trainingRequest.Comments = '';
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
    this.isApiLoading = true;
    if (actionType == 'submit') {
      this.trainingRequestData.SourceOU = this.currentUser.DepartmentID;
      this.trainingRequestData.SourceName = this.currentUser.UserID;
      this.trainingRequestData.TrainingFor = this.trainingRequest.TrainingFor == "myself" ? true : false;
      this.trainingRequestData.TraineeName = this.trainingRequest.TrainingFor ? this.currentUser.id : this.trainingRequest.TraineeName;
      this.trainingRequestData.TrainingName = this.trainingRequest.TrainingName;
      this.trainingRequestData.StartDate = this.trainingRequest.StartDate;
      this.trainingRequestData.EndDate = this.trainingRequest.EndDate;
      this.trainingRequestData.ApproverID = this.trainingRequest.ApproverID;
      this.trainingRequestData.ApproverDepartmentID = this.trainingRequest.ApproverDepartmentID;
      this.trainingRequestData.CreatedBy = this.currentUser.id;
      this.trainingRequestData.CreatedDateTime = new Date();
      this.trainingRequestData.Action = this.trainingRequest.Action;
      this.trainingRequestData.Comments = this.trainingRequest.Comments;
      if (this.id) {
        this.trainingRequestData.TrainingID = parseInt(this.id);
        this.trainingRequestData.UpdatedBy = this.currentUser.id;
        this.trainingRequestData.UpdatedDateTime = new Date();
        this.trainingRequestData.Action = 'Resubmit';
        this.message = 'Training Request Resubmitted Successfully';
        if (this.lang == 'ar') {
          this.message = this.arabic('trainingrequpdatemsg');
        }
        this.trainingRequestService.reSubmission(this.trainingRequestData).subscribe((trainingRes: any) => {
          if (trainingRes.TrainingID) {
            this.bsModalRef = this.modalService.show(SuccessComponent);
            this.bsModalRef.content.message = this.message;
            let newSubscriber = this.modalService.onHide.subscribe(r => {
              newSubscriber.unsubscribe();
              var lang = (this.common.language == 'English') ? 'en' : 'ar',
                redirectURL = '/' + lang + '/app/hr/dashboard';
              this.router.navigate([redirectURL]);
            });
          }
        });
      } else {
        this.createTrainingRequst();
      }
    } else {
      let toSendData: any;
      if (((this.trainingRequest.Comments) && (this.trainingRequest.Comments.trim() != ''))) {
        if (actionType == 'reject') {
          this.message = 'Training Request Rejected Successfully';
          if (this.lang == 'ar') {
            this.message = this.arabic('trainingreqrejectmsg');
          }
          this.updateAction('Reject', this.trainingRequest.Comments);
        }
        if (actionType == 'escalate') {
          this.message = 'Training Request Escalated Successfully';
          if (this.lang == 'ar') {
            this.message = this.arabic('trainingreqescalatemsg');
          }
          this.utilsService.currentRedirectUrl = '/app/hr/dashbaord';
          let initialState = {
            id: this.id,
            ApiString: "/Training",
            message: "Training Request Escalated Successfully",
            Title: "Escalate",
            comments: this.trainingRequest.Comments,
            redirectPath: 'app/hr/dashboard',
            isFirstApprover:this.isFirstApprover
          };
          if (this.lang == 'ar') {
            initialState.message = this.arabic('trainingreqescalatemsg');
          }
          this.modalService.show(EscalateModalComponent, Object.assign({}, {}, { initialState }));
          this.isApiLoading = false;
        }
        if (actionType == 'returnforinfo') {
          this.message = 'Training Request Returned For Info Successfully';
          if (this.lang == 'ar') {
            this.message = this.arabic('trainingreqreturnedmsg');
          }
          this.updateAction('ReturnForInfo', this.trainingRequest.Comments);
        }
      }
      if (actionType == 'assign') {
        this.onAssigneTo();
      }
      if (actionType == 'approve') {
        this.message = 'Training Request Approved Successfully';
        if (this.lang == 'ar') {
          this.message = this.arabic('trainingreqapprovedmsg');
        }
        toSendData = this.updateAction('Approve', this.trainingRequest.Comments);
      }
      if (actionType == 'assigntome') {
        this.message = 'Training Request Assigned Successfully.';
        if(this.lang != 'en'){
          this.message = this.arabic('trainingreqassignmsg');
        }
        this.updateAction('AssignToMe', this.trainingRequest.Comments);
      }
      if (actionType == 'close') {
        this.message = 'Training Request Closed Successfully';
        if(this.lang != 'en'){
          this.message = this.arabic('trainingreqclosemsg');
        }
        this.updateAction('Close', this.trainingRequest.Comments);
      }
      if (actionType == 'SAVE') {
        this.message = 'Proof of Attendance Uploaded Successfully ';
        if(this.lang != 'en'){
          this.message = this.arabic('trainingreqattendencesavemsg');
        }
        this.updateTrainingAttendance('SAVE', this.trainingRequest.Comments);
      }
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  checkStartEndDiff(){
    let toRetVal = false;
    if(this.utilsService.isValidDate(this.trainingRequest.StartDate)
    && this.utilsService.isValidDate(this.trainingRequest.EndDate)){
      if(this.trainingRequest.StartDate.getTime() <= this.trainingRequest.EndDate.getTime()){
        toRetVal =  true;
      }
    }
    return toRetVal;
  }

  dateChange(eve,isStartEnd){
    if(eve){
      if(isStartEnd == 'start'){
        this.trainingRequest.StartDate = eve;
      }
      if(isStartEnd == 'end'){
        this.trainingRequest.EndDate = eve;
      }
      this.validate();
    }
  }

  handleFileUpload(event) {
    var files = event.target.files;
    if (files.length > 0) {
      this.uploadProcess = true;
      this.trainingRequestService.uploadAttachment(files).subscribe((event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'TrainingId': '','CreatedBy':this.userId });
          }
          this.trainingRequest.Attachments = this.attachments;
          this.myInputVariable.nativeElement.value = "";
          this.attachment='';
        }
      });
    }
  }

  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    if (this.attachments.length == 0) {
      this.myInputVariable.nativeElement.value = "";
    }
  }


}
