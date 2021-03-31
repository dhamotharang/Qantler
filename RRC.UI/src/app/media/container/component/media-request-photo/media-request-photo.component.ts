import { Component, OnInit, OnDestroy, TemplateRef, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { ModalComponent } from '../../../../modal/modalcomponent/modal.component';
import { CommonService } from '../../../../common.service';
import { DatePipe } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { MediaRequestPhotoService } from './media-request-photo.service';
import { AssignModalComponent } from '../../../../modal/assignmodal/assignmodal.component';
import { HttpEventType } from '@angular/common/http';
import { CreatePhotoModal } from './media-request-photo.modal';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { environment } from 'src/environments/environment';
import { CommentSectionService } from 'src/app/shared/service/comment-section.service';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-media-request-photo',
  templateUrl: './media-request-photo.component.html',
  styleUrls: ['./media-request-photo.component.scss']
})
export class MediaRequestPhotoComponent implements OnInit,OnDestroy {

  bsConfig: Partial<BsDatepickerConfig>;
  bsConfigs: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  @ViewChild('template') template: TemplateRef<any>;
  @ViewChild('variable') myInputVariable: ElementRef;
  @ViewChild('printContent') printContent: ElementRef<any>;
  bsModalRef: BsModalRef;
  requestPhoto: CreatePhotoModal = new CreatePhotoModal();commentSubscriber: any;
;
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
  //circularData: any;
  status = '';
  user = [];//this.masterData.data.user;
  department = [];//this.masterData.data.department;
  Departments:any = [];
  priorityList = ['High', 'Medium', 'Low', 'Very low'];
  attachmentFiles = [];
  createdTime: string;
  createdDate: string;
  attachmentDownloadUrl = environment.AttachmentDownloadUrl;
  RequestComments: any;
  submitted = false;

  dropdownSettings: any;

  colorTheme = 'theme-green';
  PhotoID: any;
  mediarequestPhoto: any = {
    PhotoID: 0,
    Date: new Date(),
    Location: '',
    Title: '',
    SourceOU: '',
    SourceName: '',
    EventDate: NaN,
    EventName: '',
    PhotoDescription: '',
    ApproverID: '',
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
    CreatedDateTime: new Date(),
    UpdatedDateTime: new Date(),
    Status: 0,
    CurrentApprover: []
  }
  img_file: any;
  message: any;
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
  ApproverID: any;
  downloadBtnLoad = false;
  approveBtnLoad = false;
  deleteBtnLoad = false;
  returnInfoBtnLoad = false;
  rejectBtnLoad = false;
  assignBtnLd = false;
  assigntomeBtnLd = false;
  btnLoad = false;
  lang: string;
  empProfileImg: string = 'assets/home/user_male.png';
  creationApproverID: any;
  canComment: boolean;

