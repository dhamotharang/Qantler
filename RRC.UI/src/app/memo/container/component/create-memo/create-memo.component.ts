import { Component, OnInit, ViewChild, TemplateRef, ChangeDetectorRef, ElementRef } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { FormGroup, FormControl } from '@angular/forms';
import { CreateMemoModal } from './create-memo.modal';
import { MemoService } from '../../../services/memo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MasterData } from './masterdata';
import { SampleData } from './sampleDB';
// import 'tinymce';
import { async } from 'q';
import { DatePipe } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../../../../modal/modalcomponent/modal.component';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { CommonService } from '../../../../common.service';
import { HttpEventType } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { EndPointService } from 'src/app/api/endpoint.service';

// declare var tinymce: any;

@Component({
  selector: 'app-create-memo',
  templateUrl: './create-memo.component.html',
  styleUrls: ['./create-memo.component.scss']
})
export class CreateMemoComponent implements OnInit {
  environment = environment;
  @ViewChild('variable') myInputVariable: ElementRef;
  @ViewChild('template') template: TemplateRef<any>;
  @ViewChild('printContent') printContent: ElementRef<any>;
  @ViewChild('tinyDetail') tinyDetail: ElementRef;
  bsModalRef: BsModalRef;
  createMemo: CreateMemoModal = new CreateMemoModal();
  screenStatus = 'Create';
  masterData: MasterData = new MasterData();
  memoData: any = {
    HistoryLog: []
  };
  config = {
    backdrop: true,
    ignoreBackdropClick: true
  };

