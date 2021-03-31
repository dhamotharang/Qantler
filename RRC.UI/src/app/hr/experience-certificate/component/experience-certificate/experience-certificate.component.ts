import { Component, OnInit, Input, ViewChild, TemplateRef, Renderer2, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { DatePipe, DOCUMENT } from '@angular/common'
import { AssignModalComponent } from 'src/app/shared/modal/assign-modal/assign-modal.component';
import { ExperienceFormModel } from '../../model/experience-form.model';
import { SalarycertificateService } from 'src/app/hr/salary-certificate/service/salarycertificate.service';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';

@Component({
  selector: 'app-experience-certificate',
  templateUrl: './experience-certificate.component.html',
  styleUrls: ['./experience-certificate.component.scss']
})

export class ExperienceCertificateComponent implements OnInit {
  historyData:any = [];
  Department = [];
  certificatedata={
    SourceOU: "",
    SourceName: "",
    Attention: '',
    To: "",
    Reason: "",
    CreatedBy: 0,
    CreatedDateTime: new Date(),
    Action: "",
    Comments : "",
    CertificateType: "Experience",
    ReferenceNumber: ""
  }

  @Input() screenStatus: String;
  lang: String;
  SalaryFormModel: ExperienceFormModel = new ExperienceFormModel();
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  message: string;
  bsModalRef: BsModalRef;
  certificateId: number;
  certificateResult: any;
  commonMes: any;
  @ViewChild('template') template : TemplateRef<any>;
  assignToMeBtn: boolean;
  assignBtn: boolean;
  submitBtn: boolean;
  closeBtnShow: boolean;
  IsOrgHead: any;
  isSubmitted = true;
  CreatedDateTime: string;
  popupMsg: string;

  constructor(public arabic: ArabicDataService, private renderer: Renderer2, @Inject(DOCUMENT) private document: Document, public router: Router, private route: ActivatedRoute, public salService: SalarycertificateService, public modalService: BsModalService, private datePipe: DatePipe, private common: CommonService) {
  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    this.route.params.subscribe(param => {
      this.certificateId = +param.id;
      if(this.certificateId > 0 && this.screenStatus == 'View') {
        this.common.breadscrumChange('HR', 'Experience Certificate View', '');
        if (this.lang == 'ar') {
          this.common.breadscrumChange(this.arabicfn('humanresource'), this.arabicfn('experiencecertificateview'), '');
        }
        this.loadData(this.certificateId);
      } else {
        if (this.lang == 'ar') {
          this.common.breadscrumChange(this.arabicfn('humanresource'), this.arabicfn('experiencecertificatecreation'), '');
        } else {
          this.common.breadscrumChange('HR', 'Experience Certificate Creation', '');
        }
      }
    });
  }

  async loadData(certificateId) {
    await this.salService.getCertificate(certificateId, this.currentUser.id).subscribe((data: any) => {
      this.certificateResult = data;
      this.Department = data.OrganizationList;
      if (this.screenStatus == 'View' || this.screenStatus == 'Edit') {
        var date = this.certificateResult.CreatedDateTime;
        this.certificatedata.CreatedDateTime = new Date(date);
        this.certificatedata.ReferenceNumber = data.ReferenceNumber;
        this.getSouceName(data.SourceName,data.SourceOU); 
        this.buttonControl()
        this.prepareData();
        this.transformDate();
      } else {
        // this.initPage();
        // this.bottonControll();
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
       this.certificatedata.SourceOU= this.Department.find(x=> x.OrganizationID == DepID).OrganizationUnits;  
       this.certificatedata.SourceName = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
     });
    
   }

  transformDate() {
    var currentDate = this.datePipe.transform(this.certificatedata.CreatedDateTime, 'dd/MM/yyyy');
    this.CreatedDateTime = currentDate;
  }

  hisLog(status){
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
        arabicStatusStr = 'experienceassignedby';
      } else if(sts == 'submit'){
        arabicStatusStr = 'experiencesubmittedby';
      } else if(sts == 'resubmit'){
        arabicStatusStr = sts+'tedby';
      } else if(sts == 'close'){
        arabicStatusStr = 'experienceclosedby';
      } else {
        arabicStatusStr = sts+'dby';
      }
      return this.common.arabic.words[arabicStatusStr];
    }
  }

  buttonControl() {
    let AssigneeId = '';
    if(this.certificateResult.AssigneeId.length > 0) {
      AssigneeId = this.certificateResult.AssigneeId[0].AssigneeId
    }
    this.IsOrgHead = this.currentUser.IsOrgHead;
    if(this.currentUser.OrgUnitID == 9) {
      if(this.IsOrgHead) {
        if(this.certificateResult.Status == 34 && this.certificateResult.AssigneeId.length == 0) {
          this.closeBtnShow = false;
          this.assignToMeBtn = false;
          this.assignBtn = true;
        }
        if(this.certificateResult.Status == 34 && this.certificateResult.AssigneeId.length > 0) {
          this.closeBtnShow = false;
          this.assignToMeBtn = false;
          this.assignBtn = true;
        }
        if(this.certificateResult.Status == 34 && this.certificateResult.AssigneeId.length > 0 && AssigneeId === this.currentUser.id){
          this.closeBtnShow = true;
          this.assignToMeBtn = false;
          this.assignBtn = false;
        }
      } else {
        if(this.certificateResult.Status == 34 && this.certificateResult.AssigneeId.length == 0){
          this.assignToMeBtn = true;
          this.assignBtn = true;
        }
        if(this.certificateResult.Status == 34 && this.certificateResult.AssigneeId.length > 0 && AssigneeId === this.currentUser.id){
          this.closeBtnShow = true;
          this.assignToMeBtn = false;
          this.assignBtn = false;
        }
        if(this.certificateResult.Status == 34 && this.certificateResult.AssigneeId.length > 0 && AssigneeId !== this.currentUser.id){
          this.closeBtnShow = false;
          this.assignToMeBtn = true;
          this.assignBtn = false;
        }
      }
      if(this.certificateResult.Status == 35){
        this.closeBtnShow = false;
        this.assignToMeBtn = false;
        this.assignBtn = false;
      }
    }else {
      this.closeBtnShow = false;
      this.assignToMeBtn = false;
      this.assignBtn = false;
    }
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
      "value": 'Approved',
      "path": "Comments",
      "op": "replace"
    }];
    return data;
  }

  statusChange(status: any, dialog) {
    var data = this.formatPatch(status, 'Action')
    this.commonMes = status;
    this.salService.statusChange("Certificate", this.certificateId, data).subscribe(data => {
      if(status == 'AssignToMe'){
        this.message = "Experience Certificate Assigned Successfully";
        if (this.lang == 'ar') {
          this.message = this.arabicfn('experiencecertificateassignedmsg');
        }
      }else if(status == 'Close'){
        this.message = "Experience Certificate Closed Successfully";
        if (this.lang == 'ar') {
          this.message = this.arabicfn('experiencecertificateclosedmsg');
        }
      }else{
        this.message = 'Experience Certificate Assigned Successfully';
        if (this.lang == 'ar') {
          this.message = this.arabicfn('experiencecertificateassignedmsg');
        }
      }
      this.modalService.show(this.template);
      this.loadData(this.certificateId);
    });

  }

  popup(status: any) {
    let modalTitleString;
    if (this.lang == 'ar') {
      this.popupMsg = this.arabicfn('experiencecertificateassignedmsg');
      modalTitleString = this.arabicfn('salaryassignto');
    } else {
      this.popupMsg = 'Experience Certificate Assigned Successfully';
      modalTitleString = "Assign To";
    }
    const initialState = {
      id: this.certificateId,
      ApiString: "Certificate",
      message: this.popupMsg,
      ApiTitleString: modalTitleString,
      redirectUrl: '/app/hr/dashboard'
    };
    const modalConfig = {
      backdrop: true,
      ignoreBackdropClick: true,
      };
    this.modalService.show(AssignModalComponent, Object.assign({}, modalConfig, { initialState }))
  }

  closemodal(){
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
    this.router.navigate(['app/hr/dashboard']);
  }

  validateForm() {
    var flag = true;
    let val = this.certificatedata.Reason.trim();
    if (this.certificatedata.Attention && val) {
      flag = false;
      if(this.certificatedata.Attention == "1") {
        let toVal = this.certificatedata.To.trim();
        if(!toVal) {
          flag = true;
        }else {
          flag = false;
        }
      }
    }
    return flag;
  }

  prepareData() {
    if (this.screenStatus == 'View' || this.screenStatus == 'Edit') {
      this.certificatedata = this.certificateResult;
    }
    this.SalaryFormModel.CreatedDateTime = this.certificatedata.CreatedDateTime;
    this.SalaryFormModel.CertificateType = this.certificatedata.CertificateType;
    this.SalaryFormModel.Attention = this.certificatedata.Attention;
    this.SalaryFormModel.To = this.certificatedata.To;
    this.SalaryFormModel.Reason = this.certificatedata.Reason;
    this.SalaryFormModel.CreatedBy = this.currentUser.id;
    this.SalaryFormModel.SourceOU = this.currentUser.DepartmentID;
    this.SalaryFormModel.SourceName = this.currentUser.UserID;
    this.SalaryFormModel.Comments = '';
    return this.SalaryFormModel;
  }

  saveSalCer() {
    this.isSubmitted = false;
    var requestData = this.prepareData();
    requestData.Action = 'Submit';
    this.salService.saveSalaryCert(requestData).subscribe((data:any) => {
      if(data.CertificateId) {
        this.message = "Experience Certificate Submitted Successfully";
        if (this.lang == 'ar') {
          this.message = this.arabicfn('experiencecertificatecreatedmsg');
        }
        this.bsModalRef = this.modalService.show(SuccessComponent);
        this.bsModalRef.content.message = this.message;
        let newSubscriber = this.modalService.onHide.subscribe(r=>{
          newSubscriber.unsubscribe();
          this.router.navigate(['app/hr/dashboard']);
        });
      }

    });
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }
}