  constructor(private changeDetector: ChangeDetectorRef, public common: CommonService, public utils: UtilsService, public router: Router, public route: ActivatedRoute, public datepipe: DatePipe,
    private modalService: BsModalService, public requestPhotoService: MediaRequestPhotoService, private commentSectionService:CommentSectionService,private endpoint: EndPointService) {
    // this.bsConfigs = {
    //   dateInputFormat: 'DD/MM/YYYY'
    // }
    this.lang = this.common.currentLang;
    route.url.subscribe(() => {
      console.log(route.snapshot.data);
      this.screenStatus = route.snapshot.data.title;
    });
    route.params.subscribe(param => {
      var id = +param.id;
      this.PhotoID = +param.id;
      if (id > 0) {
        this.loadData(id, this.currentUser.id);
      }
      if (this.currentUser.AttachmentGuid && this.currentUser.AttachmentName) {
        this.empProfileImg = this.endpoint.fileDownloadUrl + '?filename=' + this.currentUser.AttachmentName + '&guid=' + this.currentUser.AttachmentGuid;
      }
      //this.loadData(id, this.currentUser.id);
    });

    this.requestPhotoService.getPhoto('Photo', 0, 0).subscribe((data: any) => {
      this.department = data.M_ApproverDepartmentList;
      this.Departments = data.OrganizationList;
    });
    if (this.screenStatus == 'Create') {
      this.displayStatus = 'CREATION';
      this.canComment = true;
    }
    if (this.screenStatus == 'View') {
      this.displayStatus = 'VIEW';
    }
    if (this.screenStatus == 'Edit') {
      this.displayStatus = 'EDIT';
    }
    this.mediarequestPhoto.ApproverDepartmentID = this.currentUser.DepartmentID;
    this.onChangeDepartment();
  }
  statusOptions = ['High', 'Medium', 'Low', 'Very low'];
  ngOnInit() {
    if (this.lang == 'en') {
      this.common.breadscrumChange('Media', 'Request for Photo', '');
      if (this.screenStatus == 'View') {
        this.common.breadscrumChange('Media', 'Request for Photo View', '');
      }
    } else {
      this.common.breadscrumChange(this.arabic('media'), this.arabic('requestforphoto'), '');
      if (this.screenStatus == 'View') {
        this.common.breadscrumChange(this.arabic('media'), this.arabic('requestforphotoview'), '');
      }
    }
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.bottonControll();
    this.commentSubscriber = this.commentSectionService.newCommentCreation$.subscribe((newComment) => {
      if(newComment){
        this.loadData(this.PhotoID, this.currentUser.id);
      }
    });
  }

