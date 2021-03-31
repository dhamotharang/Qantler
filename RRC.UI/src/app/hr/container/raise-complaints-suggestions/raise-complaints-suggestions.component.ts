import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { CitizenAffairService } from '../../../citizen-affair/service/citizen-affair.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { AssignModalComponent } from '../../../modal/assignmodal/assignmodal.component';
import { SuccessComponent } from '../../../modal/success-popup/success.component';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-raise-complaints-suggestions',
  templateUrl: './raise-complaints-suggestions.component.html',
  styleUrls: ['./raise-complaints-suggestions.component.scss']
})
export class RaiseComplaintsSuggestionsComponent implements OnInit {
  bsConfig: Partial<BsDatepickerConfig>;
  @ViewChild('profile_upload') profile_upload: ElementRef
  requestTypes = ['Field visit Report', 'Personal Report'];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  requestType = '';
  screenStatus = 'Create';
  approval = false;
    config = {
    backdrop: true,
    ignoreBackdropClick: true
  };
  AssigneeId=0;
  displayStatus: any;
  url: string | ArrayBuffer;
  complaint: any = {
    HRComplaintsuggestionId: 0,
   CreationDate: '',
    Source: '',
    ReferenceNumber: '',
    Type: '0',
    Subject: '',
    Details: '',
    RequestCreatedBy: '',
    MailID: '',
    PhoneNumber: '',
    Status: '',
    CreatedDateTime: new Date(),
    CreatedBy: this.currentUser.id,
    Comments: '',
    Action: '',
    ActionTaken:'',
    HistoryLog: []

  }
  message: any;
  ComplaintData: any = {};
  bsModalRef: BsModalRef;
  lang: any;
  constructor(private common: CommonService, private modalService: BsModalService, public citizenService: CitizenAffairService,
    public router: Router, route: ActivatedRoute) {
  	this.bsConfig = {
      dateInputFormat: 'DD/MM/YYYY'
    }
    route.url.subscribe(() => {
      console.log(route.snapshot.data);
      this.screenStatus = route.snapshot.data.title;
    });
    route.params.subscribe(param => {
      var id = +param.id;
      var id = +param.id;
      if (id > 0)
        this.loadData(id, this.currentUser.id);
    });
    // this.citizenService.getComplaintSuggestion('CAComplaintSuggestions', 0, 0).subscribe((data: any) => {
    //   //this.department = data.OrganizationList;
    // });
    if (this.screenStatus == 'Create') {
      this.displayStatus = 'CREATION';
      this.bottonControll();
    }
    if (this.screenStatus == 'View') {
      this.displayStatus = 'VIEW';
    }
    if (this.screenStatus == 'Edit') {
      this.displayStatus = 'EDIT';
    }
  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    if (this.lang == 'en') {
      this.common.breadscrumChange('HR', 'Raise Complaints & Suggestions', '');
    } else {
      this.common.breadscrumChange('الموارد البشرية', this.arabic('raisecomplaintsandsuggestions'), '');
    }
    this.common.topBanner(false, '', '', ''); // Its used to hide the top banner section into children page
    //this.bottonControll();
  }

   loadData(id, userid) {
     this.citizenService.getComplaintSuggestion('HRComplaintSuggestion', id, userid).subscribe((data: any) => {
      this.complaint = data;
      data.CreatedDateTime=new Date(data.CreatedDateTime);
    this.complaint.CreationDate = data.CreatedDateTime;
      if(this.complaint.AssigneeId.length){
      	this.AssigneeId =this.complaint.AssigneeId[0].AssigneeId;
        this.approval = true;
      }
      if(data.RequestCreatedBy != '0'){
        this.getSouceName(data.Source,0);
      }else{ this.complaint.Source = (this.lang == 'en')? 'Anonymous': this.arabic('anonymous'); }
      
      this.bottonControll();
    });
  }

  async getSouceName(UserID,DepID) {
     let params = [];
     this.common.getUserList(params,0).subscribe((data: any) => {
       let Users = data;
       this.complaint.Source = Users.find(x=> x.UserID == UserID).EmployeeName.toString();
     });
   }

  prepareData() {
    this.ComplaintData = {};
    if (this.complaint.RequestCreatedBy == 'Anonymous') {
      this.complaint.MailID = "";
      this.complaint.PhoneNumber = "";
    }
    this.ComplaintData = this.complaint;
    this.ComplaintData.Source=this.currentUser.UserID;
    return this.ComplaintData;
  }