  tinyConfig = {
    plugins: 'powerpaste casechange importcss tinydrive searchreplace directionality visualblocks visualchars fullscreen table charmap hr pagebreak nonbreaking toc insertdatetime advlist lists checklist wordcount tinymcespellchecker a11ychecker imagetools textpattern noneditable help formatpainter permanentpen pageembed charmap tinycomments mentions quickbars linkchecker emoticons',
    language: this.common.language != 'English' ? "ar" : "en",
    menubar: 'file edit view insert format tools table tc help',
    toolbar: 'undo redo | bold italic underline strikethrough | fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media pageembed template link anchor codesample | a11ycheck ltr rtl | showcomments addcomment',
    toolbar_drawer: 'sliding',
    directionality: this.common.language != 'English' ? "rtl" : "ltr"
  };
  //tinyConfig:any;
  status = this.masterData.data.status;
  user = [];//this.masterData.data.user;
  department = [];//this.masterData.data.department;
  // priorityList = this.common.priorityList;
  priorityList = [];
  attachmentFiles = [];
  createdTime: string;
  createdDate: string;
  dropdownSettings: any;
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  colorTheme = 'theme-green';
  sendDraftBtnLoad = false;
  sendBtnLoad = false;
  closeBtnLoad = false;
  shareBtnLoad = false;
  printBtnLoad = false;
  cloneBtnLoad = false;
  downloadBtnLoad = false;
  approveBtnLoad = false;
  deleteBtnLoad = false;
  returnInfoBtnLoad = false;
  rejectBtnLoad = false;
  autocompleteItems = ['s'];
  btnLoad = false;
  destinationRedirectBtn = false;
  memoModel = {
    MemoID: 0,
    ReferenceNumber: '',
    Title: '',
    SourceOU: '',
    SourceName: '',
    DestinationOU: [],
    DestinationUsername: [],
    ApproverName: 0,
    ApproverDepartment: 0,
    Details: '',
    Private: '1',
    Priority: '',
    Keywords: [],
    Comment: '',
    Attachments: [],
    AttachmentName: '',
    DeleteFlag: '',
    CreatedBy: '',
    UpdatedBy: '',
    CreatedDateTime: new Date(),
    UpdatedDateTime: '',
    Status: 0,
    IsRedirect: 0,
  }
  img_file: any;
  message: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  attachments: any = [];
  AttachmentDownloadUrl = environment.AttachmentDownloadUrl;
  userDestination: any;
  userReceiver: any;
  commonMes: any;
  DestinationDepartmentId: any[];
  pdfSrc: string;
  showPdf: boolean = false;
  uploadProcess: boolean = false;
  uploadPercentage: number;
  currentId: any;
  screenTitle: string;
  approverDepartment: any;
  destinationDepartment: any;
  disableKeywordsField: boolean;
  departments: any = []
  listTypeID = 0;
  subscribedProp: any;
  constructor(private changeDetector: ChangeDetectorRef,public env:EndPointService, private memoservice: MemoService, public common: CommonService, public router: Router, route: ActivatedRoute, public datepipe: DatePipe,
    private modalService: BsModalService) {
    route.url.subscribe(() => {
      this.screenStatus = route.snapshot.data.title;
    });
    if (this.common.language != 'English') {
      this.priorityList = [this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
      // debugger;
      // // this.tinyConfig = {
      // //   plugins: "directionality",
      // //   language: "ar",
      // //   directionality: "rtl",
      // // };
      // this.tinyConfig.plugins = "directionality";
      // this.tinyConfig.directionality = 'rtl';
      // this.tinyConfig.language = 'ar';
    } else {
      this.priorityList = ['High', 'Medium', 'Low', 'VeryLow'];
    }
    
      route.params.subscribe(param => {
      var id = param.id;
      if (id > 0){
        this.loadData(id, this.currentUser.id);
        if(!localStorage.getItem('BreadcrumbsURL')){
          // var typeParam = {
          //   typeID: 1,
          //   memoID: id
          // }
          // this.common.viewMemo(typeParam);
          if (this.common.language == 'English')
            this.memoservice.breadscrumChange(1, this.screenStatus, id);
          else
            this.memoservice.breadscrumChange(1, this.screenStatus, id, 'ar');
            localStorage.removeItem('BreadcrumbsURL');
        }else{
          var breadcrumb = localStorage.getItem('BreadcrumbsURL').split('>>');
          if(breadcrumb[1]){
            if(this.common.language == 'English')
              this.memoservice.breadscrumChange(+breadcrumb[1], this.screenStatus, id);
            else
              this.memoservice.breadscrumChange(+breadcrumb[1], this.screenStatus, id, 'ar');
          }
          localStorage.removeItem('BreadcrumbsURL');
        }
      }
      else{
        if(this.screenStatus = 'Create'){
          if (this.common.language == 'English')
            this.memoservice.breadscrumChange(0, this.screenStatus, 0);
          else
            this.memoservice.breadscrumChange(0, this.screenStatus, 0, 'ar');
        }
      }
    });

    
    
    this.memoservice.getMemo('memo', 0, 0).subscribe((data: any) => {
      var calendar_id = environment.calendar_id;
      this.destinationDepartment = data.OrganizationList.filter(res => calendar_id != res.OrganizationID);
      this.departments = data.OrganizationList;
      this.approverDepartment = data.M_ApproverDepartmentList;
    });
    this.memoModel.ApproverDepartment = this.currentUser.DepartmentID;
    this.getDestUserList(this.currentUser.DepartmentID);
  }

  async loadData(id, userid) {
    this.currentId = id;
    // if (this.common.language != 'English') {
    //   this.priorityList = [this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
    // } else {
    //   this.priorityList = ['High', 'Medium', 'Low', 'Verylow'];
    // }
    await this.memoservice.getMemo('memo', id, userid).subscribe((data: any) => {
      this.memoData = data;
      console.log(this.memoData.HistoryLog);
      if (this.screenStatus == 'View' || this.screenStatus == 'Edit') {
        let that = this;
        this.status = this.memoData.M_LookupsList;
        var date = this.memoModel.CreatedDateTime;
        this.memoModel.CreatedDateTime = new Date(date);
        this.setData(this.memoData);
      } else {
        this.initPage();
        this.bottonControll();
      }
    });
  }

  async setData(data) {
    await this.getDestUserList(+data.ApproverDepartmentID);
    await this.getRecvPrepareUserList(data.DestinationDepartmentID);
    this.memoModel.MemoID = data.MemoID;
    this.memoModel.ReferenceNumber = data.ReferenceNumber
    this.memoModel.Title = data.Title;

    let params = [];
    let userId = (this.screenStatus == 'View') ? 0 : this.currentUser.id;
    this.getSouceName(data.SourceName, data.SourceOU);
    const DestinationOU = [];
    const DestinationUsername = [];
    data.DestinationDepartmentID.forEach((department, index) => {
      DestinationOU.push(department.MemoDestinationDepartmentID);
    });
    this.DestinationDepartmentId = DestinationOU;
    this.memoModel.DestinationOU = DestinationOU;
    data.DestinationUsernameID.forEach((user, index) => {
      DestinationUsername.push(user.MemoDestinationUsersID);
    });
    this.memoModel.DestinationUsername = DestinationUsername;
    this.memoModel.ApproverName = +data.ApproverNameID;
    this.memoData.ApproverId = data.CurrentApprover[0].ApproverId;
    this.memoModel.ApproverDepartment = +data.ApproverDepartmentID;
    this.memoModel.Details = data.Details;
    this.tinyDetail.nativeElement.insertAdjacentHTML('beforeend', this.memoModel.Details);
    this.memoModel.Private = data.Private;
    this.memoModel.Priority = this.priorityList[data.Priority];
    this.memoModel.CreatedBy = data.CreatedBy;
    this.memoModel.CreatedDateTime = new Date(data.CreatedDateTime);
    this.attachments = data.Attachments;
    this.memoModel.IsRedirect = data.IsRedirect;

    let Keywords = [];
    if (data.Keywords.length) {
      data.Keywords.forEach(key => {
        if (key.userID != data.CreatedBy) {
          Keywords.push({ display: key.keywords, value: key.keywords, readonly: true });
        } else {
          Keywords.push({ display: key.keywords, value: key.keywords, readonly: false });
        }
      });
    }
    this.memoModel.Keywords = Keywords;

    // data.Keywords.forEach(key => {
    //   this.memoModel.Keywords.push({ display: key.keywords, value: key.keywords });
    // });

    this.memoModel.Attachments = data.Attachments;
    this.memoModel.Status = data.Status;
    await this.bottonControll();
    // if (this.common.language == 'English')
    //   this.memoservice.breadscrumChange(this.listTypeID, this.screenStatus, this.currentId);
    // else
    //   this.memoservice.breadscrumChange(this.listTypeID, this.screenStatus, this.currentId, 'ar');

    if (this.screenStatus == 'View') {
      if (data.Status == 2 && this.currentUser.id == data.ApproverId) {
        this.disableKeywordsField = false; // enable for approver
      } else {
        this.disableKeywordsField = true; // disable for normal users
      }
    } else {
      this.disableKeywordsField = false; // enable in edit screen
    }

  }

  async ngOnInit() {
    if (this.common.language != 'English') {
      switch (this.screenStatus) {
        case 'Create':
          this.screenTitle = this.arabic('memocreate');
          break;
        case 'View':
          this.screenTitle = this.arabic('memoView');
          break;
        case 'Edit':
          this.screenTitle = this.arabic('edit');
          break;
      }
      // this.tinyConfig = {
      //   plugins: "directionality",
      //   language: "ar",
      //   directionality: "rtl",
      // };
    }
    this.bottonControll();
  //  this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
    this.common.memoViewChanged$.subscribe(type => {
      this.listTypeID = type.typeID;
      console.log('breadcrumb:' +type);
      if (this.common.language == 'English')
      this.memoservice.breadscrumChange(this.listTypeID, '', type.memoID);
      else
      this.memoservice.breadscrumChange(this.listTypeID, '', type.memoID, 'ar'); 
     });
  }

  ngAfterViewInit() {

  }

  closemodal() {
    this.modalService.hide(1);
    setTimeout(function () { location.reload(); }, 1000);
  }


  initPage() {
    this.memoModel.MemoID = 0
    this.memoModel.ReferenceNumber = '';
    this.memoModel.Title = '';
    this.memoModel.SourceOU = this.currentUser.department;
    this.memoModel.SourceName = this.currentUser.username;
    this.memoModel.DestinationOU = [];
    this.memoModel.DestinationUsername = [];
    this.memoModel.ApproverName = 0;
    this.memoModel.ApproverDepartment = 0;
    this.memoModel.Details = '';
    this.memoModel.Private = 'yes';
    this.memoModel.Priority = '';
    this.memoModel.Keywords = [];
    this.memoModel.Comment = '';
    this.memoModel.Attachments = [];
    this.memoModel.AttachmentName = '';
    this.memoModel.DeleteFlag = '';
    this.memoModel.CreatedBy = this.currentUser.id;
    this.memoModel.UpdatedBy = '';
    this.memoModel.CreatedDateTime = new Date();
    this.memoModel.UpdatedDateTime = '';
    this.memoModel.Status = 0;
    if (this.screenStatus == 'Create') {
      this.disableKeywordsField = false;
    }
  }

  onTextChange(event) {
    if (event != '')
      this.memoModel.Keywords.push({ display: event, value: event });
  }

  Attachments(event) {
    var files = event.target.files;
    if (files.length > 0) {
      let that = this;
      this.uploadProcess = true;
      this.common.postAttachment(files).subscribe((event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.myInputVariable.nativeElement.value = "";
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'MemoID': '' });
          }
          this.memoModel.Attachments = this.attachments;
        }
      });
    }
  }
  selectChange(data) {
    this.memoModel.DestinationOU;
  }

  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    // if (this.attachments.length == 0) {
    this.myInputVariable.nativeElement.value = "";
    //}
  }


  prepareData() {
    this.memoModel.Keywords.forEach((data, index) => {
      this.createMemo.Keywords.push({ 'keywords': data.value });
    });
    if (typeof this.DestinationDepartmentId !== 'undefined' && this.DestinationDepartmentId.length) {
      this.DestinationDepartmentId.forEach(data => {
        this.createMemo.DestinationDepartmentId.push({
          "MemoDestinationDepartmentID": data,
          'MemoDestinationDepartmentName': ''
        });
      });
    }
    this.userName();
    this.createMemo.Title = this.memoModel.Title;
    this.createMemo.SourceOU = this.currentUser.DepartmentID;
    this.createMemo.SourceName = this.currentUser.UserID;
    this.createMemo.ApproverId = this.memoModel.ApproverName;
    this.createMemo.ApproverDepartmentId = this.memoModel.ApproverDepartment;
    this.createMemo.Details = this.memoModel.Details;
    this.createMemo.Private = this.memoModel.Private;
    this.createMemo.Priority = this.priorityList.indexOf(this.memoModel.Priority).toString();
    this.createMemo.Attachments = this.memoModel.Attachments;
    //this.createMemo.Action = this.memoModel.Status+'';
    this.createMemo.Comments = '';
    return this.createMemo;
  }

  validateForm() {
    var flag = true;
    var destination = (this.memoModel.DestinationOU) ? (this.memoModel.DestinationOU.length > 0) : false;
    var Keywords = (this.memoModel.Keywords) ? (this.memoModel.Keywords.length > 0) : false;
    var username = (this.memoModel.DestinationUsername) ? (this.memoModel.DestinationUsername.length > 0) : false;

    if (destination && username && this.memoModel.Title && this.memoModel.ApproverName
      && this.memoModel.ApproverDepartment && this.memoModel.Details && this.memoModel.Private
      && this.memoModel.Priority && !this.sendBtnLoad) {
      flag = false;
    }
    return flag;
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
  id = '';
  printbtn = false;
 

  bottonControll() {
    if (this.screenStatus == 'Create') {
      this.createBtnShow = true;
      this.printbtn = true;
    } else if (this.screenStatus == 'Edit') {
      this.editBtnShow = true;
    } else if (this.screenStatus == 'View' && this.memoModel.CreatedBy == this.currentUser.id) {
      this.viewBtnShow = true;
    }
    if (this.memoModel.CreatedBy == this.currentUser.id && this.memoModel.Status == 5) {
      this.creatorBtn = true;
    }
    if (this.memoModel.CreatedBy == this.currentUser.id && this.memoModel.Status == 1) {
      this.draftBtn = true;
    }
    if (this.memoModel.CreatedBy == this.currentUser.id && this.memoModel.Status != 1) {
      this.cloneBtn = true;
    }
    if (this.screenStatus == 'View' && this.memoData.ApproverId == this.currentUser.id && this.memoModel.Status == 2) {
      this.approverBtn = true;
    }

    if(this.memoModel.Status == 3){
      if(this.memoModel.DestinationUsername.find(item => item == this.currentUser.id)){
        if(this.memoModel.IsRedirect == 1 || this.memoModel.IsRedirect == null){
          this.receiverBtn = false;
          this.destinationRedirectBtn = false;
        }else{
          this.receiverBtn = true;
          this.destinationRedirectBtn = true;
        }
        this.editBtnShow = false;
      }else{
        if(this.memoModel.IsRedirect == 1 || this.memoModel.IsRedirect == null){
          this.receiverBtn = false;
          this.destinationRedirectBtn = false;
        }else{
          this.receiverBtn = true;
          this.destinationRedirectBtn = true;
        }
      }
    }
    
    // this.memoModel.DestinationUsername.forEach(element => {
    //   if(this.memoModel.Status == 3){
    //     if(element == this.currentUser.id) {
    //       if(this.memoModel.IsRedirect == 1){
    //         this.receiverBtn = false;
    //       }else{
    //         this.receiverBtn = true;
    //       }
    //       // this.receiverBtn = true;
    //       this.editBtnShow = false;
    //     }else{
    //       if(this.memoModel.IsRedirect == 1 || this.memoModel.IsRedirect == null){
    //         this.receiverBtn = false;
    //       }else{
    //         this.receiverBtn = true;
    //       }
    //     }
    //   }
    // });
    if (this.memoModel.CreatedBy == this.currentUser.id && this.memoModel.Status == 1) {
      this.deleteBtn = true;
    }
  }

  async Destination(event) {
    this.memoModel.DestinationUsername = [];
    this.DestinationDepartmentId = this.memoModel.DestinationOU;
    await this.getRecvUserList(this.DestinationDepartmentId);
  }

  userName(event = '') {
    this.createMemo.DestinationUserId = [];
    if (this.memoModel.DestinationUsername.length) {
      if (this.memoModel.DestinationUsername.find(x => x == 'allSelect')) {
        this.userReceiver.forEach(data => {
          if (data.UserID != 'allSelect') {
            this.createMemo.DestinationUserId.push({
              'MemoDestinationUsersID': data.UserID,
              'MemoDestinationUsersName': ''
            });
          }
        })
      } else {
        this.memoModel.DestinationUsername.forEach(data => {
          this.createMemo.DestinationUserId.push({
            'MemoDestinationUsersID': data,
            'MemoDestinationUsersName': ''
          });
        });
      }
    }
  }

  onChangeDepartment() {
    this.memoModel.ApproverName = 0;
    this.getDestUserList(+this.memoModel.ApproverDepartment);
  }

  async getRecvUserList(departments) {
    let params = [];
    let userId = (this.screenStatus == 'View') ? 0 : this.currentUser.id;
    if (departments.length) {
      departments.forEach(element => {
        params.push({ "OrganizationID": element, "OrganizationUnits": "string" });
      });
      let Duser = [], selectAll;
      if (this.common.language == 'English') {
        selectAll = { UserID: 'allSelect', EmployeeName: 'All' };
      } else { selectAll = { UserID: 'allSelect', EmployeeName: this.arabic('all') }; }
      this.common.getUserList(params, userId).subscribe((data: any) => {
        Duser.push(selectAll);
        data.forEach(user => {
          Duser.push(user);
        });
        this.userReceiver = Duser;
      });
    } else {
      this.userReceiver = [];
    }
  }

  async getRecvPrepareUserList(departments) {
    let params = [];
    let userId = (this.screenStatus == 'View') ? 0 : this.currentUser.id;
    if (departments.length) {
      departments.forEach(element => {
        params.push({ "OrganizationID": element.MemoDestinationDepartmentID, "OrganizationUnits": "string" });
      });
    }
    let Duser = [], selectAll;
    if (this.common.language == 'English') {
      selectAll = { UserID: 'allSelect', EmployeeName: 'All' };
    } else { selectAll = { UserID: 'allSelect', EmployeeName: this.arabic('all') }; }
    this.common.getUserList(params, userId).subscribe((data: any) => {
      Duser.push(selectAll);
      data.forEach(user => {
        Duser.push(user);
      });
      this.userReceiver = Duser;
    });
  }

  async getDestUserList(id) {
    if (id) {
      this.userDestination = [];
      let userId = (this.screenStatus == 'View') ? 0 : this.currentUser.id;
      let params = [{
        "OrganizationID": id,
        "OrganizationUnits": "string"
      }];
      this.common.getmemoUserList(params, userId).subscribe((data: any) => {
        this.userDestination = data;
      });
    } else {
      this.userDestination = [];
    }
  }

  async getSouceName(UserID, DepID) {
    let params = [{
      "OrganizationID": DepID,
      "OrganizationUnits": "string"
    }];
    this.common.getUserList(params, 0).subscribe((data: any) => {
      this.memoModel.SourceName = data.find(x => x.UserID == UserID).EmployeeName.toString();
      this.memoModel.SourceOU = this.departments.find(x => x.OrganizationID == DepID).OrganizationUnits;
    });
  }


  saveMemo(data = '') {
    if (data == 'draft' && this.memoModel.MemoID == 0) {
      var requestData = this.prepareData();
      requestData.CreatedBy = this.currentUser.id;
      requestData.CreatedDateTime = this.memoModel.CreatedDateTime;
      requestData.Action = 'Save';
      this.memoservice.saveMemo('memo', requestData).subscribe(data => {
        this.sendDraftBtnLoad = false;
        this.sendBtnLoad = false;
        if (this.common.language == 'English') {
          this.message = "Memo Drafted Successfully.";
        } else {
          this.message = this.arabic('memodraftsuccess');
        }
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        this.bsModalRef.content.message = this.message;
        this.bsModalRef.content.pagename = 'memo';
      });
    } else {
      if (this.memoModel.Status == 1 || this.memoModel.Status == 5) {
        requestData = this.prepareData();
        requestData.Action = 'Submit';
        if (data == 'draft') {
          requestData.Action = 'Save';
        }
        if (this.memoModel.Status == 5) {
          requestData.Action = 'Resubmit';
        }
        requestData.MemoID = this.memoModel.MemoID
        requestData.UpdatedBy = this.currentUser.id;
        requestData.UpdatedDateTime = this.memoModel.CreatedDateTime;
        this.memoservice.updateMemo('memo', this.memoModel.MemoID, requestData).subscribe(res => {
          this.sendDraftBtnLoad = false;
          this.sendBtnLoad = false;
          if (this.screenStatus == 'Create' || data != 'draft') {
            if (this.common.language == 'English') {
              this.message = "Memo Sent Successfully";
            } else {
              this.message = this.arabic('memosentsuccess');
            }
            this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
            this.bsModalRef.content.message = this.message;
            this.bsModalRef.content.screenStatus = this.screenStatus;
            this.bsModalRef.content.pagename = 'memo';

          } else {
            if (this.common.language == 'English') {
              this.message = "Memo Drafted Successfully.";
            } else {
              this.message = this.arabic('memodraftsuccess');
            }
            this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
            this.bsModalRef.content.message = this.message;
            this.bsModalRef.content.pagename = 'memo';
          }
        });
      } else {
        requestData = this.prepareData();
        requestData.Action = 'Submit';
        requestData.CreatedBy = this.currentUser.id;
        requestData.CreatedDateTime = this.memoModel.CreatedDateTime;
        this.memoservice.saveMemo('memo', requestData).subscribe(data => {
          this.sendDraftBtnLoad = false;
          this.sendBtnLoad = false;
          if (this.screenStatus == 'Create') {
            if (this.common.language == 'English') {
              this.message = "Memo Sent Successfully";
            } else {
              this.message = this.arabic('memosentsuccess');
            }
            this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
            this.bsModalRef.content.message = this.message;
            this.bsModalRef.content.screenStatus = this.screenStatus;
            this.bsModalRef.content.pagename = 'memo';

          } else {
            if (this.common.language == 'English') {
              this.message = "Memo Drafted Successfully.";
            } else {
              this.message = this.arabic('memodraftsuccess');
            }
            this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
            this.bsModalRef.content.message = this.message;
            this.bsModalRef.content.pagename = 'memo';
          }
        });
      }
    }
  }


  statusChange(status: any, dialog) {
    var data = this.formatPatch(status, 'Action')
    this.commonMes = status;
    if (status == 'ReturnForInfo') {
      let updateKeywords = [];
      this.memoModel.Keywords.forEach((item, index) => {
        updateKeywords.push({ 'keywords': item.value });
      });
      this.memoservice.updateKeywords('Memo/Keywords', this.memoModel.MemoID, updateKeywords, this.currentUser.id).subscribe(result => {
        console.log(result);
      });
    }
    this.memoservice.statusChange('Memo', this.memoModel.MemoID, data).subscribe(data => {
      this.closeBtnLoad = false;
      this.shareBtnLoad = false;
      this.approveBtnLoad = false;
      this.rejectBtnLoad = false;
      this.returnInfoBtnLoad = false;
      this.btnLoad = false;
      if (status == 'ReturnForInfo') {
        this.message = (this.common.language == 'English') ? "Memo Returned For Info Successfully" : this.arabic('memoreturnsuccess');
      } else if (status == 'Approve') {
        this.message = (this.common.language == 'English') ? "Memo Approved Successfully" : this.arabic('memoapprovesuccess');
      } else if (status == 'Reject') {
        this.message = (this.common.language == 'English') ? "Memo Rejected Successfully" : this.arabic('memorejectsuccess');
      } else if (status == 'Close') {
        this.message = (this.common.language == 'English') ? "Memo Closed Successfully" : this.arabic('memoclosesuccess');
      } else {
        this.message = (this.common.language == 'English') ? 'Memo ' + status + 'd Successfully' : this.arabic('memo' + status + 'success');
      }
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'memo';
    });
  }

  popup(status: any, btn?: boolean) {
    this.bsModalRef = this.modalService.show(ModalComponent, this.config);
    this.bsModalRef.content.status = status;
    this.bsModalRef.content.button = (status == 'Redirect') ? 'Redirect' : 'Escalate';
    this.bsModalRef.content.screenStatus = this.screenStatus;
    this.bsModalRef.content.fromScreen = 'Memo';
    this.bsModalRef.content.destination = btn;
    this.bsModalRef.content.Comments = this.memoModel.Comment;
    this.bsModalRef.content.memoid = this.memoModel.MemoID;
    this.bsModalRef.content.onClose.subscribe(result => {
      this.btnLoad = result;
    });
  }

  returnForInfo() {

  }
  clone() {
    this.memoservice.cloneMemo('Memo/Clone', this.memoModel.MemoID, this.currentUser.id).subscribe(res => {
      this.cloneBtnLoad = false;
      this.btnLoad = false;
      this.message = (this.common.language == 'English') ? "Memo Cloned Successfully" : this.arabic('memoclonesuccess');
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.page_url = 'app/memo/memo-edit/' + res;
      this.bsModalRef.content.pagename = 'Memo Clone';
      //this.loadData(res, this.currentUser.id);
    });
  }
  shareMemo() {
    this.bsModalRef = this.modalService.show(ModalComponent, this.config);
    this.bsModalRef.content.status = 'Share Memo';
    this.bsModalRef.content.button = 'Share Memo';
    this.bsModalRef.content.fromScreen = 'Memo';
    this.bsModalRef.content.destination = true;
    this.bsModalRef.content.memoid = this.memoModel.MemoID;
    this.bsModalRef.content.Comments = this.memoModel.Comment;
    this.bsModalRef.content.onClose.subscribe(result => {
      this.shareBtnLoad = result;
      this.btnLoad = result;
    });

  }

  print(template: TemplateRef<any>) {
    this.memoservice.printPreview('Memo/preview', this.memoModel.MemoID, this.currentUser.id).subscribe(res => {
      if (res) {
        this.memoservice.pdfToJson(this.memoModel.ReferenceNumber).subscribe((data: any) => {
          this.showPdf = true;
          this.pdfSrc = data;
          this.bsModalRef = this.modalService.show(template, { class: 'modal-xl' });
          this.common.deleteGeneratedFiles('files/delete', this.memoModel.ReferenceNumber + '.pdf').subscribe(result => {
            console.log(result);
          });
        });
      }
    });
  }


  downloadPrint() {
    this.memoservice.printPreview('Memo/preview', this.memoModel.MemoID, this.currentUser.id).subscribe(res => {
      if (res) {
        this.common.previewPdf(this.memoModel.ReferenceNumber)
          .subscribe((data: Blob) => {
            this.downloadBtnLoad = false;
            this.btnLoad = false;
            this.printBtnLoad = false;
            var url = window.URL.createObjectURL(data);
            var a = document.createElement('a');
            document.body.appendChild(a);
            a.setAttribute('style', 'display: none');
            a.href = url;
            a.download = this.memoModel.ReferenceNumber + '.pdf';
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
            this.common.deleteGeneratedFiles('files/delete', this.memoModel.ReferenceNumber).subscribe(result => {
              console.log(result);
            });
          });
      }
    });
    //   this.common.previewPdf(this.memoModel.ReferenceNumber)
    // .subscribe((data: Blob) => {
    //   this.downloadBtnLoad = false;
    //   this.btnLoad = false;
    //   this.printBtnLoad = false;
    //   var url = window.URL.createObjectURL(data);
    //   var a = document.createElement('a');
    //   document.body.appendChild(a);
    //   a.setAttribute('style', 'display: none');
    //   a.href = url;
    //   a.download = this.memoModel.ReferenceNumber + '.pdf';
    //   a.click();
    //   window.URL.revokeObjectURL(url);
    //   a.remove();
    // });

  }

  printPdf(html: ElementRef<any>) {
    this.btnLoad = false;
    this.printBtnLoad = false;
    this.memoservice.printPreview('Memo/preview', this.memoModel.MemoID, this.currentUser.id).subscribe(res => {
      if(res){
          this.common.printPdf(this.memoModel.ReferenceNumber);
      }
    });
  }

  delete(id) {
    this.memoservice.deleteMemo('memo', this.memoModel.MemoID).subscribe(data => {
      this.deleteBtnLoad = false;
      this.btnLoad = false;
      console.log('deleted' + data);
      this.message = (this.common.language == 'English') ? "Memo Deleted Successfully" : this.arabic('memodeletesuccess');
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'memo';
    });

  }

  closePrintPop() {
    this.btnLoad = false;
    this.printBtnLoad = false;
    this.bsModalRef.hide();
  }



  formatPatch(val, path) {
    var data = [{
      "value": val,
      "path": path,
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
      "value": this.memoModel.Comment,
      "path": "Comments",
      "op": "replace"
    }];
    return data;
  }

  hisLog(status) {
    return this.historyLog(status);
  }

  historyLog(status) {
    if (status == 'Reject') {
      return (this.common.language == 'English') ? status + 'ed' : this.common.arabic.words['rejectedby'];
    } else if (status == 'Redirect') {
      return (this.common.language == 'English') ? status + 'ed' : this.common.arabic.words['redirectedby'];
    } else if (status == 'Submit') {
      return (this.common.language == 'English') ? 'Submitted' : this.common.arabic.words['submittedby'];
    } else if (status == 'Resubmit') {
      return (this.common.language == 'English') ? status + 'ted' : this.common.arabic.words['resubmittedby'];
    } else if (status == 'ReturnForInfo') {
      return (this.common.language == 'English') ? 'ReturnForInfo' : this.common.arabic.words['returnforinfoby'];
    } else if (status == 'Save') {
      return (this.common.language == 'English') ? 'Saved' : this.common.arabic.words['memoSaved'];
    } else if (status == 'Escalate') {
      return (this.common.language == 'English') ? 'Escalated' : this.common.arabic.words['memoescalatedby'];
    } else if (status == 'Approve') {
      return (this.common.language == 'English') ? 'Approved' : this.common.arabic.words['approvedby'];
    } else if (status == 'Share') {
      return (this.common.language == 'English') ? 'Shared' : this.common.arabic.words['sharedby'];
    } else {
      return (this.common.language == 'English') ? status + 'd' : this.common.arabic.words[status + 'dby'].toLocaleLowerCase();
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  setMemoType(id){
    this.listTypeID = id;
  }

  showSpanForEscalateRedirect(action){
    if(action === 'Redirect' || action === 'Escalate'){
      return true
    }else{
      return false
    }
  }

}