  async loadData(id, userid) {
    await this.requestPhotoService.getPhoto('Photo', id, userid).subscribe((data: any) => {
      this.mediarequestPhoto = data;
      this.Departments = data.OrganizationList;
      if (this.screenStatus == 'View' || this.screenStatus == 'Edit') {
        let that = this;
        this.status = this.requestPhoto['M_LookupsList'];
        var date = this.mediarequestPhoto.CreatedDateTime;
        this.mediarequestPhoto.CreatedDateTime = new Date(date);
        //  this.mediarequestPhoto.CreatedBy = this.requestPhoto.CreatedBy;
        if (this.mediarequestPhoto.AssigneeID.length) {
          this.AssigneeID = this.mediarequestPhoto.AssigneeID[0].AssigneeId;
          this.approval = true;
        }

        this.RequestComments = this.setRequestComments(data.PhotoCommunicationHistory);
        this.setData(this.mediarequestPhoto);
        this.bottonControll();
        this.checkCanComment();
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
    this.mediarequestPhoto.PhotoID = data.PhotoID;
    this.mediarequestPhoto.Title = data.Title;
    this.getSouceName(data.SourceName,data.SourceOU);
  //  this.mediarequestPhoto.SourceOU =
  this.mediarequestPhoto.SourceOU  = this.Departments.filter(x=> x.OrganizationID == data.SourceOU)[0].OrganizationUnits
    //  var y = t.OrganizationUnits;
    //this.mediarequestPhoto.SourceName = data.SourceName;


    // data.DestinationUsernameID.forEach((user,index)=>{
    //   DestinationUsername.push(user.MemoDestinationUsersID);
    // });
    //this.incomingcircular.DestinationUsername = DestinationUsername;

    this.mediarequestPhoto.ApproverID = data.ApproverID;
    // if (data.CurrentApprover.length > 0) {
      this.ApproverID = data.ApproverID;

    this.mediarequestPhoto.CurrentApprover = data.CurrentApprover;
    //Check this set
    this.mediarequestPhoto.ApproverDepartmentID = data.ApproverDepartmentID; //check this set
    this.mediarequestPhoto.PhotoDescription = data.PhotoDescription;
    this.mediarequestPhoto.EventName = data.EventName;
    this.mediarequestPhoto.Date = new Date(data.Date);
    data.EventDate = new Date(data.EventDate);
    this.mediarequestPhoto.EventDate = data.EventDate;
    this.mediarequestPhoto.Comments = "";
    this.mediarequestPhoto.Location = data.Location;
    this.mediarequestPhoto.CreatedBy = data.CreatedBy;
    this.attachments = data.Attachments;
    this.mediarequestPhoto.HistoryLog = data.PhotoCommunicationHistory;

    this.mediarequestPhoto.Attachments = data.Attachments;
    this.mediarequestPhoto.Status = data.Status;
  }

  async getSouceName(UserID,DepID) {
    let params = [{
      "OrganizationID": DepID,
      "OrganizationUnits": "string"
    }];
    this.common.getUserList(params,0).subscribe((data: any) => {
      let Users = data;
      this.mediarequestPhoto.SourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
    });

  }

  Attachments(event) {
    this.img_file = event.target.files;
    for (var i = 0; i < this.img_file.length; i++) {
      this.attachmentFiles.push(this.img_file[i]);
      this.attachments.push({ 'AttachmentGuid': 0, 'AttachmentsName': this.img_file[i].name, 'PhotoID': '' });
    }
    this.mediarequestPhoto.Attachments = this.attachments;
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
          this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'PhotoID': '' });
        }
        this.mediarequestPhoto.Attachments = this.attachments;
      }
    });
    this.myInputVariable.nativeElement.value = "";
  }


  onChangeDepartment() {
    this.getDestUserList(+this.mediarequestPhoto.ApproverDepartmentID);
    this.mediarequestPhoto.ApproverID = null;
  }


  async getDestUserList(id) {
    // if(this.mediarequestPhoto.ApproverID)
    //   this.mediarequestPhoto.ApproverID = null;
    if (id) {
      let params = [{
        "OrganizationID": id,
        "OrganizationUnits": "string"
      }];
      let user_dep_id = this.currentUser.id;
      if (this.screenStatus == 'View') {
        user_dep_id = 0;
      }
      this.common.getmemoUserList(params, user_dep_id).subscribe((data: any) => {
        this.userDestination = data;
        // this.mediarequestPhoto.ApproverID = this.ApproverID;
      });
    } else {
      this.userDestination = [];
    }
  }

  validateForm() {
    var flag = true;
    //var Keywords = (this.incomingcircular.Keywords) ? (this.incomingcircular.Keywords.length > 0) : false;
    //var username = (this.incomingcircular.DestinationUsername) ? (this.incomingcircular.DestinationUsername.length > 0) : false;

    if (this.mediarequestPhoto.EventDate && this.utils.isValidDate(this.mediarequestPhoto.EventDate) && this.mediarequestPhoto.Location.trim()
      && this.mediarequestPhoto.EventName.trim() && this.mediarequestPhoto.PhotoDescription.trim() &&!this.sendBtnLoad) {
      flag = false;
    }
    return flag;
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
        // if(commObj.Photo == null){
        // commObj.Photo = 'assets/home/user_male.png';
        // }
        recursiveCommentsArr.push(commObj);
      }
    });
    return recursiveCommentsArr;
  }

  prepareData() {
    this.requestPhoto.CreatedDateTime = this.mediarequestPhoto.CreatedDateTime;
    this.requestPhoto.Date = this.mediarequestPhoto.Date;
    this.requestPhoto.EventDate = this.mediarequestPhoto.EventDate;
    this.requestPhoto.EventName = this.mediarequestPhoto.EventName;
    this.requestPhoto.PhotoDescription = this.mediarequestPhoto.PhotoDescription;
    this.requestPhoto.Location = this.mediarequestPhoto.Location;
    this.requestPhoto.SourceOU = this.currentUser.DepartmentID;
    this.requestPhoto.SourceName = this.currentUser.UserID;
    this.requestPhoto.ApproverID = this.mediarequestPhoto.ApproverID;
    this.requestPhoto.ApproverDepartmentID = this.mediarequestPhoto.ApproverDepartmentID;
    this.requestPhoto.CreatedBy = this.mediarequestPhoto.CreatedBy;
    this.requestPhoto.Attachments = this.mediarequestPhoto.Attachments;
    this.requestPhoto.CreatedBy = this.currentUser.id;
    //this.createMemo.Action = this.memoModel.Status+'';
    this.requestPhoto.Comments = this.mediarequestPhoto.Comments;

    return this.requestPhoto;
  }

  saveRequestPhoto(data = '') {
    var requestData = this.prepareData();
    requestData.Action = 'Submit';


    let url = this.requestPhotoService.saveRequestPhoto('Photo', requestData);
    if (this.mediarequestPhoto.Status == 89) {
      requestData.Action = 'Resubmit';
      requestData['PhotoID'] = this.mediarequestPhoto.PhotoID;
      requestData['UpdatedBy'] = this.currentUser.id;
      requestData['UpdatedDateTime'] = new Date();
      url = this.requestPhotoService.updateRequestPhoto('Photo', requestData);
    }
    url.subscribe(data => {

      this.sendDraftBtnLoad = false;
      this.sendBtnLoad = false;
      console.log(data);
      if (this.lang == 'en') {
        this.message = "Photo Request Submitted Successfully";
      } else {
        this.message = this.arabic('photosubmitsuccessmsg');
      }
      if (this.mediarequestPhoto.Status == 89) {
        if (this.lang == 'en') {
          this.message = "Photo Request ReSubmitted Successfully";
        } else {
          this.message = this.arabic('photoresubmitsuccessmsg');
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
    var data = this.formatPatch(status, 'Action')
    this.commonMes = status;
    this.requestPhotoService.statusChange('Photo', this.mediarequestPhoto.PhotoID, data).subscribe(data => {
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
          this.message = "Photo Request Returned For Info Successfully";
        } else {
          this.message = this.arabic('photoreturnforinfosuccessmsg');
        }
      } else if (status == 'Approve') {
        if (this.lang == 'en') {
          this.message = "Photo Request Approved Successfully ";
        } else {
          this.message = this.arabic('photoapprovesuccessmsg');
        }
      } else if (status == 'Reject') {
        if (this.lang == 'en') {
          this.message = "Photo Request Rejected Successfully";
        } else {
          this.message = this.arabic('photorejectsuccessmsg');
        }
      } else if (status == 'Close') {
        if (this.lang == 'en') {
          this.message = "Photo Request Closed Successfully";
        } else {
          this.message = this.arabic('photoclosesuccessmsg');
        }
      } else if (status == 'AssignToMe') {
        if (this.lang == 'en') {
          this.message = 'Photo Request Assigned Successfully';
        } else {
          this.message = this.arabic('photoassignsuccessmsg');
        }
      } else {
        this.message = 'Photo Request ' + status + 'd Successfully';
      }
      //that.modalService.show(that.template);
      //this.modalService.show(this.template);
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'Media';
      this.loadData(data['PhotoID'], this.currentUser.id);
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
    if (this.screenStatus == 'Create' || this.screenStatus == 'Edit') {
      this.createBtnShow = true;
      this.savedraftBtn = true;
      this.printbtn = true;
    } else if (this.screenStatus == 'Edit') {
      this.editBtnShow = true;
    } else if (this.screenStatus == 'View' && this.mediarequestPhoto.CreatedBy == this.currentUser.id) {
      this.viewBtnShow = true;
    }
    if (this.mediarequestPhoto.CreatedBy == this.currentUser.id && this.mediarequestPhoto.Status == 89) {
      this.createBtnShow = true;
    }
    // if (this.mediarequestPhoto.CreatedBy == this.currentUser.id && (this.mediarequestPhoto.Status == 12 || this.mediarequestPhoto.Status == 16) && this.screenStatus == 'Edit') {
    //   this.draftBtn = true;
    // }
    if (this.screenStatus == 'View' && (this.mediarequestPhoto.CurrentApprover.findIndex(ca => ca.ApproverId == this.currentUser.id) > -1) && this.mediarequestPhoto.Status == 92) {
      this.approverBtn = true;
    }
    if (this.screenStatus == 'View' && this.currentUser.IsOrgHead && (this.currentUser.OrgUnitID == 17) && this.mediarequestPhoto.Status == 88&&this.AssigneeID == this.currentUser.id) {
      this.closeBtn = true;
    }
    if (this.AssigneeID == this.currentUser.id && (this.currentUser.OrgUnitID == 17) && this.mediarequestPhoto.Status != 90 && this.mediarequestPhoto.Status != 92 && this.mediarequestPhoto.Status != 0) {
      this.closeBtn = true;
    }
    if (this.screenStatus == 'View' && this.currentUser.IsOrgHead && !this.approval && (this.currentUser.OrgUnitID == 17) && this.mediarequestPhoto.Status == 88) {
      this.assignBtn = true;
    }
    if (this.screenStatus == 'View' && this.currentUser.IsOrgHead && this.approval && (this.currentUser.OrgUnitID == 17) && this.mediarequestPhoto.Status == 88) {
      if(this.AssigneeID != this.currentUser.id)
      this.assignBtn = true;
    }
    if (this.screenStatus == 'View' && !this.currentUser.IsOrgHead && !this.approval && (this.currentUser.OrgUnitID == 17) && this.mediarequestPhoto.Status == 88) {
      this.assigntomeBtn = true;
    }
    if (this.screenStatus == 'View' && !this.currentUser.IsOrgHead && this.approval && (this.currentUser.OrgUnitID == 17) && this.mediarequestPhoto.Status == 88) {
      if(this.AssigneeID != this.currentUser.id)
      this.assigntomeBtn = true;
    }
    // if (this.AssigneeID) {
    //   this.assigntomeBtn = false;
    //   this.assignBtn = false;
    // }
    if (this.mediarequestPhoto.Status == 90) {
      this.assigntomeBtn = false;
      this.assignBtn = false;
      this.closeBtn = false;
    }

    // if (this.mediarequestPhoto.CreatedBy == this.currentUser.id && this.mediarequestPhoto.Status == 12 && this.screenStatus == 'Edit') {
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
      "value": this.mediarequestPhoto.Comments,
      "path": "Comments",
      "op": "Replace"
    }, {
      "value": this.mediarequestPhoto.CurrentApprover[0].ApproverId,
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
    this.bsModalRef.content.fromScreen = 'PhotoMedia';
    this.bsModalRef.content.page = 'Media';
    this.bsModalRef.content.ActionTaken = this.mediarequestPhoto.Comments;
    this.bsModalRef.content.HRComplaintSuggestionId = this.mediarequestPhoto.PhotoID;
    this.btnLoad = false;
  }
  popup(status: any) {
    this.bsModalRef = this.modalService.show(ModalComponent, this.config);
    this.bsModalRef.content.status = 'Photo Escalate';
    this.bsModalRef.content.fromScreen = 'PhotoMedia';
    this.bsModalRef.content.Comments = this.mediarequestPhoto.Comments;
    this.bsModalRef.content.memoid = this.mediarequestPhoto.PhotoID;
    this.bsModalRef.content.onClose.subscribe(result => {
      this.btnLoad = result;
    });
  }
  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.myInputVariable.nativeElement.value = "";
  }


  checkCanComment(){
    if (this.mediarequestPhoto.Status == 89) { //Waiting for Resubmission
      if (this.mediarequestPhoto.CreatedBy == this.currentUser.id) {
        this.canComment = true;
      }
    }

    if (this.mediarequestPhoto.Status == 88) { // Under Proccess
      if ((this.currentUser.IsOrgHead && ((this.currentUser.OrgUnitID == 17))) || (!this.currentUser.IsOrgHead && ((this.currentUser.OrgUnitID == 17)))) {
        this.canComment = true;
      } else {
        this.canComment = false;
      }
    }
    if (this.mediarequestPhoto.Status == 90) { //Closed
      this.canComment = false;
    }

    if (this.mediarequestPhoto.CurrentApprover && this.mediarequestPhoto.CurrentApprover.length > 0 && this.mediarequestPhoto.Status == 92) {  // waiting for approval
      this.mediarequestPhoto.CurrentApprover.forEach((assignee: any) => {
        if (assignee.ApproverId == this.currentUser.id) {
          this.canComment = true;
        }
      });
    }
  }
  arabic(word) {
    return this.common.arabic.words[word];
  }





}