  validateForm() {
    var flag = true;
    // var destination = (this.incomingcircular.DestinationOU) ? (this.incomingcircular.DestinationOU.length > 0) : false;

    if (this.complaint.Subject.trim() && this.complaint.RequestCreatedBy && this.complaint.Details.trim()) {
      flag = false;
    }
    return flag;
  }
  btnLoad= false;
  submitBtn = false;
  assignBtn = false;
  assigntomeBtn = false;
  closeBtn = false;
  submitBtnLd = false;
  assignBtnLd = false;
  assigntomeBtnLd = false;
  closeBtnLd = false;
  bottonControll() {
    if (this.screenStatus == 'Create') {
      this.submitBtn = true;
    }

    // if (this.screenStatus == 'View') {
    //   this.assignBtn = true;
    // }
    if (this.screenStatus == 'View' &&  this.currentUser.IsOrgHead && this.currentUser.OrgUnitID==9 && this.complaint.Status != 48) {
      this.closeBtn = true;
    } if(this.AssigneeId == this.currentUser.id && this.currentUser.OrgUnitID==9 && this.complaint.Status != 49 ){
       this.closeBtn = true;
    }
    if(this.screenStatus == 'View' &&  this.currentUser.IsOrgHead && !this.approval && this.currentUser.OrgUnitID==9 && this.complaint.Status != 49){
      this.assignBtn = true;
    }
    if(this.screenStatus == 'View' &&  this.currentUser.IsOrgHead && this.approval && this.currentUser.OrgUnitID==9 && this.complaint.Status != 49){
      if(this.AssigneeId!=this.currentUser.id)
      this.assignBtn = true;
    }
    if(this.screenStatus == 'View' &&  !this.currentUser.IsOrgHead && !this.approval && this.currentUser.OrgUnitID==9 && this.complaint.Status != 49){
      this.assigntomeBtn = true;
    }
    if(this.screenStatus == 'View' &&  !this.currentUser.IsOrgHead && this.approval && this.currentUser.OrgUnitID==9 && this.complaint.Status != 49){
      if(this.AssigneeId!=this.currentUser.id)
      this.assigntomeBtn = true;
    }


    if(this.complaint.Status == 49){
      this.assigntomeBtn = false;
      this.assignBtn = false;
      this.closeBtn = false;
    }
    // if (this.screenStatus == 'View' && this.incomingcircular.ApproverName == this.currentUser.id && this.incomingcircular.Status == 13) {
    //   this.approverBtn = true;
    // }

    // if (this.incomingcircular.CreatedBy == this.currentUser.id && this.incomingcircular.Status == 12) {
    //   this.deleteBtn = true;
    // }
  }

  submit() {
    var requestData = this.prepareData();
    requestData.Action = 'Submit';
    this.submitBtnLd = true;
    this.btnLoad = true;
    this.citizenService.saveComplaintSuggestion('HRComplaintSuggestion',requestData).subscribe(data => {
      this.btnLoad= false;
      if(this.common.currentLang != 'ar'){
        this.message = "Complaint/Suggestion Submitted Successfully";
      } else {
        this.message = this.arabic('complaintsuggestionsubmitted');
      }
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'HR';
    });
  }
  assign(status: any) {
    var data = this.formatPatch(status, 'Action');
    if (status == 'AssignTo'){
    	this.assignBtnLd = true;
    	this.btnLoad = true;
    }else if(status == 'AssignToMe'){
    	this.assigntomeBtnLd = true;
    	this.btnLoad = true;
    }else if(status == 'Close'){
    	this.closeBtnLd = true;
    	this.btnLoad = true;
    }
    this.citizenService.statusChange('HRComplaintSuggestion', this.complaint.HRComplaintSuggestionsID, data).subscribe(data => {
    	this.btnLoad= false;
      //this.message = 'Memo '+status+'d';
      if (status == 'AssignTo' || status=='AssignToMe') {
        if(this.common.currentLang != 'ar'){
          this.message = "Complaint/Suggestion Assigned Successfully";
        }else{ this.message = this.arabic('complaintsuggestionassigned'); }
      } else if (status == 'Close') {
        if(this.common.currentLang != 'ar'){
          this.message = "Complaint/Suggestion Closed Successfully";
        }else{ this.message = this.arabic('complaintsuggestionclosed'); }
      }
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'HR';

    });
  }
  assignpopup(status: any) {
    this.bsModalRef = this.modalService.show(AssignModalComponent, this.config);
    this.bsModalRef.content.status = status;
    this.bsModalRef.content.fromScreen = 'Complaint/Suggestion';
    this.bsModalRef.content.page = 'HR';
    this.bsModalRef.content.ActionTaken = this.complaint.ActionTaken;
    this.bsModalRef.content.HRComplaintSuggestionId = this.complaint.HRComplaintSuggestionsID;
    this.btnLoad = false;
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
      "value": this.complaint.ActionTaken,
      "path": "Comments",
      "op": "replace"
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
      } else if(this.common.currentLang == 'ar'){
        let arabicStatusStr = '';
        if (sts == 'reject' || sts == 'redirect') {
          arabicStatusStr = sts+'edby';
        } else if (sts == 'assignto' || sts == 'assigntome') {
          arabicStatusStr = 'complaintassignedby';
        } else if(sts == 'submit' || sts == 'resubmit'){
          arabicStatusStr = 'hrsubmittedby';
        } else if(sts == 'close'){
          arabicStatusStr = 'complaintclosedby';
        } else {
          arabicStatusStr = sts+'dby';
        }
        return this.common.arabic.words[arabicStatusStr];
      }
    }

    arabic(word) {
      return this.common.arabic.words[word];
    }

}

