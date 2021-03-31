import { Component, OnInit, OnDestroy, TemplateRef, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { ModalComponent } from '../../../../modal/modalcomponent/modal.component';
import { CommonService } from '../../../../common.service';
import { DatePipe } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { MediaRequestDesignService } from './media-request-design.service';
import { AssignModalComponent } from '../../../../modal/assignmodal/assignmodal.component';
import { HttpEventType } from '@angular/common/http';
import { CreateDesignModal } from './media-request-design.modal';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { EndPointService } from 'src/app/api/endpoint.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-media-request-design-form',
  templateUrl: './media-request-design-form.component.html',
  styleUrls: ['./media-request-design-form.component.scss']
})
export class MediaRequestDesignFormComponent implements OnInit,OnDestroy {

  bsConfig: Partial<BsDatepickerConfig>;
  bsConfigs: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  @ViewChild('template') template: TemplateRef<any>;
  @ViewChild('variable') myInputVariable: ElementRef;
  @ViewChild('printContent') printContent: ElementRef<any>;
  bsModalRef: BsModalRef;
  requestDesign: CreateDesignModal = new CreateDesignModal();
  screenStatus = 'Create';
  displayStatus: any = 'CREATION';
  masterData: any;
  approval = false;
  requestPhotoData: any = {
    HistoryLog: []
  };
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };

  DesignID: any;
  //circularData: any;
  status = '';
  user = [];//this.masterData.data.user;
  department = [];//this.masterData.data.department;
  approverDepartment = [];
  priorityList = ['High', 'Medium', 'Low', 'Very low'];
  attachmentFiles = [];
  createdTime: string;
  createdDate: string;
  submitted = false;
  RequestComments: any;
  ApproverID = 0;

  dropdownSettings: any;

  colorTheme = 'theme-green';
  mediarequestDesign = {
    DesignID: 0,
    Date: new Date(),
    Project: '',
    Title: '',
    DiwansRole: '',
    OtherParties: '',
    TargetGroups: '',
    SourceOU: '',
    SourceName: '',
    DateofDeliverable: NaN,
    TypeofDesignRequired: null,
    Languages: '',
    ApproverID: null,
    ApproverDepartmentID: 0,
    Comments: '',
    Attachments: [],
    AttachmentName: '',
    AssigneeID: [],
    DeleteFlag: '',
    CreatedBy: 0,
    UpdatedBy: 0,
    Action: '',
    HistoryLog: [],
    GeneralObjective: '',
    MainObjective: '',
    StrategicObjective: '',
    CreatedDateTime: new Date(),
    UpdatedDateTime: new Date(),
    Status: 0
  }
  img_file: any;
  message: any;
  designlist = [];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  attachments: any = [];
  userDestination: any;
  userReceiver: any;
  commonMes: any;
  DestinationDepartmentId: any = [];
  uploadProcess: boolean = false;
  uploadPercentage: number;
  pdfSrc: string;
  showPdf: boolean = false;
  sendDraftBtnLoad = false;
  sendBtnLoad = false;
  printBtnLoad = false;
  cloneBtnLoad = false;
  AssigneeID = 0;
  downloadBtnLoad = false;
  approveBtnLoad = false;
  deleteBtnLoad = false;
  returnInfoBtnLoad = false;
  rejectBtnLoad = false;
  assignBtnLd = false;
  assigntomeBtnLd = false;
  btnLoad = false;
  lang: string;
  isFirstApprover:boolean = false;
  userProfileImg:string = '';

  attachmentDownloadUrl = environment.AttachmentDownloadUrl;
  commentSubscriber: any;


  constructor(private changeDetector: ChangeDetectorRef, public common: CommonService, public router: Router, public route: ActivatedRoute, public datepipe: DatePipe,
    private modalService: BsModalService, public requestDesignService: MediaRequestDesignService, public utils: UtilsService, private commentService:CommentSectionService,private endpoint:EndPointService) {
    // this.bsConfigs = {
    //   dateInputFormat: 'DD/MM/YYYY'
    // }
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    route.url.subscribe(() => {
      this.screenStatus = route.snapshot.data.title;
    });
    route.params.subscribe(param => {
      var id = +param.id;
      this.DesignID = +param.id;
      if (id > 0) {
        this.loadData(id, this.currentUser.id);
      }
      //this.loadData(id, this.currentUser.id);
    });
    if (this.currentUser.AttachmentGuid && this.currentUser.AttachmentName) {
      this.userProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.currentUser.AttachmentName + '&guid=' + this.currentUser.AttachmentGuid;
    }else{
      this.userProfileImg = 'assets/home/user_male.png';
    }
    this.requestDesignService.getDesign('Design', 0, 0).subscribe((data: any) => {
      this.department = data.OrganizationList;
      this.approverDepartment = data.M_ApproverDepartmentList;
    });
    this.requestDesignService.getDesignType('DesignType', 0, 1).subscribe((data: any) => {
      this.designlist = data;
    });
    if (this.screenStatus == 'Create') {
      this.displayStatus = 'CREATION';
    }
    if (this.screenStatus == 'View') {
      this.displayStatus = 'VIEW';
    }
    if (this.screenStatus == 'Edit') {
      this.displayStatus = 'EDIT';
    }
    this.mediarequestDesign.ApproverDepartmentID = this.currentUser.DepartmentID;
    this.getDestUserList(+this.mediarequestDesign.ApproverDepartmentID);
  }
  statusOptions = ['High', 'Medium', 'Low', 'Very low'];
  ngOnInit() {
    this.lang = this.common.currentLang;
    if (this.lang == 'en') {
      this.common.breadscrumChange('Media', 'Request for Design', '');
      if (this.screenStatus == 'View') {
        this.common.breadscrumChange('Media', 'Request for Design', '');
      }
    } else {
      this.common.breadscrumChange(this.arabic('media'), this.arabic('requestfordesign'), '');
      if (this.screenStatus == 'View') {
        this.common.breadscrumChange(this.arabic('media'), this.arabic('requestfordesign'), '');
      }
    }
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.bottonControll();
    this.commentSubscriber = this.commentService.newCommentCreation$.subscribe((newComment) => {
      if(newComment){
        this.loadData(this.DesignID,this.currentUser.id);
      }
    });
  }

  async loadData(id, userid) {
    await this.requestDesignService.getDesign('Design', id, userid).subscribe((data: any) => {
      this.mediarequestDesign = data;
      if (this.screenStatus == 'View' || this.screenStatus == 'Edit') {
        let that = this;
        this.status = this.requestDesign['M_LookupsList'];
        var date = this.mediarequestDesign.CreatedDateTime;
        this.mediarequestDesign.CreatedDateTime = new Date(date);
        this.mediarequestDesign.CreatedBy = this.requestDesign.CreatedBy;
        if (this.mediarequestDesign.AssigneeID.length > 0) {
          this.AssigneeID = this.mediarequestDesign.AssigneeID[0].AssigneeId;
          this.approval = true;
        }
        this.RequestComments = this.setRequestComments(data.DesignCommunicationHistory);
        this.setData(this.mediarequestDesign);
        this.bottonControll();
      } else {
        //this.initPage();
        this.bottonControll();
      }
    });
  }

  ngOnDestroy() {
    this.commentSubscriber.unsubscribe();
  }

  async  setData(data) {
    this.getDestUserList(+data.ApproverDepartmentID);
    //this.getRecvPrepareUserList(data.DestinationDepartmentID);
    this.mediarequestDesign.DesignID = data.DesignId;
    this.mediarequestDesign.Title = data.Title;
    this.getSouceName(data.SourceName,data.SourceOU,data.TypeofDesignRequired);

  //  this.mediarequestDesign.SourceOU = data.SourceOU;
    //this.mediarequestDesign.SourceName = data.SourceName;
    this.mediarequestDesign.Languages = '' + data.Languages;


    // data.DestinationUsernameID.forEach((user,index)=>{
    //   DestinationUsername.push(user.MemoDestinationUsersID);
    // });
    //this.incomingcircular.DestinationUsername = DestinationUsername;

    this.mediarequestDesign.ApproverID = data.ApproverID;
    if (data.CurrentApprover.length > 0) {
      this.ApproverID = data.CurrentApprover[0].ApproverId;
    }
    //Check this set
    this.mediarequestDesign.ApproverDepartmentID = data.ApproverDepartmentID; //check this set
    this.mediarequestDesign.Project = data.InitiativeProjectActivity;
    this.mediarequestDesign.DiwansRole = data.DiwansRole;
    this.mediarequestDesign.Date = new Date(data.Date);
    data.DeliverableDate = new Date(data.DeliverableDate);
    this.mediarequestDesign.DateofDeliverable = data.DeliverableDate;
    this.mediarequestDesign.TargetGroups = data.TargetGroup;

    this.mediarequestDesign.Languages = data.Languages;
    this.mediarequestDesign.OtherParties = data.OtherParties;
    this.mediarequestDesign.MainObjective = data.MainObjective;
    this.mediarequestDesign.GeneralObjective = data.GeneralObjective;
    this.mediarequestDesign.StrategicObjective = data.StrategicObjective;
    this.mediarequestDesign.CreatedBy = data.CreatedBy;
    this.mediarequestDesign.Comments = "";
    this.attachments = data.Attachments;
    this.mediarequestDesign.HistoryLog = data.DesignCommunicationHistory;

    this.mediarequestDesign.Attachments = data.Attachments;
    this.mediarequestDesign.Status = data.Status;
  }

  async getSouceName(UserID,DepID,DesignType) {
     let params = [{
       "OrganizationID": DepID,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;
       this.mediarequestDesign.SourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
       this.mediarequestDesign.SourceOU  = this.department.filter(x=> x.OrganizationID == DepID)[0].OrganizationUnits;
       var TempDesignTypeName = this.designlist.find(x=> x.DesignTypeID == DesignType);
       this.mediarequestDesign.TypeofDesignRequired = TempDesignTypeName.DesignTypeName;
     });

   }

  Attachments(event) {
    this.img_file = event.target.files;
    for (var i = 0; i < this.img_file.length; i++) {
      this.attachmentFiles.push(this.img_file[i]);
      this.attachments.push({ 'AttachmentGuid': 0, 'AttachmentsName': this.img_file[i].name, 'DesignID': '' });
    }
    this.mediarequestDesign.Attachments = this.attachments;
  }
  uploadFiles(event) {
    var files = event.target.files;
    let that = this;
    this.uploadProcess = true;
    this.common.postAttachment(files).subscribe((event: any) => {
      if (event.type === HttpEventType.UploadProgress) {
        this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
      } else if (event.type === HttpEventType.Response) {
        this.uploadProcess = false;
        this.uploadPercentage = 0;
        for (var i = 0; i < event.body.FileName.length; i++) {
          this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'DesignID': '' });
        }
        this.mediarequestDesign.Attachments = this.attachments;
      }
    });
    this.myInputVariable.nativeElement.value = "";
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


  onChangeDepartment() {
    this.getDestUserList(+this.mediarequestDesign.ApproverDepartmentID);
    this.mediarequestDesign.ApproverID = 0;
  }


  async getDestUserList(id) {
    console.log("###");
    if (id) {
      let params = [{
        "OrganizationID": id,
        "OrganizationUnits": "string"
      }];
      let user_dept_id = this.currentUser.id;
      if (this.screenStatus == 'View') {
        user_dept_id = 0;
      }
      this.common.getmemoUserList(params, user_dept_id).subscribe((data: any) => {
        this.userDestination = data;
      });
    } else {
      this.userDestination = [];
    }
  }

  validateForm() {
    var flag = true;

    //var Keywords = (this.incomingcircular.Keywords) ? (this.incomingcircular.Keywords.length > 0) : false;
    //var username = (this.incomingcircular.DestinationUsername) ? (this.incomingcircular.DestinationUsername.length > 0) : false;

    if (this.mediarequestDesign.Title && this.mediarequestDesign.Project
      && this.mediarequestDesign.TargetGroups && this.mediarequestDesign.OtherParties && this.mediarequestDesign.DiwansRole && this.mediarequestDesign.DateofDeliverable && this.mediarequestDesign.TypeofDesignRequired && this.mediarequestDesign.Languages && this.mediarequestDesign.ApproverDepartmentID && this.mediarequestDesign.ApproverID && this.mediarequestDesign.GeneralObjective && this.mediarequestDesign.MainObjective && this.mediarequestDesign.StrategicObjective && !this.sendBtnLoad) {
      flag = false;
    }
    if (this.mediarequestDesign.Title !== null && this.mediarequestDesign.Title.trim() === '') {
      flag = true;
    }
    if (this.mediarequestDesign.Project !== null && this.mediarequestDesign.Project.trim() === '') {
      flag = true;
    }
    if (this.mediarequestDesign.TargetGroups !== null && this.mediarequestDesign.TargetGroups.trim() === '') {
      flag = true;
    }
    if (this.mediarequestDesign.OtherParties !== null && this.mediarequestDesign.OtherParties.trim() === '') {
      flag = true;
    }
    if (this.mediarequestDesign.DiwansRole !== null && this.mediarequestDesign.DiwansRole.trim() === '') {
      flag = true;
    }
    if (this.mediarequestDesign.GeneralObjective !== null && this.mediarequestDesign.GeneralObjective.trim() === '') {
      flag = true;
    }
    if (this.mediarequestDesign.MainObjective !== null && this.mediarequestDesign.MainObjective.trim() === '') {
      flag = true;
    }
    if (this.mediarequestDesign.StrategicObjective !== null && this.mediarequestDesign.StrategicObjective.trim() === '') {
      flag = true;
    }
    if (!this.utils.isValidDate(this.mediarequestDesign.DateofDeliverable)) {
      flag = true;
    }
    return flag;
  }

  prepareData() {
    this.requestDesign.CreatedDateTime = this.mediarequestDesign.CreatedDateTime;
    this.requestDesign.Date = this.mediarequestDesign.Date;
    this.requestDesign.DateofDeliverable = this.mediarequestDesign.DateofDeliverable;
    this.requestDesign.Title = this.mediarequestDesign.Title;
    this.requestDesign.Project = this.mediarequestDesign.Project;
    this.requestDesign.Languages = this.mediarequestDesign.Languages;
    this.requestDesign.TypeofDesignRequired = this.designlist.find(x=> x.DesignTypeName== this.mediarequestDesign.TypeofDesignRequired).DesignTypeID;
    this.requestDesign.DiwansRole = this.mediarequestDesign.DiwansRole;
    this.requestDesign.OtherParties = this.mediarequestDesign.OtherParties;
    this.requestDesign.TargetGroups = this.mediarequestDesign.TargetGroups;
    this.requestDesign.MainObjective = this.mediarequestDesign.MainObjective;
    this.requestDesign.StrategicObjective = this.mediarequestDesign.StrategicObjective;
    this.requestDesign.GeneralObjective = this.mediarequestDesign.GeneralObjective;
    this.requestDesign.SourceOU = this.currentUser.DepartmentID;
    this.requestDesign.SourceName = this.currentUser.UserID;
    this.requestDesign.ApproverID = this.mediarequestDesign.ApproverID;
    this.requestDesign.ApproverDepartmentID = this.mediarequestDesign.ApproverDepartmentID;
    this.requestDesign.CreatedBy = this.mediarequestDesign.CreatedBy;
    this.requestDesign.Attachments = this.mediarequestDesign.Attachments;
    this.requestDesign.CreatedBy = this.currentUser.id;
    //this.createMemo.Action = this.memoModel.Status+'';
    this.requestDesign.Comments = this.mediarequestDesign.Comments;

    return this.requestDesign;
  }

  saveRequestDesign(data = '') {
    var requestData = this.prepareData();
    requestData.Action = 'Submit';
    let url = this.requestDesignService.saveRequestDesign('Design', requestData);
    if (this.mediarequestDesign.Status == 65) {
      requestData.Action = 'Resubmit';
      requestData['DesignID'] = this.mediarequestDesign.DesignID;
      requestData['UpdatedBy'] = this.currentUser.id;
      requestData['UpdatedDateTime'] = new Date();
      url = this.requestDesignService.updateRequestDesign('Design', requestData);
    }
    url.subscribe(data => {

      this.sendDraftBtnLoad = false;
      this.sendBtnLoad = false;
      if (this.lang == 'en') {
        this.message = "Media Request for Design Submitted Successfully";
      } else {
        this.message = this.arabic('designsubmitmsg');
      }
      if (this.mediarequestDesign.Status == 65) {
        if (this.lang == 'en') {
          this.message = "Media Request for Design Resubmitted Successfully";
        } else {
          this.message = this.arabic('designsubmitmsg');
        }
      }
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'Media';
      //location.reload();
    });
  }

  statusChange(status: any, dialog) {
    this.submitted = true;
    this.btnLoad = true;
    var data = this.formatPatch(status, 'Action')
    this.commonMes = status;
    this.requestDesignService.statusChange('Design', this.mediarequestDesign.DesignID, data).subscribe(data => {
      //this.message = 'Memo '+status+'d';
      this.approveBtnLoad = false;
      this.returnInfoBtnLoad = false;
      this.rejectBtnLoad = false;
      this.assignBtnLd = false;
      this.assigntomeBtnLd = false;
      this.cloneBtnLoad = false;
      this.btnLoad = false;
      if (status == 'ReturnForInfo') {
        if (this.lang == 'en') {
          this.message = "Media Request Design Return for Info Successfully";
        } else {
          this.message = this.arabic('designreturnedmsg');
        }
      } else if (status == 'Approve') {
        if (this.lang == 'en') {
          this.message = "Media Request Design Approved Successfully ";
        } else {
          this.message = this.arabic('designapprovedmsg');
        }
      } else if (status == 'Reject') {
        if (this.lang == 'en') {
          this.message = "Media Request Design Rejected Successfully";
        } else {
           this.message = this.arabic('designrejectmsg');
        }
      } else if (status == 'Close') {
        if (this.lang == 'en') {
          this.message = "Media Request Design Closed Successfully";
        } else {
          this.message = this.arabic('designclosedmsg');
        }
      } else if (status == 'AssignToMe') {
        if (this.lang == 'en') {
          this.message = 'Media Request Design Assigned Successfully';
        } else {
          this.message = this.arabic('designassignedmsg');
        }
      } else {
        this.message = 'Media Request Design ' + status + 'd Successfully';
      }
      console.log('mes', this.message)
      //that.modalService.show(that.template);
      //this.modalService.show(this.template);
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'Media';
      this.loadData(data['DesignID'], this.currentUser.id);
      //location.reload();

    });

  }

  createBtnShow = false;
  editBtnShow = false;
  viewBtnShow = false;
  approverBtn = false;
  receiverBtn = false;
  deleteBtn = false;
  creatorBtn = false;
  draftBtn = false;
  cloneBtn = false;
  savedraftBtn = false;
  assignBtn = false;
  assigntomeBtn = false;
  closeBtn = false;
  id = '';
  printbtn = false;
  bottonControll() {
    this.assigntomeBtn = false;
    this.isFirstApprover = false;
    if (this.screenStatus == 'Create' || this.screenStatus == 'Edit') {
      this.createBtnShow = true;
      this.savedraftBtn = true;
      this.printbtn = true;
    } else if (this.screenStatus == 'Edit') {
      this.editBtnShow = true;
    } else if (this.screenStatus == 'View' && this.mediarequestDesign.CreatedBy == this.currentUser.id) {
      this.viewBtnShow = true;
    }
    if (this.mediarequestDesign.CreatedBy == this.currentUser.id && this.mediarequestDesign.Status == 65) {
      this.createBtnShow = true;
    }
    // if (this.mediarequestDesign.CreatedBy == this.currentUser.id && (this.mediarequestDesign.Status == 12 || this.mediarequestDesign.Status == 16) && this.screenStatus == 'Edit') {
    //   this.draftBtn = true;
    // }
    if (this.screenStatus == 'View' && (this.ApproverID == this.currentUser.id) && this.mediarequestDesign.Status == 68) {
      this.approverBtn = true;
      this.isFirstApprover = true;
    }
    if (this.screenStatus == 'View' && this.currentUser.IsOrgHead && (this.currentUser.OrgUnitID == 17) && this.mediarequestDesign.Status == 64 &&this.AssigneeID == this.currentUser.id) {
      this.closeBtn = true;
    }
    if (this.AssigneeID == this.currentUser.id && (this.currentUser.OrgUnitID == 17) && this.mediarequestDesign.Status != 66 && this.mediarequestDesign.Status != 68 && this.mediarequestDesign.Status != 0) {
      this.closeBtn = true;
    }
    if (this.screenStatus == 'View' && this.currentUser.IsOrgHead && !this.approval && (this.currentUser.OrgUnitID == 17) && this.mediarequestDesign.Status == 64) {
      this.assignBtn = true;
    }
    if (this.screenStatus == 'View' && this.currentUser.IsOrgHead && this.approval && (this.currentUser.OrgUnitID == 17) && this.mediarequestDesign.Status == 64) {
      if(this.AssigneeID != this.currentUser.id)
      this.assignBtn = true;
    }
    if (this.screenStatus == 'View' && !this.currentUser.IsOrgHead && !this.approval && (this.currentUser.OrgUnitID == 17)&& this.mediarequestDesign.Status == 64) {
      this.assigntomeBtn = true;
    }
    if (this.screenStatus == 'View' && !this.currentUser.IsOrgHead && this.approval && (this.currentUser.OrgUnitID == 17)&& this.mediarequestDesign.Status == 64) {
      if(this.AssigneeID != this.currentUser.id)
      this.assigntomeBtn = true;
    }
    // if (this.AssigneeID) {
    //   this.assigntomeBtn = false;
    //   this.assignBtn = false;
    // }
    if (this.mediarequestDesign.Status == 66) {
      this.assigntomeBtn = false;
      this.assignBtn = false;
      this.closeBtn = false;
    }
    // if (this.mediarequestDesign.CreatedBy == this.currentUser.id && this.mediarequestDesign.Status == 12 && this.screenStatus == 'Edit') {
    //   this.deleteBtn = true;
    //   this.savedraftBtn = true;
    // }

  }
  formatPatch(val, path) {
    var data = [{
      "value": val,
      "path": path,
      "op": "Replace"
    }, {
      "value": this.currentUser.id,
      "path": "UpdatedBy",
      "op": "Replace"
    }, {
      "value": new Date(),
      "path": "UpdatedDateTime",
      "op": "Replace"
    }, {
      "value": this.mediarequestDesign.Comments,
      "path": "Comments",
      "op": "Replace"
    }, {
      "value": this.mediarequestDesign.ApproverID,
      "path": "ApproverId",
      "op": "Replace"
    }];
    return data;
  }

  hisLog(status) {
    let sts = status.toLowerCase();
    if(this.common.currentLang != 'ar'){
      if (sts == 'submit') {
        return status + 'ted By';
      } else if (sts == 'reject' || sts == 'redirect') {
        return status + 'ed By';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        return 'Assigned By'
      } else {
        return status + 'd By';
      }
    }else if(this.common.currentLang == 'ar'){
      let arabicStatusStr = '';
      if (sts == 'reject' || sts == 'redirect') {
        arabicStatusStr = sts+'edby';
      } else if (sts == 'assignto' || sts == 'assigntome') {
        arabicStatusStr = 'assignedby';
      } else if(sts == 'submit' || sts == 'resubmit'){
        arabicStatusStr = sts+'tedby';
      } else {
        arabicStatusStr = sts+'dby';
      }
      return this.common.arabic.words[arabicStatusStr];
    }
  }
  assignpopup(status: any) {
    this.bsModalRef = this.modalService.show(AssignModalComponent, this.config);
    this.bsModalRef.content.status = status;
    this.bsModalRef.content.fromScreen = 'Media Request Design';
    this.bsModalRef.content.page = 'Media';
    this.bsModalRef.content.ActionTaken = this.mediarequestDesign.Comments;
    this.bsModalRef.content.HRComplaintSuggestionId = this.mediarequestDesign.DesignID;
    this.btnLoad = false;
  }
  popup(status: any) {
    this.bsModalRef = this.modalService.show(ModalComponent, this.config);
    this.bsModalRef.content.status = 'Design Escalate';
    this.bsModalRef.content.fromScreen = 'DesignMedia';
    this.bsModalRef.content.button = 'Escalate';
    this.bsModalRef.content.Comments = this.mediarequestDesign.Comments;
    this.bsModalRef.content.memoid = this.mediarequestDesign.DesignID;
    this.bsModalRef.content.onClose.subscribe(result => {
      this.btnLoad = result;
    });
  }
  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.myInputVariable.nativeElement.value = "";
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  validDeliverableDate(eventData){
    if(eventData){
      this.mediarequestDesign.DateofDeliverable = eventData;
      this.validateForm();
    }
  }


}
