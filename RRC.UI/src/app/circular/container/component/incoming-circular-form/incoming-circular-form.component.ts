import { Component, OnInit, ViewChild, TemplateRef, ElementRef, ChangeDetectorRef } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { FormGroup, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CreateCircularModal } from './create-circular.modal';
//import { SampleData } from './sampleDB';
import 'tinymce';
import { async } from 'q';
import { DatePipe } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../../../../modal/modalcomponent/modal.component';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { CommonService } from '../../../../common.service';
import { CircularService } from '../../../service/circular.service';
import { HttpEventType } from '@angular/common/http';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { environment } from 'src/environments/environment';

declare var tinymce: any;

@Component({
  selector: 'app-circular-form',
  templateUrl: './incoming-circular-form.component.html',
  styleUrls: ['./incoming-circular-form.component.scss']
})
export class IncomingCircularFormComponent implements OnInit {
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  @ViewChild('template') template: TemplateRef<any>;
  @ViewChild('variable') myInputVariable: ElementRef;
  @ViewChild('printContent') printContent: ElementRef<any>;
  @ViewChild('tinyDetail') tinyDetail: ElementRef;
  bsModalRef: BsModalRef;
  createCircular: CreateCircularModal = new CreateCircularModal();
  screenStatus = 'Create';
  displayStatus: any = 'CREATION';
  masterData: any;
  circularData: any = {
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
  priorityList = [];
  // documentList = this.common.documentList;
  attachmentFiles = [];
  createdTime: string;
  createdDate: string;
  submitted = false;

  dropdownSettings: any;

  colorTheme = 'theme-green';
  incomingcircular = {
    CircularID: 0,
    ReferenceNumber: '',
    Title: '',
    SourceOU: '',
    SourceName: '',
    DestinationOU: [],
    ApproverName: 0,
    CurrentApprover: 0,
    ApproverDepartment: 0,
    Details: '',
    Priority: '',
    Comments: '',
    Attachments: [],
    AttachmentName: '',
    DeleteFlag: '',
    CreatedBy: '',
    UpdatedBy: '',
    HistoryLog: [],
    CreatedDateTime: new Date(),
    UpdatedDateTime: '',
    Status: 0,
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
  downloadBtnLoad = false;
  approveBtnLoad = false;
  deleteBtnLoad = false;
  returnInfoBtnLoad = false;
  btnLoad = false;
  currentId: any;
  approverDepartment: any;
  destinationDepartment: any;
  OrganizationList: any;
  AttachmentDownloadUrl = environment.AttachmentDownloadUrl;
  
  tinyConfig = {
    plugins: 'powerpaste casechange importcss tinydrive searchreplace directionality visualblocks visualchars fullscreen table charmap hr pagebreak nonbreaking toc insertdatetime advlist lists checklist wordcount tinymcespellchecker a11ychecker imagetools textpattern noneditable help formatpainter permanentpen pageembed charmap tinycomments mentions quickbars linkchecker emoticons',
    language: this.common.language != 'English' ? "ar" : "en",
    menubar: 'file edit view insert format tools table tc help',
    toolbar: 'undo redo | bold italic underline strikethrough | fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media pageembed template link anchor codesample | a11ycheck ltr rtl | showcomments addcomment',
    toolbar_drawer: 'sliding',
    directionality: this.common.language != 'English' ? "rtl" : "ltr"
  };
  
  constructor(private changeDetector: ChangeDetectorRef, public common: CommonService, public router: Router, public route: ActivatedRoute, public datepipe: DatePipe,
    private modalService: BsModalService, private circularService: CircularService,public util:UtilsService) {
    // this.priorityList = this.common.priorityList;
    route.url.subscribe(() => {
      this.screenStatus = route.snapshot.data.title;
    });
    console.log("currentLang", this.common.currentLang);
    route.params.subscribe(param => {
      var id = +param.id;
      if (id > 0){
        this.loadData(id, this.currentUser.id);
        if(!localStorage.getItem('BreadcrumbsURL')){
          // var typeParam = {
          //   typeID: 1,
          //   circularID: id
          // }
          // this.common.viewCircular(typeParam);
          if (this.common.language == 'English')
            this.circularService.breadscrumChange(1, this.screenStatus, id);
          else
            this.circularService.breadscrumChange(1, this.screenStatus, id, 'ar');
            localStorage.removeItem('BreadcrumbsURL');
        }else{
          var breadcrumb = localStorage.getItem('BreadcrumbsURL').split('>>');
          if(breadcrumb[1]){
            if(this.common.language == 'English')
              this.circularService.breadscrumChange(+breadcrumb[1], this.screenStatus, id);
            else
              this.circularService.breadscrumChange(+breadcrumb[1], this.screenStatus, id, 'ar');
          }
          localStorage.removeItem('BreadcrumbsURL');
        }
      }else{
        if(this.screenStatus = 'Create'){
        if (this.common.currentLang == 'en')
          this.circularService.breadscrumChange(0, this.screenStatus, id);
        else
          this.circularService.breadscrumChange(0, this.screenStatus, id, 'ar');
        }
      }
    });
   
    this.circularService.getCircular('Circular', 0, 0).subscribe((data: any) => {
      var calendar_id = environment.calendar_id;
      this.approverDepartment = data.M_ApproverDepartmentList;
      var dep = [];
      if (this.common.currentLang == 'en'){
      dep.push({"OrganizationID": 0, "OrganizationUnits": 'All'});
      }else{
        dep.push({"OrganizationID": 0, "OrganizationUnits": this.arabic('all')});
      }
      data.OrganizationList.map(res=>{
      dep.push(res);
      })
      this.destinationDepartment = dep.filter(res => calendar_id != res.OrganizationID);
    });
    if (this.screenStatus == 'Create') {
      this.displayStatus = 'CREATION';
      this.onChangeDepartment();
    }
    if (this.screenStatus == 'View') {
      this.displayStatus = 'VIEW';
    }
    if (this.screenStatus == 'Edit') {
      this.displayStatus = 'EDIT';
    }
    if(this.common.currentLang == 'en'){
      this.priorityList = ['High', 'Medium', 'Low', 'VeryLow'];
    }else{
      this.priorityList = [this.arabic('high'), this.arabic('medium'), this.arabic('low'), this.arabic('verylow')];
    }
  }

  async loadData(id, userid) {
    this.currentId = id;
    await this.circularService.getCircular('Circular', id, userid).subscribe((data: any) => {
      this.circularData = data;
      if (this.screenStatus == 'View' || this.screenStatus == 'Edit') {
        let that = this;
        this.status = this.circularData.M_LookupsList;
        var date = this.incomingcircular.CreatedDateTime;
        this.incomingcircular.CreatedDateTime = new Date(date);
        this.incomingcircular.CreatedBy = this.circularData.CreatedBy;
        this.department = this.circularData.OrganizationList;
        this.setData(this.circularData);
        this.bottonControll();
      } else {
        this.initPage();
        this.bottonControll();
      }
    });
  }

  async setData(data) {
    this.getDestUserList(+data.ApproverDepartmentId);
    //this.getRecvPrepareUserList(data.DestinationDepartmentID);
    this.incomingcircular.CircularID = data.CircularID;
    this.incomingcircular.ReferenceNumber = data.ReferenceNumber
    this.incomingcircular.Title = data.Title;
    this.OrganizationList=data.OrganizationList;
    let params = [];
    let userId = (this.screenStatus == 'View') ? 0 : this.currentUser.id;
    this.getSouceName(data.SourceName, data.SourceOU);
    //this.incomingcircular.SourceName = data.SourceName;

    const DestinationOU = [];
    const DestinationUsername = [];
    data.DestinationDepartmentID.forEach((department, index) => {
      DestinationOU.push(department.CircularDestinationDepartmentID);
    });
    this.incomingcircular.DestinationOU = DestinationOU;
    this.DestinationDepartmentId = this.incomingcircular.DestinationOU;
    // data.DestinationUsernameID.forEach((user,index)=>{
    //   DestinationUsername.push(user.MemoDestinationUsersID);
    // });
    //this.incomingcircular.DestinationUsername = DestinationUsername;
    if (data.CurrentApprover.length > 0) {
      this.incomingcircular.CurrentApprover = data.CurrentApprover[0].ApproverId;
    }
    this.incomingcircular.ApproverName = data.ApproverId; //Check this set
    this.incomingcircular.ApproverDepartment = +data.ApproverDepartmentId; //check this set
    this.incomingcircular.Details = data.Details;
    this.incomingcircular.Priority = this.priorityList[data.Priority];
    this.tinyDetail.nativeElement.insertAdjacentHTML('beforeend', this.incomingcircular.Details);
    this.getDestUserList(+data.ApproverDepartmentId);
    this.incomingcircular.CreatedBy = data.CreatedBy;
    this.attachments = data.Attachments;
    this.incomingcircular.HistoryLog = data.HistoryLog;

    this.incomingcircular.Attachments = data.Attachments;
    this.incomingcircular.Status = data.Status;
  //   if (this.common.currentLang == 'en')
  //     this.circularService.breadscrumChange(this.incomingcircular.Status, this.screenStatus, this.currentId);
  //   else
  //     this.circularService.breadscrumChange(this.incomingcircular.Status, this.screenStatus, this.currentId, 'ar');
 }

  async ngOnInit() {
    if (this.common.currentLang != 'en') {
      switch (this.screenStatus) {
        case 'Create':
          this.displayStatus = this.arabic('circularcreate');
          break;
        case 'View':
          this.displayStatus = this.arabic('circularview');
          break;
        case 'Edit':
          this.displayStatus = this.arabic('circularedit');
          break;
      }

    }
    this.bottonControll();
    //this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
    this.incomingcircular.ApproverDepartment = this.currentUser.DepartmentID;
    this.onChangeDepartment();
    this.common.circularViewChanged$.subscribe(type => {
      console.log('breadcrumb:' +type);
      if (this.common.language == 'English')
      this.circularService.breadscrumChange(type.typeID, '', type.circularID);
      else
      this.circularService.breadscrumChange(type.typeID, '', type.circularID, 'ar'); 
     });
}
    
  public tinyMceSettings = {
    skin_url: 'assets/tinymce/skins/lightgray',
    inline: false,
    statusbar: false,
    browser_spellcheck: true,
    height: 320,
    plugins: 'fullscreen',
  };


  ngAfterViewInit() {

  }

  closemodal() {
    this.modalService.hide(1);
    //this.router.navigate(['app/circular/circular-list']);
    this.router.navigate(['app/circular/circular-list'], { relativeTo: this.route });
  }


  initPage() {
    this.incomingcircular.CircularID = 0
    this.incomingcircular.ReferenceNumber = '';
    this.incomingcircular.Title = '';
    this.incomingcircular.SourceOU = this.currentUser.department;
    this.incomingcircular.SourceName = this.currentUser.username;
    this.incomingcircular.DestinationOU = [];
    // this.incomingcircular.DestinationUsername = [];
    this.incomingcircular.ApproverName = 0;
    this.incomingcircular.ApproverDepartment = this.currentUser.DepartmentID;
    this.incomingcircular.Details = '';
    this.incomingcircular.Priority = '';
    this.incomingcircular.Comments = '';
    this.incomingcircular.Attachments = [];
    this.incomingcircular.AttachmentName = '';
    this.incomingcircular.DeleteFlag = '';
    this.incomingcircular.CreatedBy = this.currentUser.id;
    this.incomingcircular.UpdatedBy = '';
    this.incomingcircular.CreatedDateTime = new Date();
    this.incomingcircular.UpdatedDateTime = '';
    this.incomingcircular.Status = 0;
    
  }

  Attachments(event) {
    this.img_file = event.target.files;
    for (var i = 0; i < this.img_file.length; i++) {
      this.attachmentFiles.push(this.img_file[i]);
      this.attachments.push({ 'AttachmentGuid': 0, 'AttachmentsName': this.img_file[i].name, 'CircularID': '' });
    }
    this.incomingcircular.Attachments = this.attachments;
  }
  selectChange(data) {
    this.incomingcircular.DestinationOU;
  }

  deleteAttachment(index) {
    this.attachments.splice(index, 1);
    this.myInputVariable.nativeElement.value = "";
  }


  prepareData() {
    this.createCircular.CreatedDateTime = this.incomingcircular.CreatedDateTime;
    if (this.DestinationDepartmentId.length) {
      this.DestinationDepartmentId.forEach(data => {
        this.createCircular.DestinationDepartmentID.push({
          "CircularDestinationDepartmentID": data,
          'CircularDestinationDepartmentName': ''
        });
      });
    } else {
      this.DestinationDepartmentId = [];
    }
    this.createCircular.Title = this.incomingcircular.Title;
    this.createCircular.SourceOU = this.currentUser.DepartmentID;
    this.createCircular.SourceName = this.currentUser.UserID;
    this.createCircular.ApproverId = this.incomingcircular.ApproverName;
    this.createCircular.ApproverDepartmentId = this.incomingcircular.ApproverDepartment;
    this.createCircular.Details = this.incomingcircular.Details;
    this.createCircular.Priority = this.priorityList.indexOf(this.incomingcircular.Priority).toString();
    this.createCircular.Attachments = this.incomingcircular.Attachments;
    this.createCircular.CreatedBy = this.currentUser.id;
    //this.createMemo.Action = this.memoModel.Status+'';
    this.createCircular.Comments = this.incomingcircular.Comments;



    return this.createCircular;
  }

  validateForm() {
    var flag = true;
    var destination = (this.incomingcircular.DestinationOU) ? (this.incomingcircular.DestinationOU.length > 0) : false;
    //var Keywords = (this.incomingcircular.Keywords) ? (this.incomingcircular.Keywords.length > 0) : false;
    //var username = (this.incomingcircular.DestinationUsername) ? (this.incomingcircular.DestinationUsername.length > 0) : false;
    if (destination && this.incomingcircular.Title && this.incomingcircular.ApproverName
      && this.incomingcircular.ApproverDepartment && this.incomingcircular.Details
      && this.incomingcircular.Priority) {
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
  savedraftBtn = false;
  id = '';
  printbtn = false;



  bottonControll() {
    if (this.screenStatus == 'Create') {
      this.createBtnShow = true;
      this.savedraftBtn = true;
      this.printbtn = true;
    } else if (this.screenStatus == 'Edit') {
      this.editBtnShow = true;
    } else if (this.screenStatus == 'View' && this.incomingcircular.CreatedBy == this.currentUser.id) {
      this.viewBtnShow = true;
    }
    if (this.incomingcircular.CreatedBy == this.currentUser.id) {
      this.creatorBtn = true;
    }
    if (this.incomingcircular.CreatedBy == this.currentUser.id && (this.incomingcircular.Status == 12 || this.incomingcircular.Status == 16) && this.screenStatus == 'Edit') {
      this.draftBtn = true;
    }
    if (this.screenStatus == 'View' && this.incomingcircular.CurrentApprover == this.currentUser.id && this.incomingcircular.Status == 13) {
      this.approverBtn = true;
    }
    // this.incomingcircular.DestinationUsername.forEach(element => {
    //   if (element == this.currentUser.id && this.incomingcircular.Status == 3) {
    //     this.receiverBtn = true;
    //     this.editBtnShow = false;
    //   }
    // });
    if (this.incomingcircular.CreatedBy == this.currentUser.id && this.incomingcircular.Status == 12 && this.screenStatus == 'Edit') {
      this.deleteBtn = true;
      this.savedraftBtn = true;
    }
  }

  async Destination(event) {
    this.DestinationDepartmentId = this.incomingcircular.DestinationOU;
    if(this.incomingcircular.DestinationOU.indexOf(0) !== -1){
      this.DestinationDepartmentId = [];
      this.destinationDepartment.forEach(data => {
        this.DestinationDepartmentId.push(data.OrganizationID);
      });
      this.DestinationDepartmentId = this.DestinationDepartmentId.filter(x => x != 0);
    }
    //await this.getRecvUserList(this.DestinationDepartmentId);
  }


  onChangeDepartment() {
    this.userDestination = [];
    this.getDestUserList(+this.incomingcircular.ApproverDepartment);
    this.incomingcircular.ApproverName = 0;
  }


  async getDestUserList(id) {
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
        if(data && data.length > 0) {
          this.userDestination = data;
        } else {
          this.userDestination = [];
        }
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
      let Users = data;
      let empName = Users.find(x => x.UserID == UserID)
      this.incomingcircular.SourceName = empName ? empName.EmployeeName.toString() : "";
      this.incomingcircular.SourceOU = this.OrganizationList.find(x => x.OrganizationID == DepID).OrganizationUnits;
    });

  }

  saveCircular(data = '') {
    //this.submitted = true;
    if (data == 'draft' && this.incomingcircular.CircularID == 0) {
      var requestData = this.prepareData();
      requestData.Action = 'Save';
      //requestData['CircularID'] = this.incomingcircular.CircularID;
      this.circularService.saveCircular('Circular', requestData).subscribe(result => {

        this.sendDraftBtnLoad = false;
        this.sendBtnLoad = false;
        this.message = "Circular Drafted Successfully";
        if (this.common.currentLang == 'ar') {
          this.message = this.arabic('circulardraftsuccess');
        }
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        this.bsModalRef.content.message = this.message;
        this.bsModalRef.content.pagename = 'Circular';
        //location.reload();
      });
    } else {
      if (this.incomingcircular.Status == 12 || this.incomingcircular.Status == 16) {
        requestData = this.prepareData();
        requestData.Action = 'Submit';
        if (data == 'draft') {
          requestData.Action = 'Save';
        }
        if (this.incomingcircular.Status == 16) {
          requestData.Action = 'Resubmit';
        }
        requestData['DeleteFlag'] = false;
        requestData['UpdatedBy'] = this.incomingcircular.CreatedBy;
        requestData['UpdatedDateTime'] = new Date();
        requestData['CircularID'] = this.incomingcircular.CircularID;

        this.circularService.updateCircular('Circular', this.incomingcircular.CircularID, requestData).subscribe(result => {
          this.sendDraftBtnLoad = false;
          this.sendBtnLoad = false;
          if(data == 'draft') {
            this.message = "Circular Drafted Successfully";
            if (this.common.currentLang == 'ar') {
              this.message = this.arabic('circulardraftsuccess');
            }
          }else if(this.incomingcircular.Status == 16){
            this.message = "Circular Resubmitted Successfully";
            if (this.common.currentLang == 'ar') {
              this.message = this.arabic('circularresubmitsuccess');
            }
          }else{
            this.message = "Circular Sent Successfully";
            if (this.common.currentLang == 'ar') {
              this.message = this.arabic('circularsubmitsuccess');
            }
          }
            this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
            this.bsModalRef.content.message = this.message;
            this.bsModalRef.content.screenStatus = this.screenStatus;
            this.bsModalRef.content.pagename = 'Circular';

          // } else {
          //   if (this.incomingcircular.HistoryLog[0].Action == 'Clone')
          //     this.message = (this.common.currentLang == 'ar') ? this.arabic('circularsubmitted') : "Circular Sent Successfully";
          //   else
          //     this.message = (this.common.currentLang == 'ar') ? this.arabic('circularupdate') : "Circular Updated Successfully";
          //   this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
          //   this.bsModalRef.content.message = this.message;
          //   this.bsModalRef.content.pagename = 'Circular';
          //   //location.reload();
          // }
        });
      } else {
        requestData = this.prepareData();
        requestData.Action = 'Submit';
        this.circularService.saveCircular('Circular', requestData).subscribe(result => {
          this.sendDraftBtnLoad = false;
          this.sendBtnLoad = false;
          if(this.screenStatus == 'Create') {
            this.message = (this.common.currentLang == 'ar') ? this.arabic('circularsubmitsuccess') : "Circular Sent Successfully";
            this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
            this.bsModalRef.content.message = this.message;
            this.bsModalRef.content.screenStatus = this.screenStatus;
            this.bsModalRef.content.pagename = 'Circular';
          } 
          // else {
          //   this.message = (this.common.currentLang == 'ar') ? this.arabic('circularsubmitted') : "Circular Sent Successfully";
          //   this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
          //   this.bsModalRef.content.message = this.message;
          //   this.bsModalRef.content.pagename = 'Circular';
          //   //location.reload();
          // }
        });
      }
    }
    // this.router.navigate(['app/circular/circular-list']);
  }


  statusChange(status: any, dialog) {
    this.submitted = true;
    var data = this.formatPatch(status, 'Action')
    this.commonMes = status;
    this.circularService.statusChange('Circular', this.incomingcircular.CircularID, data).subscribe(data => {
      //this.message = 'Memo '+status+'d';
      this.approveBtnLoad = false;
      this.returnInfoBtnLoad = false;
      this.btnLoad = false;
      if (status == 'ReturnForInfo') {
        this.message = (this.common.currentLang == 'ar') ? this.arabic('circularreturninfo') : "Circular Returned For Info Successfully.";
      } else if (status == 'Approve') {
        this.message = (this.common.currentLang == 'ar') ? this.arabic('circularapproved') : "Circular Approved Successfully ";
      } else if (status == 'Reject') {
        this.message = (this.common.currentLang == 'ar') ? this.arabic('circularreject') : "Circular Rejected Successfully";
      } else if (status == 'Close') {
        this.message = (this.common.currentLang == 'ar') ? this.arabic('circularclosed') : "Circular Closed Successfully";
      } else {
        this.message = (this.common.currentLang == 'ar') ? this.arabic(('Circular ' + status + 'd Successfully').replace(/ +/g, "").toLowerCase()) : 'Circular ' + status + 'd Successfully';
      }
      //that.modalService.show(that.template);
      //this.modalService.show(this.template);
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'Circular';
      this.loadData(data['CircularId'], this.currentUser.id);
      //location.reload();

    });

  }

  popup(status: any) {
    this.bsModalRef = this.modalService.show(ModalComponent, this.config);
    this.bsModalRef.content.status = status;
    this.bsModalRef.content.button = 'Escalate';
    this.bsModalRef.content.modalTitle = 'Escalate';
    this.bsModalRef.content.fromScreen = 'Circular';
    this.bsModalRef.content.Comments = this.incomingcircular.Comments;
    this.bsModalRef.content.memoid = this.incomingcircular.CircularID;
    this.bsModalRef.content.onClose.subscribe(result => {
      this.btnLoad = false;
    });
  }

  // approve() {
  //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  //     console.log('approved' + data);
  //   });
  // }

  // escalate() {
  //   var data = this.formatPatch('escalate', 'Status')
  //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  //     console.log('escalated' + data);
  //   });
  // }

  // reject() {
  //   var data = this.formatPatch('reject', 'Status')
  //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  //     console.log('rejected' + data);
  //   });
  // }
  returnForInfo() {

  }
  clone(data) {
    this.submitted = true;
    var requestData = this.prepareData();
    requestData.Action = data;
    this.circularService.saveClone('Circular/Clone', this.incomingcircular.CircularID, this.currentUser.id).subscribe(data => {
      this.cloneBtnLoad = false;
      this.btnLoad = false;
      this.message = (this.common.currentLang == 'ar') ? this.arabic('circularclonesuccess') : "Circular Cloned Successfully";
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.page_url = 'app/circular/Circular-edit/' + data;
      this.bsModalRef.content.pagename = 'Circular Clone';
      //this.bsModalRef.content.pagename = 'Letter';
      //location.reload();
    });
  }
  print(template: TemplateRef<any>) {
    this.circularService.printPreview('Circular/Download', this.incomingcircular.CircularID, this.currentUser.id).subscribe(res => {
      if (res) {
        this.circularService.pdfToJson(this.incomingcircular.ReferenceNumber).subscribe((data: any) => {
          this.showPdf = true;
          this.pdfSrc = data;
          this.bsModalRef = this.modalService.show(template, { class: 'modal-xl' });
          this.common.deleteGeneratedFiles('files/delete', this.incomingcircular.ReferenceNumber + '.pdf').subscribe(result => {
            console.log(result);
          });
        });
      }
    });
  }


  downloadPrint() {
    this.circularService.printPreview('Circular/Download', this.incomingcircular.CircularID, this.currentUser.id).subscribe(res => {
      if (res) {
        this.common.previewPdf(this.incomingcircular.ReferenceNumber)
          .subscribe((data: Blob) => {
            this.downloadBtnLoad = false;
            this.btnLoad = false;
            this.printBtnLoad = false;
            var url = window.URL.createObjectURL(data);
            var a = document.createElement('a');
            document.body.appendChild(a);
            a.setAttribute('style', 'display: none');
            a.href = url;
            a.download = this.incomingcircular.ReferenceNumber + '.pdf';
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
            this.common.deleteGeneratedFiles('files/delete', this.incomingcircular.ReferenceNumber + '.pdf').subscribe(result => {
              console.log(result);
            });
          });
      }
    });
  }



  printPdf(html: ElementRef<any>) {
    // var myWindow = window.open('', '');
    // myWindow.document.write(html.innerHTML);
    // myWindow.document.close();
    // myWindow.focus();
    // myWindow.print();
    // myWindow.close();
    this.btnLoad = false;
    this.printBtnLoad = false;
    this.circularService.printPreview('Circular/Download', this.incomingcircular.CircularID, this.currentUser.id).subscribe(res => {
      if (res) {
        this.common.printPdf(this.incomingcircular.ReferenceNumber);
      }
    });

  }
  delete(id) {
    this.submitted = true;
    this.circularService.deleteCircular('Circular', this.incomingcircular.CircularID).subscribe(data => {
      this.deleteBtnLoad = false;
      this.btnLoad = false;
      this.message = (this.common.currentLang == 'ar') ? this.arabic('circulardeletedsuccess') : "Circular Deleted Successfully";
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'Circular';
    });

  }

  closePrintPop() {
    this.btnLoad = false;
    this.printBtnLoad = false;
    this.bsModalRef.hide();
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
          this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'CircularID': '' });
        }
        this.incomingcircular.Attachments = this.attachments;
      }
    });
    this.myInputVariable.nativeElement.value = "";
  }

  // close() {
  //   var data = this.formatPatch('close', 'Status')
  //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  //     console.log('closed' + data);
  //   });
  // }

  // redirect() {
  //   var data = this.formatPatch('redirect', 'Status')
  //   this.memoservice.statusChange('memo', this.id, data).subscribe(data => {
  //     console.log('redirected' + data);
  //   });
  // }

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
      "value": this.incomingcircular.Comments,
      "path": "Comments",
      "op": "replace"
    }];
    return data;
  }

  hisLog(status) {
    if (status == 'Reject') {
      return (this.common.currentLang == 'en') ? 'Rejected By' : this.arabic('circularrejectby');
    } else if (status == 'Submit') {
      return (this.common.currentLang == 'en') ? 'Submitted By' : this.arabic('circularsubmitby');
    } else if (status == 'Resubmit') {
      return (this.common.currentLang == 'en') ? 'Resubmitted By' : this.arabic('circularresubmitby');
    } else if (status == 'ReturnForInfo') {
      return (this.common.currentLang == 'en') ? 'ReturnForInfo By' : this.arabic('circularreturnforinfoby');
    } else if (status == 'Clone'){
      return (this.common.currentLang == 'en') ? 'Cloned By' : this.arabic('circularcloneby');
    } else if (status == 'Save'){
      return (this.common.currentLang == 'en') ? 'Saved By' : this.arabic('circularsaveby');
    } else if (status == 'Escalate'){
      return (this.common.currentLang == 'en') ? 'Escalated By' : this.arabic('circularescalateby');
    } else {
      return (this.common.currentLang == 'en') ? status + 'd By' : this.arabic(status + 'by');
    }
    // return this.common.historyLog(status);
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}


