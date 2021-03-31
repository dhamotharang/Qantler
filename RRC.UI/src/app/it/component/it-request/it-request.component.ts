import { ArabicDataService } from 'src/app/arabic-data.service';
import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { IT } from '../../model/it.model';
import { ItService } from '../../service/it.service';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { CommonService } from 'src/app/common.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { HttpEventType } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';

@Component({
  selector: 'app-it-request-form',
  templateUrl: './it-request.component.html',
  styleUrls: ['./it-request.component.scss']
})
export class ItRequestComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  @Input() mode:string;
  colorTheme = 'theme-green';
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  id:any;
  editMode:boolean = true;
  isApiLoading: boolean;
  refId:any;
  status:any;
  date:Date;
  sourceOU:any;
  sourceName:any;
  subject:any;
  departmentList = [];
  RequestorDepartment:any;
  RequestorName: any;
  requestorList = [];
  reqName:any;
  RequestType:any;
  RequestDetails:any;
  Priority:any;
  message:any;
  bsModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig> = {
     dateInputFormat:'DD/MM/YYYY'
  };
  uploadProcess: boolean;
  uploadPercentage: number;
  attachments:any = [];
  lang: string;
  @ViewChild('variable') myInputVariable: ElementRef;
  currDate = null;
  it: IT = new IT();
  inProgress: boolean = false;
  isItDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 11;
  isItDepartmentTeamUserID = this.currentUser.OrgUnitID == 11 && !this.currentUser.IsOrgHead;
  downloadUrl;
  historyLogs = [];

  constructor(
    private route: ActivatedRoute,
    public modalService: BsModalService,
    private common: CommonService,
    public router: Router,
    private itService: ItService,
    private utilsService: UtilsService,
    public arabic: ArabicDataService,
    public endpointService: EndPointService
  ) {
    this.lang = this.common.currentLang;
    
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get("id");
    //this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
    if (this.id) {
      this.editMode = false;
      this.loadRequest();
    } else {
      this.editMode = true;
      this.RequestorDepartment = this.currentUser.DepartmentID;
      this.RequestorName = this.currentUser.id;
      this.getApproverUserList(+this.RequestorDepartment);
      this.isItDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 11;
      this.isItDepartmentTeamUserID = this.currentUser.OrgUnitID == 11 && !this.currentUser.IsOrgHead;
      this.downloadUrl = this.endpointService.fileDownloadUrl;
      this.getApproverDepartmentList(+this.RequestorDepartment);
    }

    if(this.mode === 'create') {
      if (this.lang === 'en') {
        this.common.breadscrumChange('IT', 'Create IT Request', '');
      } else {
        this.common.breadscrumChange(this.arabic.words.informationtechnology, this.arabic.words.itcreaterequest, '');
      }
      this.common.topBanner(false, '', '', '');
    } else if(this.mode === 'view') {
      if (this.lang === 'en') {
        this.common.breadscrumChange('IT', 'Request View', '');
        this.common.topBanner(true, 'IT Dashboard', '+ CREATE REQUEST', '/en/app/it/it-request-create');
      } else {
        this.common.breadscrumChange(this.arabic.words.informationtechnology, this.arabic.words.itrequestview, '');
        this.common.topBanner(true, this.arabic.words['informationtechnology'], '+ '+this.arabic.words['createrequest'], '/ar/app/it/it-request-create');
      }
    }
    
    this.isApiLoading = true;
    
  }

  onChangeRequstorDepartment() {
    let orgUnitID = 0;
    this.departmentList.forEach((cardObj)=>{
      if(cardObj.OrganizationID == this.RequestorDepartment){
        orgUnitID = cardObj.OrganizationID;
      }
    });
    this.getApproverUserList(+orgUnitID,true);
  }

  async getApproverDepartmentList(id: Number) {
    this.itService.getDepartments(0, this.currentUser.id).subscribe((data: any) => {
      this.departmentList = data.OrganizationList;
    });
  }

  async getApproverUserList(orgUnitID: any, isDepartmentChanged?:boolean) {
    let params = [{
      'OrganizationID':orgUnitID,
      'OrganizationUnits': 'string'
    }];
    if(isDepartmentChanged){
      this.RequestorName = '';
    }
    this.common.getUserList(params,0).subscribe((data: any) => {
      this.requestorList = data;
    });
  }

  validate() {
    this.isApiLoading = false;
    if (this.utilsService.isEmptyString(this.subject)
    || this.utilsService.isEmptyString(this.RequestType)
    || this.utilsService.isEmptyString(this.RequestDetails)
    || this.utilsService.isEmptyString(this.Priority)
    ) {
      this.isApiLoading = true;
    }
    return this.isApiLoading;
  }

  onSubmit() {
    this.inProgress = true;
    this.it.Date = new Date();
    this.it.SourceOU = this.currentUser.DepartmentID;
    this.it.SourceName = this.currentUser.UserID;
    this.it.RequestorName = this.RequestorName == "" ? this.currentUser.id : this.RequestorName;
    this.it.RequestorDepartment = !this.RequestorDepartment ? this.currentUser.departmentID : this.RequestorDepartment;
    this.it.Subject = this.subject;
    this.it.RequestType = this.RequestType;
    this.it.Status = "New";
    this.it.RequestDetails = this.RequestDetails;
    this.it.Priority = this.Priority == "high" ? 1 : 0;
    this.it.CreatedBy = this.currentUser.id;
    this.it.CreatedDateTime = new Date();
    this.it.Action = 'Submit';
    this.itService.create(this.it)
      .subscribe((response: any) => {
        if (response.ITSupportID) {
          this.message = "IT-Support Request Submitted Successfully";
          if(this.lang != 'en'){
            this.message = this.arabic.words.itsupportreqsubmitmsg;
          }
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.router.navigate(['/'+this.lang+'/app/it/home']);
          });
        }
        this.inProgress = false;
      });
  }

  loadRequest() {
    this.itService.getById(this.id, this.currentUser.id)
      .subscribe((data:any) => {
        this.date = new Date(data.CreatedDateTime);
        this.getSouceName(data.SourceName,data.SourceOU);
        var tempSourceOU = data.OrganizationList.find(x=> x.OrganizationID==data.SourceOU);
        this.sourceOU = tempSourceOU.OrganizationUnits;
        
       // this.sourceName = data.SourceName;
        this.refId = data.ReferenceNumber;
        this.RequestorDepartment = data.RequestorDepartment ? parseInt(data.RequestorDepartment) : null;
        if(this.RequestorDepartment){
          this.getApproverDepartmentList(+this.RequestorDepartment);
          this.getApproverUserList(this.RequestorDepartment);
        }
        if(this.id){
          this.RequestorName = parseInt(data.RequestorName);
          
        }
        this.subject = data.Subject;
        this.RequestType = data.RequestType;
        this.RequestDetails = data.RequestDetails;
        if(data.Status == 94){
          this.status = "Under Process";
          if(this.lang != 'en'){
            this.status = this.arabic.words.underprocess;
          }
        }
        if(data.Status == 95){
          this.status = "Closed";
          if(this.lang != 'en'){
            this.status = this.arabic.words.closed;
          }
        }
        if(data.Status == 202){
          this.status = "New";
          if(this.lang != 'en'){
            this.status = this.arabic.words.new;
          }
        }
        this.attachments = data.Attachments;
        if(data.Priority == 1 || data.Priority == "True"){
          this.Priority = 'high';
        }else{
          this.Priority = 'low';
        }
        // this.Priority = data.Priority == 1 || "True" ? "high" : "low";
        this.historyLogs = data.HistoryLog;
        this.syncDate();
      });
  }

  async getSouceName(UserID,DepID) {
     let params = [{
       "OrganizationID": DepID,
       "OrganizationUnits": "string"
     }];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;
       this.sourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
     });
    
   }

  syncDate(){
    this.itService.syncDate().subscribe((data:any) => {
      this.currDate = data ? new Date(data): '';
    });
  }

  handleFileUpload(event) {
    var files = event.target.files;
    if (files.length > 0) {
      this.uploadProcess = true;
      this.itService.uploadAttachment(files).subscribe((event: any) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadPercentage = Math.round(event.loaded / event.total) * 100;
        } else if (event.type === HttpEventType.Response) {
          this.uploadProcess = false;
          this.uploadPercentage = 0;
          for (var i = 0; i < event.body.FileName.length; i++) {
            this.attachments.push({ 'AttachmentGuid': event.body.Guid, 'AttachmentsName': event.body.FileName[i], 'BabyAdditionID': '' });
          }
          this.it.Attachments = this.attachments;
          this.fileInput.nativeElement.value = '';
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

  hisLog(status:string) {
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
}
