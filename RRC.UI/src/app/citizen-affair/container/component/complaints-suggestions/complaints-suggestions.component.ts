import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { CitizenAffairService } from '../../../service/citizen-affair.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { AssignModalComponent } from '../../../../modal/assignmodal/assignmodal.component';
import { SuccessComponent } from '../../../../modal/success-popup/success.component';
import { Router, ActivatedRoute } from '@angular/router';
//yln3tngw2lhxbwam47vkghortp4y4qny474ogsy5so37qeekuvuq
@Component({
  selector: 'app-complaints-suggestions',
  templateUrl: './complaints-suggestions.component.html',
  styleUrls: ['./complaints-suggestions.component.scss']
})
export class ComplaintSuggestionComponent implements OnInit {
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
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
  Ismemolink: boolean = true;
  AssigneeId = 0;
  displayStatus: any;
  url: string | ArrayBuffer;
  complaint: any = {
    CAComplaintsuggestionId: 0,
    CreationDate: '',
    Source: '',
    ReferenceNumber: '',
    Type: '0',
    RequestCreatedBy: '',
    Subject: '',
    Details: '',
    ReporterName: '',
    MailID: '',
    PhoneNumber: '',
    Status: '',
    CreatedDateTime: new Date(),
    CreatedBy: this.currentUser.id,
    Comments: '',
    Action: '',
    ActionTaken: '',
    HistoryLog: []

  }
  RequestReportedBy: any;
  creatorList: any;
  createdByDisplayName: any;
  message: any;
  ComplaintData: any = {};
  bsModalRef: BsModalRef;
  emailErr: string;
  isCurrentUnit: any;
  constructor(private common: CommonService, private modalService: BsModalService, public citizenService: CitizenAffairService,
    public router: Router, route: ActivatedRoute) {
    // this.bsConfig = {
    //   dateInputFormat: 'DD/MM/YYYY'
    // }
    this.isCurrentUnit = this.citizenService.unitId.find(res => res == this.currentUser.OrgUnitID);

    route.url.subscribe(() => {
      console.log(route.snapshot.data);
      this.screenStatus = route.snapshot.data.title;
    });
    route.params.subscribe(param => {
      var id = +param.id;
      var id = +param.id;
      if (id > 0){
        this.loadData(id, this.currentUser.id);
      }
    });
    // this.citizenService.getComplaintSuggestion('CAComplaintSuggestions', 0, 0).subscribe((data: any) => {
    //   //this.department = data.OrganizationList;
    // });
    if (this.screenStatus == 'Create') {
      this.displayStatus = (this.common.language == 'English') ? 'CREATION' : this.arabic('create');
      this.bottonControll();
    }
    if (this.screenStatus == 'View') {
      this.displayStatus = (this.common.language == 'English') ? 'VIEW' : this.arabic('view');
    }
    if (this.screenStatus == 'Edit') {
      this.displayStatus = (this.common.language == 'English') ? 'EDIT' : this.arabic('edit');
    }
  }

  ngOnInit() {
    this.getCreatorList();
    this.common.topBanner(false, '', '', '');
    if (this.screenStatus == 'Create'){
      if (this.common.language == 'English') {
        this.common.breadscrumChange('Citizen Affair', 'Complaints/Suggestions Creation', '');
        this.emailErr = 'Please Enter Valid Email ID';
      } else {
        this.common.breadscrumChange(this.arabic('citizenaffair'), this.arabic('complaintssuggestionscreation'), '');
        this.emailErr = this.arabic('emailvalidationerror');
      }
    }else if(this.screenStatus == 'View'){
      if (this.common.language == 'English') {
        this.common.breadscrumChange('Citizen Affair', 'Complaints/Suggestions View', '');
        this.emailErr = 'Please Enter Valid Email ID';
      } else {
        this.common.breadscrumChange(this.arabic('citizenaffair'), this.arabic('complaintssuggestionsview'), '');
        this.emailErr = this.arabic('emailvalidationerror');
      }
    }
  }

  getCreatorList(){
    let params = [{
      'OrganizationID': '',
      'OrganizationUnits': 'string'
    }];
    this.common.getUserList(params, 0).subscribe((data: any) => {
      this.creatorList = data;
      if(this.complaint.CreatedBy != 0){
        let creator = this.creatorList.find(user => user.UserID == this.complaint.CreatedBy);
        this.createdByDisplayName = creator.EmployeeName;
      }else{
        this.createdByDisplayName = (this.common.language == 'English') ? 'WRD website': this.arabic('wrdwebsite');
      }
    });
  }

  loadData(id, userid) {
    this.citizenService.getComplaintSuggestion('CAComplaintSuggestion', id, userid).subscribe((data: any) => {
      this.complaint = data;
      data.CreatedDateTime = new Date(data.CreatedDateTime);
      this.complaint.CreationDate = data.CreatedDateTime;
      if(data.CreatedBy != 0){
        let creator = this.creatorList.find(user => user.UserID == data.CreatedBy);
        this.createdByDisplayName = creator.EmployeeName;
      }else{
        this.createdByDisplayName = (this.common.language == 'English') ? 'WRD website': this.arabic('wrdwebsite');
      }
      
      if (this.complaint.AssigneeId.length) {
        this.AssigneeId = this.complaint.AssigneeId[0].AssigneeId;
        this.approval = true;
        if (this.complaint.AssigneeId[0].AssigneeId != userid) {
          this.approval = false;
        }
      }

      this.bottonControll();
    });
  }

