import { HrRequestTypes } from './../../../../shared/enum/hr-request-types/hr-request-types.enum';
import { CalendarManagementService } from './../../service/calendar-management.service';
import { Router } from '@angular/router';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { CommonService } from './../../../../common.service';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CalendarManagementReportModalComponent } from 'src/app/modal/calendar-management-report-modal/calendar-management-report-modal.component';
import * as _ from 'lodash';

@Component({
  selector: 'app-list-page',
  templateUrl: './list-page.component.html',
  styleUrls: ['./list-page.component.scss']
})
export class ListPageComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('startDateTemplate') startDateTemplate: TemplateRef<any>;
  @ViewChild('endDateTemplate') endDateTemplate: TemplateRef<any>;
  @ViewChild('eventTypeTemplate') eventTypeTemplate: TemplateRef<any>;
  ReferenceId: number;
  EventType: string;
  EventRequestor: string;
  StartDate: string;
  EndDate: string;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  public page: number = 1;
  public pageSize: number = 10;
  public pageCount: number;
  public maxSize: number = 10;
  columns: Array<any> = [];
  arabicColumns: Array<any> = [];
  StatusList: Array<any> = [];
  lang: string;
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    pageSize: 10,
    page: 1
  };
  filterBy: any = {
    ReferenceNumber: null,
    EventType: null,
    EventRequestor: null,
    StartDate: null,
    EndDate: null,
    SmartSearch: '',
    Status: null,
    Type: 2
  };
  EventTypes: Array<any> = [
    // { 'label': 'All', 'value': ''},
    // { 'label': 'Meeting', 'value': '1'},
    // { 'label': 'Visit', 'value': '2'},
    // { 'label': 'Wedding', 'value': '3'},
    // { 'label': 'Interview', 'value': '4'},
    // { 'label': 'Appointment', 'value': '5'}
  ];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  rows: any;
  bsModalRef: BsModalRef;
  homeCards: Array<any> = [];
  userCards = [
    {
      image: 'assets/hr-dashboard/inbox.png',
      title: 'Maintenance Documents'
    }
  ];

  dropDownEventTypes:Array<any> = [];
  tableMessages: { emptyMessage: any; };

  constructor(
    private common: CommonService,
    public arabic: ArabicDataService,
    public router: Router,
    private modalService: BsModalService,
    public calendarManagementService: CalendarManagementService
  ) {
    this.lang = this.common.currentLang;
  }

  ngOnInit() {
    if (this.lang === 'en') {
      this.common.topBanner(true, 'Protocol Calendar', '+ CREATE EVENTS', '/app/media/calendar-management/create-event');
      this.common.breadscrumChange('Protocol Calendar', 'List Page', '');
      this.filterBy.Status = 'All';
      this.filterBy.EventType = '';
    } else {
      this.common.topBanner(true, this.arabicfn('protocolcalendar'), '+ ' + this.arabicfn('createevent'), '/app/media/calendar-management/create-event');
      this.common.breadscrumChange(this.arabicfn('protocolcalendar'), this.arabicfn('listpage'), '');
      this.filterBy.EventType = '';
      this.filterBy.Status = this.arabicfn('all');
    }
    this.tableMessages = {
      emptyMessage: this.lang == 'en' ? 'No data to display' : this.arabicfn('nodatatodisplay')
    }
    this.loadList();
    // this.loadListOfRequests('');
    this.formHomeCards();
    this.loadEventTypes();
  }

  formHomeCards() {
    this.calendarManagementService.getCardCount(this.currentUser.id).subscribe((res:any) => {
      if (this.lang == 'en') {
        this.homeCards.push({
          'type': 1,
          'image': 'assets/maintenance/tabs.png',
          'count': res.MyEvents || 0,
          'progress': 50,
          'requestType':'MyEvents',
          'displayName':'My Events',
        },{
          'type': 2,
          'image': 'assets/maintenance/tabs.png',
          'count': res.MyPendingRequest || 0,
          'progress': 50,
          'requestType':'MyPendingAction',
          'displayName':'My Pending Actions',
        },{
          'type': 3,
          'image': 'assets/maintenance/tabs.png',
          'count': res.Approved || 0,
          'progress': 50,
          'requestType':'Approved',
          'displayName':'Approved',
        },{
          'type': 4,
          'image': 'assets/maintenance/tabs.png',
          'count':res.AllEvents || 0,
          'progress': 50,
          'requestType':'All',
          'displayName':'All',
        });
      } else {
        this.homeCards.push({
          'type': 1,
          'image': 'assets/maintenance/tabs.png',
          'count': res.MyEvents || 0,
          'progress': 50,
          'requestType':'MyEvents',
          'displayName': this.arabicfn('myevents'),
        },{
          'type': 2,
          'image': 'assets/maintenance/tabs.png',
          'count': res.MyPendingRequest || 0,
          'progress': 50,
          'requestType':'MyPendingAction',
          'displayName': this.arabicfn('mypendingactions'),
        },{
          'type': 3,
          'image': 'assets/maintenance/tabs.png',
          'count': res.Approved || 0,
          'progress': 50,
          'requestType':'Approved',
          'displayName': this.arabicfn('approved'),
        },{
          'type': 4,
          'image': 'assets/maintenance/tabs.png',
          'count':res.AllEvents || 0,
          'progress': 50,
          'requestType':'All',
          'displayName': this.arabicfn('all'),
        });
      }
    });
  }

  onChangePage(config,event) {
    this.loadListOfRequests('');
  }

  getEventTypeByID(id:any) {
    let types = _.filter(this.EventTypes, { 'value': id});
    if (types && types.length > 0) {
      let type: any = types[0];
      return type.label || 'N/A';
    }
    return 'N/A';
  }

  loadList() {
    this.columns = [
      { name: 'Reference ID', prop: 'ReferenceNumber' },
      { name: 'Event Requestor', prop: 'EventRequestor' },
      { name: 'Event Type', cellTemplate: this.eventTypeTemplate },
      { name: 'Start Date', cellTemplate: this.startDateTemplate },
      { name: 'End Date', cellTemplate: this.endDateTemplate },
      { name: 'Action', cellTemplate: this.actionTemplate }
    ];
    this.arabicColumns = [
      { name: this.arabicfn('referenceid'), prop: 'ReferenceNumber' },
      { name: this.arabicfn('eventrequestor'), prop: 'EventRequestor' },
      { name: this.arabicfn('eventtype'), cellTemplate: this.eventTypeTemplate },
      { name: this.arabicfn('startdate'), cellTemplate: this.startDateTemplate },
      { name: this.arabicfn('enddate'), cellTemplate: this.endDateTemplate },
      { name: this.arabicfn('action'), cellTemplate: this.actionTemplate }
    ];
  }

  viewDataSingleEventView(value: any) {
    this.router.navigate(['/app/media/calendar-management/view-event/' + value.CalendarID]);
  }

  viewDataBulkEventView(value: any) {
    this.router.navigate(['/app/media/calendar-management/event-list/' + value.CalendarID]);
  }

  openReport() {
    let initialState = {};
    this.bsModalRef = this.modalService.show(CalendarManagementReportModalComponent, {class: 'modal-lg'});
  }

  onSearch() {
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.loadListOfRequests('');
    this.dateValidation();
  }

  loadListOfRequests(value: any) {
    let ReferenceNumber = '';
    let EventType:number;
    let EventRequestor = '';
    let StartDate = '';
    let EndDate = '';
    let SmartSearch = '';
    let Status = '';
    let Type = '';
    if(this.filterBy.SmartSearch) {
      SmartSearch = this.filterBy.SmartSearch;
    }
    if(this.filterBy.ReferenceNumber) {
      ReferenceNumber = this.filterBy.ReferenceNumber;
    }

    if(this.filterBy.Status) {
      Status = this.filterBy.Status;
    }

    if(!this.filterBy.Status) {
      this.filterBy.Status = 'All';
      if(this.lang != 'en'){
        this.filterBy.Status = this.arabicfn('all');
      }
    }

    if(this.filterBy.EventType >= 0 && this.filterBy.EventType != null) {
      EventType = this.filterBy.EventType;
    }

    if(!this.filterBy.EvenType){
      this.filterBy.EventType = '';
    }

    if(this.filterBy.EventRequestor) {
      EventRequestor = this.filterBy.EventRequestor;
    }
    if(this.filterBy.StartDate) {
      StartDate = new Date(this.filterBy.StartDate).toJSON();
    }
    if(this.filterBy.EndDate){
      EndDate = new Date(this.filterBy.EndDate).toJSON();
    }
    if(this.filterBy.Type) {
      Type = this.filterBy.Type;
    }
    let toSendParams:any = {
      pageNumber : this.config.page,
      pageSize : this.config.pageSize,
      UserID : this.currentUser.id,
      Type: Type,
      ReferenceNumber: ReferenceNumber,
      EventRequestor : EventRequestor,
      StartDate: StartDate,
      EndDate : EndDate,
      Status: Status,
      SmartSearch: SmartSearch
    };
    if(EventType >= 0){
      toSendParams.EventType = EventType;
    }else{
      toSendParams.EventType = '';
    }

    if (this.filterBy.Status == null || this.filterBy.Status == 'All' || this.filterBy.Status == this.arabicfn('all')) {
      toSendParams.Status = '';
    }
    this.calendarManagementService.getList(toSendParams).subscribe((allOwnRes: any) => {
      if (allOwnRes) {
        this.rows = allOwnRes.Collection;
        this.StatusList = allOwnRes.M_LookupsList;
        if (this.lang == 'en') {
          this.StatusList.unshift({DisplayName: "All"});
        } else {
          this.StatusList.unshift({DisplayName: this.arabicfn('all')});
        }
        this.config.totalItems = allOwnRes.Count;
          if (value.isbulkevent == 1) {
          this.viewDataBulkEventView(value);
        } else  if (value.isbulkevent == 0) {
          this.viewDataSingleEventView(value);
        }
        // this.departmentList = allOwnRes.OrganizationList;
        // this.statusList = allOwnRes.M_LookupsList;
      }
    });
  }

  dateValidation() {
    let startDate = new Date(this.filterBy.StartDate).getTime();
    let endDate = new Date(this.filterBy.EndDate).getTime();
    if (startDate && endDate) {
      if (startDate > endDate) {
        return true;
      } else {
        return false;
      }
    } else if (!startDate && endDate) {
      return true;
    } else {
      return false;
    }
  }

  onCardClick(Type){
    this.filterBy = {
      ReferenceNumber: null,
      EventType: '',
      Status: this.lang=='en' ? 'All' : this.arabicfn('all'),
      EventRequestor: null,
      StartDate: null,
      EndDate: null,
      SmartSearch: '',
    };
    this.filterBy.Type = Type;
    this.loadListOfRequests('');
    this.calendarManagementService.triggerScrollTo();
  }

  loadEventTypes(){
    this.calendarManagementService.eventTypes(this.currentUser.id).subscribe(
      (response: any) => {
        if (this.lang == 'en') {
          var toDisplayEventList:any = [{ 'label': 'All', 'value': ''},{'label':'Bulk','value':0}];
          var dropDownEventList:any = [{ 'label': 'All', 'value': ''},{'label':'Bulk','value':0}];
        } else {
          var toDisplayEventList:any = [{ 'label': this.arabicfn('all'), 'value': ''},{'label': this.arabicfn('bulk'),'value':0}];
          var dropDownEventList:any = [{ 'label': this.arabicfn('all'), 'value': ''},{'label': this.arabicfn('bulk'),'value':0}];
        }
        this.EventTypes = [];
        response.forEach((eventTypeObj) => {
          toDisplayEventList.push({label:eventTypeObj.MeetingTypeName,value:eventTypeObj.EventID});
          dropDownEventList.push({label:eventTypeObj.MeetingTypeName,value:eventTypeObj.EventID});
        });
        this.EventTypes = toDisplayEventList;
        this.dropDownEventTypes = dropDownEventList;
        this.loadListOfRequests('');
      }
    );
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }

}
