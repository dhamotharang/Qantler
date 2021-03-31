import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { CalendarManagementService } from 'src/app/media/calendar-management/service/calendar-management.service';
import { ReportsService } from 'src/app/shared/service/reports.service';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';

@Component({
  selector: 'app-calendar-management-report-modal',
  templateUrl: './calendar-management-report-modal.component.html',
  styleUrls: ['./calendar-management-report-modal.component.scss']
})
export class CalendarManagementReportModalComponent implements OnInit {
  reportFilter: any = {
    ReferenceNumber: '',
    EventRequestor: '',
    EventType: '',
    EventDetails: '',
    DateFrom: '',
    DateTo: '',
    Location: '',
    UserName: '',
    Status: 'All',
    SmartSearch:''
  }
  StatusList: Array<any> = [];
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  EventTypes: Array<any> = [
    { 'label': 'All', 'value': ''},
    {'label':'Bulk','value':0},
    { 'label': 'Meeting', 'value': '1'},
    { 'label': 'Visit', 'value': '2'},
    { 'label': 'Wedding', 'value': '3'},
    { 'label': 'Interview', 'value': '4'},
    { 'label': 'Appointment', 'value': '5'}
  ];

  locationList: Array<any>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  lang:string;
  constructor(
    public bsModalRef: BsModalRef,
    public calendarManagementService: CalendarManagementService,
    public reportService:ReportsService,
    private commonService:CommonService,
    public arabic:ArabicDataService
  ) {
    this.lang = this.commonService.currentLang;
  }

  ngOnInit() {
    this.lang = this.commonService.currentLang;
    this.loadLocationList();
    this.dateValidation();
    if(this.lang !='ar'){
      this.EventTypes = [
        { 'label': 'All', 'value': ''},
        { 'label': 'Bulk','value':0},
        { 'label': 'Meeting', 'value': '1'},
        { 'label': 'Visit', 'value': '2'},
        { 'label': 'Wedding', 'value': '3'},
        { 'label': 'Interview', 'value': '4'},
        { 'label': 'Appointment', 'value': '5'}
      ];

      this.StatusList = [
        { DisplayName: 'All'},
        { DisplayName: 'Waiting for Approval' },
        { DisplayName: 'Approved' },
        { DisplayName: 'Pending for Resubmission' },
        { DisplayName: 'Rejected' },
        { DisplayName: 'Completed' }
      ];
      this.reportFilter.Status = 'All';
    }else{
      this.EventTypes = [
        { 'label': this.commonService.arabic.words['all'], 'value': ''},
        { 'label': this.commonService.arabic.words['bulk'],'value':0},
        { 'label': this.commonService.arabic.words['meeting'], 'value': '1'},
        { 'label': this.commonService.arabic.words['visit'], 'value': '2'},
        { 'label': this.commonService.arabic.words['wedding'], 'value': '3'},
        { 'label': this.commonService.arabic.words['interview'], 'value': '4'},
        { 'label': this.commonService.arabic.words['appointment'], 'value': '5'}
      ];

      this.StatusList = [
        { DisplayName: this.arabicfn('all') },
        { DisplayName: this.arabicfn('waitingforapproval') },
        { DisplayName: this.arabicfn('approved') },
        { DisplayName: this.arabicfn('pendingforresubmission') },
        { DisplayName: this.arabicfn('rejected') },
        { DisplayName: this.arabicfn('completed') }
      ];
      this.reportFilter.Status = this.arabicfn('all');
    }
    this.loadEventTypes();
  }

  dateValidation() {
    let startDate = new Date(this.reportFilter.DateFrom).getTime();
    let endDate = new Date(this.reportFilter.DateTo).getTime();
    if (startDate && endDate) {
      if (startDate > endDate) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }

  loadLocationList() {
    this.calendarManagementService.locationList(this.currentUser.id).subscribe(
      (response: any) => {
        this.locationList = response;
      }
    );
  }

  Download() {
    let ReferenceNumber = '';
    let EventType = '';
    let EventRequestor = '';
    let EventDetails = '';
    let DateFrom = '';
    let DateTo = '';
    let Status = '';
    let Location = '';

    let toSendReportOptions:any = {
      Location:'',
      ReferenceNumber: "",
      EventRequestor: "",
      EventType: null,
      DateFrom: "",
      DateTo: "",
      UserID: null,
      Status: "",
      SmartSearch: ""
    };

    if(this.reportFilter.Location){
      toSendReportOptions.Location = this.reportFilter.Location;
    }

    if(this.reportFilter.EventDetails){
      toSendReportOptions.EventDetails = this.reportFilter.EventDetails;
    }

    if(this.reportFilter.ReferenceNumber){
      toSendReportOptions.ReferenceNumber = this.reportFilter.ReferenceNumber;
    }

    if(this.reportFilter.Status && !((this.reportFilter.Status == 'All') || (this.reportFilter.Status == this.arabicfn('all')))){
      toSendReportOptions.Status = this.reportFilter.Status;
    }

    if(!this.reportFilter.Status){
      this.reportFilter.Status = 'All';
      if(this.lang != 'en'){
        this.reportFilter.Status = this.arabicfn('all');
      }
    }

    if(this.reportFilter.EventType >= 0 && this.reportFilter.EventType != null && this.reportFilter.EventType != undefined && this.reportFilter.EventType != ''){
      toSendReportOptions.EventType = this.reportFilter.EventType;
    }
    if(!this.reportFilter.EventType){
      this.reportFilter.EventType = '';
    }
    if(this.reportFilter.EventRequestor){
      toSendReportOptions.EventRequestor = this.reportFilter.EventRequestor;
    }
    if(this.reportFilter.DateFrom){
      toSendReportOptions.DateFrom = new Date(this.reportFilter.DateFrom).toJSON();
    }
    if(this.reportFilter.DateTo){
      toSendReportOptions.DateTo = new Date(this.reportFilter.DateTo).toJSON();
    }
    if(this.reportFilter.SmartSearch){
      toSendReportOptions.SmartSearch = this.reportFilter.SmartSearch;
    }
    toSendReportOptions.UserID = this.currentUser.id;
    let dateVal = new Date(), cur_date = dateVal.getDate() +'-'+(dateVal.getMonth()+1)+'-'+dateVal.getFullYear();
    this.reportService.downloadModuleReport(toSendReportOptions,'calendar').subscribe((data) => {
      var url = window.URL.createObjectURL(data);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = 'CalendarManagementReport.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
        this.bsModalRef.hide();
    });
  }

  arabicfn(word) {
    return this.commonService.arabic.words[word];
  }

  loadEventTypes(){
    this.calendarManagementService.eventTypes(this.currentUser.id).subscribe(
      (response: any) => {
        if (this.lang == 'en') {
          var toDisplayEventList:any = [{ 'label': 'All', 'value': ''},{'label':'Bulk','value':0}];          
        } else {
          var toDisplayEventList:any = [{ 'label': this.arabicfn('all'), 'value': ''},{'label': this.arabicfn('bulk'),'value':0}];          
        }
        this.EventTypes = [];
        response.forEach((eventTypeObj) => {
          toDisplayEventList.push({label:eventTypeObj.MeetingTypeName,value:eventTypeObj.EventID});
        });
        this.EventTypes = toDisplayEventList;
      }
    );
  }
}