  radioChange(event){
    this.Ismemolink = true;
    this.complaint.Source = '';
    this.complaint.MailID = '';
    this.complaint.PhoneNumber = '';
  }

  prepareData() {
    this.ComplaintData = {};
    if (this.complaint.RequestCreatedBy == '1') {
      this.complaint.MailID = "";
      this.complaint.PhoneNumber = "";
    }
    this.ComplaintData = this.complaint;

    return this.ComplaintData;
  }

  validateForm() {
    var flag = true;
      //myself = (this.common.language == 'English') ? "Myself" : this.arabic('myself');

    if (this.complaint.RequestCreatedBy == 0) {
        let EMAIL_REGEXP = /^[a-z0-9!#$%&'*+\/=?^_`{|}~.-]+@[a-z0-9]([a-z0-9-]*[a-z0-9])?(\.[a-z0-9]([a-z0-9-]*[a-z0-9])?)*$/i;
        if (this.Ismemolink && this.complaint.Source.trim()
          && this.complaint.Subject.trim() && this.complaint.RequestCreatedBy && !this.submitBtnLd) {
          //return { "Please provide a valid email": true };
          flag = false;
        }
      
    } else {
      if (this.complaint.Subject.trim() && this.complaint.RequestCreatedBy && !this.submitBtnLd) {
        flag = false;
      }
    }
    return flag;
  }
  btnLoad = false;
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
    if (this.AssigneeId == this.currentUser.id && this.currentUser.IsOrgHead && this.isCurrentUnit && this.complaint.Status != 52) {
      this.closeBtn = true;
    }
    if (this.AssigneeId == this.currentUser.id && this.isCurrentUnit && this.complaint.Status != 52) {
      this.closeBtn = true;
    } if (this.screenStatus == 'View' && this.currentUser.IsOrgHead && !this.approval && this.isCurrentUnit && this.complaint.Status != 52) {
      this.assignBtn = true;
    }
    if (this.screenStatus == 'View' && !this.currentUser.IsOrgHead && !this.approval && this.isCurrentUnit && this.complaint.Status != 52) {
      this.assigntomeBtn = true;
    }
    if (this.complaint.Status == 52) {
      this.assigntomeBtn = false;
      this.assignBtn = false;
      this.closeBtn = false;
    }
  }
  submit() {
    var requestData = this.prepareData();
    requestData.Action = 'Submit';
    this.submitBtnLd = true;
    this.btnLoad = true;
    this.citizenService.saveComplaintSuggestion('CAComplaintSuggestion', requestData).subscribe(data => {
      console.log(data);
      this.btnLoad = false;
      this.message = (this.common.language == 'English') ? "Complaint/Suggestion Submitted Successfully" : this.arabic('complaintsuggestionsubmitted');
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'Citizen Affair';
    });
  }
  assign(status: any) {
    var data = this.formatPatch(status, 'Action');
    if (status == 'AssignTo') {
      this.assignBtnLd = true;
      this.btnLoad = true;
    } else if (status == 'AssignToMe') {
      this.assigntomeBtnLd = true;
      this.btnLoad = true;
    } else if (status == 'Close') {
      this.closeBtnLd = true;
      this.btnLoad = true;
    }
    this.citizenService.statusChange('CAComplaintSuggestion', this.complaint.CAComplaintSuggestionsID, data).subscribe(data => {
      this.btnLoad = false;
      //this.message = 'Memo '+status+'d';
      if (status == 'AssignTo' || status == 'AssignToMe') {
        this.message = (this.common.language == 'English') ? "Complaint/Suggestion Assigned Successfully" : this.arabic('complaintsuggestionassigned');
      } else if (status == 'Close') {
        this.message = (this.common.language == 'English') ? "Complaint/Suggestion Closed Successfully" : this.arabic('complaintsuggestionclosed');
      }
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'Citizen Affair';

    });
  }
  assignpopup(status: any) {
    this.bsModalRef = this.modalService.show(AssignModalComponent, this.config);
    this.bsModalRef.content.status = status;
    this.bsModalRef.content.fromScreen = 'Complaint/Suggestion';
    this.bsModalRef.content.page = 'CitizenAffair';
    this.bsModalRef.content.ActionTaken = this.complaint.ActionTaken;
    this.bsModalRef.content.CAComplaintSuggestionId = this.complaint.CAComplaintSuggestionsID;
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
    return this.common.historyLog(status);
  }
  arabic(word) {
    return this.common.arabic.words[word];
  }
  checkemail(value) {

    let re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    let status = re.test(String(value).toLowerCase());
    this.Ismemolink = status;
    if (value == "") {
      this.Ismemolink = true;
    }
  }


}

