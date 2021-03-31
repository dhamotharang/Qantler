import { Component, OnInit, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { CommonService } from '../../../../common.service';
import { data } from './data';
import { Router } from '@angular/router';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CitizenAffairService } from '../../../service/citizen-affair.service';
import { DatePipe } from '@angular/common';
import { CitizenReportModalComponent } from 'src/app/modal/citizen-report-modal/citizen-report-modal.component';
import { UtilsService } from 'src/app/shared/service/utils.service';

@Component({
  selector: 'app-citizen-affair-list',
  templateUrl: './citizen-affair-list.component.html',
  styleUrls: ['./citizen-affair-list.component.scss']
})
export class CitizenAffairListComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('pieChart') pieChart: ElementRef;
  @ViewChild('barChart') barChart: ElementRef;
  public rows: Array<any> = [];
  public columns: Array<any> = [];
  searchFilter = false;
  public cardDetails = [
    {
      'image': 'assets/citizen-affair/tabs.png',
      'count': 0,
      'name': (this.common.language == 'English') ? 'New' : this.arabic('new'),
      'progress': 50,
      'type': 1,
      'show': true
    },
    {
      'image': 'assets/citizen-affair/close.png',
      'count': 0,
      'name': (this.common.language == 'English') ? 'In Progress' : this.arabic('inprogresscard'),
      'progress': 50,
      'type': 5,
      'show': true
    },
    {
      'image': 'assets/citizen-affair/information.png',
      'count': 0,
      'name': (this.common.language == 'English') ? 'Need more Info' : this.arabic('needmoreinfo'),
      'progress': 50,
      'type': 2,
      'show': true
    },
    {
      'image': 'assets/citizen-affair/close.png',
      'count': 0,
      'name': (this.common.language == 'English') ? 'Closed/Rejected' : this.arabic('closedrejectedcard'),
      'progress': 50,
      'type': 3,
      'show': true
    },
    {
      'image': 'assets/citizen-affair/close.png',
      'count': 0,
      'name': (this.common.language == 'English') ? 'My Pending Request' : this.arabic('mypendingrequests'),
      'progress': 50,
      'type': 4,
      'show': true
    }
  ];
  page = 1;
  maxSize = 10;
  type = 1;
  userList: any = [];
  priorityList: string[];
  IsDocumentLoad: boolean = false;
  IsPlaceLoc: boolean = true;
  masterData = new data;
  status = this.masterData.data.status;
  taskList = this.masterData.data.Collection;
  count = this.masterData.data.Count;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat:'DD/MM/YYYY'
  };
  currentUser = JSON.parse(localStorage.getItem('User'));
  filter: any = {
    Status: (this.common.language == 'English') ? 'All' : this.arabic('all'),
    PhoneNumber: '',
    ReqDateFrom: new Date,
    ReqDateTo: new Date,
    ReferenceNumber: '',
    RequestType: (this.common.language == 'English') ? 'All' : this.arabic('all'),
    PersonalLocationName: '',
    SmartSearch: '',
    sourcename: (this.common.language == 'English') ? 'All' : this.arabic('all')
  };
  progress = false;
  requestList = ['All', 'FieldVisit', 'Personal Report', 'Complaints/Suggestion'];
  ReqDateFrom: string;
  ReqDateTo: string;
  bsModalRef: BsModalRef;
  isCurrentUnit: any = false;
  tableMessages: { emptyMessage: any; };

  // unitID ------->
  // unitId = [5,6,7,8];
  // --------->

  validateStartEndDate: any = {
    isValid: true,
    msg: ''
  };
  constructor(private common: CommonService, public util: UtilsService, private modalService: BsModalService, public service: CitizenAffairService, public router: Router, public datePipe: DatePipe) {
    this.isCurrentUnit = this.service.unitId.find(res => res == this.currentUser.OrgUnitID);
    var param = [{
      "OrganizationID": 2,
      "OrganizationUnits": "string"
    }];
    this.getUserList(param);
    this.getListCount();
    this.filter.sourcename = (this.common.language == 'English') ? 'All' : this.arabic('all');
    this.priorityList = this.common.priorityList;
    this.common.breadscrumChange('Citizen Affair', 'List Page', '');
    if (this.isCurrentUnit)
      this.common.topBanner(true, 'Citizen Affair List', '+ CREATE REQUEST', 'citizen-affair-create');
    else
      this.common.topBanner(true, 'Citizen Affair List', '', 'citizen-affair-create');
    // this.bsConfig = {
    //   dateInputFormat: 'DD/MM/YYYY'
    // }
  }

  ngOnInit() {
    this.tableMessages = {
      emptyMessage: (this.common.language == 'English') ? 'No data to display' : this.arabic('nodatatodisplay')
    };
    var refid = (this.common.language == 'English') ? 'Ref ID' : this.arabic('refid'),
      status = (this.common.language == 'English') ? 'Status' : this.arabic('status'),
      assignto = (this.common.language == 'English') ? 'Assign To' : this.arabic('assignedto'),
      requesttype = (this.common.language == 'English') ? 'Request Type' : this.arabic('requesttype'),
      requestdate = (this.common.language == 'English') ? 'Request Date' : this.arabic('requestdate'),
      sourcefield = (this.common.language == 'English') ? 'Reporter' : this.arabic('reporter'),
      createdby = (this.common.language == 'English') ? 'Created By' : this.arabic('createdby'),
      phonenumber = (this.common.language == 'English') ? 'Phone Number' : this.arabic('phonenumber'),
      action = (this.common.language == 'English') ? 'Action' : this.arabic('action'),
      personallocation = (this.common.language == 'English') ? 'Personal/ Location Name' : this.arabic('personal/locationname');
    if (this.isCurrentUnit) {
      this.IsDocumentLoad = true;
      this.columns = [
        { name: refid, prop: 'referenceNumber' },
        { name: status, prop: 'status' },
        { name: assignto, prop: 'assignedTo' },
        { name: requesttype, prop: 'requestType', width: 200 },
        { name: requestdate, prop: 'requestDate' },
        { name: personallocation, prop: 'personalName' },
        { name: phonenumber, prop: 'phoneNumber' },
        { name: action, prop: '', cellTemplate: this.actionTemplate },
      ];
    }
    else {
      this.columns = [
        { name: refid, prop: 'referenceNumber' },
        { name: status, prop: 'status' },
        { name: requesttype, prop: 'requestType', width: 200 },
        { name: requestdate, prop: 'requestDate' },
        { name: personallocation, prop: 'personalName' },
        { name: phonenumber, prop: 'phoneNumber' },
        { name: action, prop: '', cellTemplate: this.actionTemplate },
      ];
    }
    if (this.common.language != 'English') {
      this.common.breadscrumChange(this.arabic('citizenaffair'), this.arabic('listpage'), '');
      if (this.isCurrentUnit)
        this.common.topBanner(true, this.arabic('dashboard'), '+ ' + this.arabic('createrequest'), 'citizen-affair-create');
      else
        this.common.topBanner(true, this.arabic('dashboard'), '', 'citizen-affair-create');
      this.requestList = [this.arabic('all'), this.arabic('fieldvisit'), this.arabic('personal'), this.arabic('complaint')];
    }
  }

  viewData(type, value) {
    if (this.common.language == 'English') {
      if (type == 'edit') {
        if (value.requestType == "Complaints/Suggestion")
          this.router.navigate(['en/app/citizen-affair/complaint-suggestion-edit/' + value.citizenAffairID]);
        else
          this.router.navigate(['en/app/citizen-affair/citizen-affair-edit/' + value.citizenAffairID]);
      } else {
        if (value.requestType == "Complaints/Suggestion")
          this.router.navigate(['en/app/citizen-affair/complaint-suggestion-view/' + value.citizenAffairID]);
        else
          this.router.navigate(['en/app/citizen-affair/citizen-affair-view/' + value.citizenAffairID]);
      }
    } else {
      if (type == 'edit') {
        if (value.requestType == this.arabic('complaint'))
          this.router.navigate(['ar/app/citizen-affair/complaint-suggestion-edit/' + value.citizenAffairID]);
        else
          this.router.navigate(['ar/app/citizen-affair/citizen-affair-edit/' + value.citizenAffairID]);
      } else {
        if (value.requestType == this.arabic('complaint'))
          this.router.navigate(['ar/app/citizen-affair/complaint-suggestion-view/' + value.citizenAffairID]);
        else
          this.router.navigate(['ar/app/citizen-affair/citizen-affair-view/' + value.citizenAffairID]);
      }
    }
  }

  initFilter() {
    this.filter = {
      Status: (this.common.language == 'English') ? 'All' : this.arabic('all'),
      PhoneNumber: '',
      ReqDateFrom: new Date,
      ReqDateTo: new Date,
      ReferenceNumber: '',
      RequestType: (this.common.language == 'English') ? 'All' : this.arabic('all'),
      PersonalLocationName: '',
      SmartSearch: '',
      sourcename: (this.common.language == 'English') ? 'All' : this.arabic('all')
    };
    this.ReqDateFrom = '';
    this.ReqDateTo = '';
  }

  async getUserList(data) {
    this.common.getmemoUserList(data, 0).subscribe((list: any) => {
      var all = (this.common.language == 'English') ? "All" : this.arabic('all');
      let user = [];
      user.push({ 'UserID': 0, 'EmployeeName': all });
      list.map(res => user.push(res));
      this.userList = user;
      // this.filter.sourcename = this.userList[0];
    });
  }

  getListCount() {
    this.service.getCount(this.currentUser.id).subscribe((res: any) => {
      this.cardDetails[0].count = res.New;
      this.cardDetails[1].count = res.InProgressRequest;
      this.cardDetails[2].count = res.NeedMoreInfo;
      this.cardDetails[4].count = res.Closed;
      this.cardDetails[3].count = res.Closed;
      if (!this.IsDocumentLoad) {
        this.cardDetails[0].show = false;
        this.cardDetails[1].show = false;
        this.cardDetails[2].show = false;
        this.cardDetails[3].show = false;
        this.cardDetails[4].show = false;
        this.type = 5;
      } else {
        this.cardDetails[4].show = false;
        this.cardDetails.pop();
      }
      this.getList();
    });
  }

  getList(filter: any= '') {
    this.progress = true;
    var list = [];
    list.push({ 'LookupsID': 0, 'DisplayName': (this.common.language == 'English') ? 'All': this.arabic('all') });
    this.service.getList(this.page, this.maxSize, this.currentUser.id, this.type, filter).subscribe((res: any) => {
      this.rows = res.Collection;
      res.M_LookupsList.map(r => list.push(r));
      this.status = list;
      this.count = res.Count;
      this.progress = false;
      this.rows.map(val => {
        val.requestDate = this.datePipe.transform(val.requestDate, "dd/MM/yyyy");
      });
    });
  }
  FilterLoad(event) {
    var refid = (this.common.language == 'English') ? 'Ref ID' : this.arabic('refid'),
      status = (this.common.language == 'English') ? 'Status' : this.arabic('status'),
      assignto = (this.common.language == 'English') ? 'Assign To' : this.arabic('assignedto'),
      requesttype = (this.common.language == 'English') ? 'Request Type' : this.arabic('requesttype'),
      requestdate = (this.common.language == 'English') ? 'Request Date' : this.arabic('requestdate'),
      sourcefield = (this.common.language == 'English') ? 'Reporter' : this.arabic('reporter'),
      createdby = (this.common.language == 'English') ? 'Created By' : this.arabic('createdby'),
      phonenumber = (this.common.language == 'English') ? 'Phone Number' : this.arabic('phonenumber'),
      personallocation = (this.common.language == 'English') ? 'Personal/ Location Name' : this.arabic('personal/locationname'),
      action = (this.common.language == 'English') ? 'Action' : this.arabic('action'),
      request = (this.common.language == 'English') ? 'Complaints/Suggestion' : this.arabic('complaint');
    if (this.isCurrentUnit) {
      if (event == request) {
        this.IsPlaceLoc = false;
        this.columns = [
          { name: refid, prop: 'referenceNumber' },
          { name: status, prop: 'status' },
          { name: assignto, prop: 'assignedTo' },
          { name: requesttype, prop: 'requestType', width: 200 },
          { name: requestdate, prop: 'requestDate' },
          { name: sourcefield, prop: 'reporter' },
          { name: createdby, prop: 'creator' },
          { name: phonenumber, prop: 'phoneNumber' },
          { name: action, prop: '', cellTemplate: this.actionTemplate },
        ];
      }
      else {
        this.IsPlaceLoc = true;
        this.columns = [
          { name: refid, prop: 'referenceNumber' },
          { name: status, prop: 'status' },
          { name: assignto, prop: 'assignedTo' },
          { name: requesttype, prop: 'requestType', width: 200 },
          { name: requestdate, prop: 'requestDate' },
          { name: personallocation, prop: 'personalName' },
          { name: phonenumber, prop: 'phoneNumber' },
          { name: action, prop: '', cellTemplate: this.actionTemplate },
        ];
      }
    }
    else {
      if (event == request) {
        this.IsPlaceLoc = false;
        this.columns = [
          { name: refid, prop: 'referenceNumber' },
          { name: status, prop: 'status' },
          { name: requesttype, prop: 'requestType', width: 200 },
          { name: requestdate, prop: 'requestDate' },
          { name: sourcefield, prop: 'reporter' },
          { name: createdby, prop: 'creator' },
          { name: phonenumber, prop: 'phoneNumber' },
          { name: action, prop: '', cellTemplate: this.actionTemplate },
        ];
      }
      else {
        this.IsPlaceLoc = true;
        this.columns = [
          { name: refid, prop: 'referenceNumber' },
          { name: status, prop: 'status' },
          { name: requesttype, prop: 'requestType', width: 200 },
          { name: requestdate, prop: 'requestDate' },
          { name: personallocation, prop: 'personalName' },
          { name: phonenumber, prop: 'phoneNumber' },
          { name: action, prop: '', cellTemplate: this.actionTemplate },
        ];
      }
    }

  }
  changeType(type) {
    //debugger;
    this.initFilter();
    this.service.triggerScrollTo();
    this.type = type;
    this.getList();
  }

  onChangePage(page) {
    this.page = page;
    if(this.searchFilter){
    this.filterList();
    }else{
    this.getList();
    }
  }

  filterList() {
    this.searchFilter = true;
    let PersonalLocationName = '', PhoneNumber, SmartSearch = '', Status = '', ReqDateFrom = '', ReqDateTo = '', 
    ReferenceNumber = '', RequestType = '', sourcename = '';
    
    var all = (this.common.language == 'English') ? 'All' : this.arabic('all');
    if (this.ReqDateFrom)
    ReqDateFrom = new Date(this.ReqDateFrom).toJSON();
    else
    ReqDateFrom = '';
    if (this.ReqDateTo)
    ReqDateTo = new Date(this.ReqDateTo).toJSON();
    else
    ReqDateTo = '';
    if (this.filter.RequestType && this.filter.RequestType != all){
      RequestType = this.filter.RequestType;
    }
    if (this.filter.Status && this.filter.Status != all){
      Status = this.filter.Status;
    }
    if (this.filter.sourcename && this.filter.sourcename != all){
      sourcename = this.filter.sourcename;
    }

    PersonalLocationName = this.filter.PersonalLocationName;
    ReferenceNumber = this.filter.ReferenceNumber;
    PhoneNumber = this.filter.PhoneNumber;
    SmartSearch = this.filter.SmartSearch;
    let filterData = {
      "Status": Status,
      "sourcename": sourcename,
      "PhoneNumber": PhoneNumber,
      "PersonalLocationName": PersonalLocationName,
      "ReqDateFrom": ReqDateFrom,
      "ReqDateTo": ReqDateTo,
      "ReferenceNumber": ReferenceNumber,
      "RequestType": RequestType,
      "SmartSearch": SmartSearch
    };
    this.getList(filterData);

  }

  openReport() {
    this.bsModalRef = this.modalService.show(CitizenReportModalComponent, { class: 'modal-lg' });
    this.bsModalRef.content.userList = this.userList;
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  minDate(date) {
    return this.util.minDateCheck(date);
  }
  maxDate(date) {
    return this.util.maxDateCheck(date);
  }

  dateValidation() {
    this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date' : this.arabic('errormsgvalidenddate');
    let showDateValidationMsg = false;
    if (!this.ReqDateFrom && this.ReqDateTo) {
      showDateValidationMsg = false;
    } else if (this.ReqDateFrom && this.ReqDateTo) {
      let startDate = new Date(this.ReqDateFrom).getTime();
      let endDate = new Date(this.ReqDateTo).getTime();
      if (endDate < startDate) {
        showDateValidationMsg = true;
      } else {
        showDateValidationMsg = false;
      }
    } else {
      showDateValidationMsg = false;
    }
    return showDateValidationMsg;
  }
}
