import { Component, OnInit, TemplateRef, ViewChild, Renderer2, Inject } from '@angular/core';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { MeetingService } from '../../service/meeting.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DatePipe, DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-list-page',
  templateUrl: './list-page.component.html',
  styleUrls: ['./list-page.component.scss']
})
export class ListPageComponent implements OnInit {
  meetingId: any;
  Subject: string;
  Location: string;
  EventRequestor: string;
  StartDatetime: string;
  EndDatetime: string;
  MeetingType = '';
  Invites = '';
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  public page: number = 1;
  public pageSize: number = 10;
  public pageCount: number;
  public maxSize: number = 10;
  public rows: Array<any> = [];
  public columns: Array<any> = [];
  public arabicColumns: Array<any> = [];
  public itemsPerPage: number = 10;
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalItems: []
  };
  MeetingTypes: Array<any>;
  Invitees: Array<any>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  UserId: any;
  progress: boolean = true;
  meetingList: any = [];
  @ViewChild('dateColumn') dateColumn: TemplateRef<any>;
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('template') template: TemplateRef<any>;
  bsModalRef: BsModalRef;
  smartSearch: string;
  lang: any;
  isEngLang: boolean = true;
  reportFilter:any = {
    meetingId: null,
    Subject: '',
    Location: '',
    EventRequestor: '',
    StartDatetime: '',
    EndDatetime: '',
    MeetingType: '',
    Invites: '',
  };
  tableMessages: { emptyMessage: any; };

  constructor(
    private common: CommonService, public service: MeetingService, public router: Router, public datepipe: DatePipe, public modalService: BsModalService,
    private renderer: Renderer2, @Inject(DOCUMENT) private document: Document, private activeRoute: ActivatedRoute
  ) {

    activeRoute.url.subscribe(() => {
      this.lang = activeRoute.snapshot.parent.parent.parent.parent.params.lang;
      // if (this.lang == 'en')
      //   this.common.language = 'English';
      // else
      //   this.common.language = this.arabic('arabic');
    });
    // this.lang = this.common.currentLang;
  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabic('nodatatodisplay')
    };
   
    if (this.lang == 'en') {
      this.isEngLang = true;
      this.MeetingTypes = [
        { 'label': 'All', 'value': '' },
        { 'label': 'Meeting', 'value': '1' },
        { 'label': 'Workshop', 'value': '2' },
        { 'label': 'Training', 'value': '3' },
        { 'label': 'Others', 'value': '4' }
      ];
      this.Invitees = [
        { 'label': 'All', 'value': '' },
        { 'label': 'Internal', 'value': 'Internal' },
        { 'label': 'External', 'value': 'External' },
        { 'label': 'Internal and External', 'value': 'Internal and External' }
      ];
    this.common.sideNavResponse('meeting', 'Meeting List');
    } else {
      this.isEngLang = false;
      this.MeetingTypes = [
        { 'label': this.arabic('all'), 'value': '' },
        { 'label': this.arabic('meeting'), 'value': '1' },
        { 'label': this.arabic('workshop'), 'value': '2' },
        { 'label': this.arabic('training'), 'value': '3' },
        { 'label': this.arabic('others'), 'value': '4' }
      ];
      this.Invitees = [
        { 'label': this.arabic('all'), 'value': '' },
        { 'label': this.arabic('internal'), 'value': 'Internal' },
        { 'label': this.arabic('external'), 'value': 'External' },
        { 'label': this.arabic('internalandexternal'), 'value': 'Internal and External' }
      ];
      this.common.sideNavResponse('meeting', this.common.arabic.sideNavArabic['Meeting List']);
    }
    this.UserId = this.currentUser.id;
    this.common.topSearchBanner(false, '', '', '');
    if(this.isEngLang){
      this.common.topBanner(true, 'Meeting', '+ CREATE MEETING', '/en/app/meeting/create');
      this.common.breadscrumChange('Meetings', 'List Page', '');
    }else{
      this.common.topBanner(true, this.arabic('meeting'), '+ '+ this.arabic('createmeeting'), '/ar/app/meeting/create');
      this.common.breadscrumChange(this.arabic('meeting'), this.arabic('listpage'), '');
    }

    this.loadList();
    this.columns = [
      { name: 'Ref ID', prop: 'ReferenceNumber' },
      { name: 'Subject', prop: 'Subject' },
      { name: 'Location', prop: 'Location' },
      { name: 'Start Date/Time', prop: 'StartDateTime', cellTemplate: this.dateColumn },
      { name: 'End Date/Time', prop: 'EndDateTime', cellTemplate: this.dateColumn },
      { name: 'Meeting Type', prop: 'MeetingType' },
      { name: 'Invitees', prop: 'Invitees' },
      { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
    ];
    this.arabicColumns = [
      { name: this.arabic('refid'), prop: 'ReferenceNumber' },
      { name: this.arabic('subject'), prop: 'Subject' },
      { name: this.arabic('location'), prop: 'Location' },
      { name: this.arabic('startdatetime'), prop: 'StartDateTime', cellTemplate: this.dateColumn },
      { name: this.arabic('enddatetime'), prop: 'EndDateTime', cellTemplate: this.dateColumn },
      { name: this.arabic('meetingtype'), prop: 'MeetingType' },
      { name: this.arabic('invitees'), prop: 'Invitees' },
      { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
    ];
  }

  public onChangePage(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
    this.page = page;
    this.loadList();

    this.tableMessages = {
      emptyMessage: this.lang == 'en' ? 'No data to display' : this.arabic('nodatatodisplay')
    }
  }

  loadList() {
    let MeetingID = '';
    let Subject = '';
    let StartDatetime = '';
    let EndDatetime = '';
    let MeetingType = '';
    let Location = '';
    let invitees = '';
    let smartSearch = '';

    if (this.smartSearch) {
      smartSearch = this.smartSearch
    }
    if (this.meetingId) {
      MeetingID = this.meetingId
    }
    if (this.Subject) {
      Subject = this.Subject
    }
    if (this.Location) {
      Location = this.Location
    }
    if (this.MeetingType) {
      MeetingType = this.MeetingType
    }
    if (this.StartDatetime) {
      StartDatetime = new Date(this.StartDatetime).toJSON();
    }
    if (this.EndDatetime) {
      EndDatetime = new Date(this.EndDatetime).toJSON();
    }
    if (this.Invites) {
      invitees = this.Invites
    }
    this.service.getMeetingList(
      this.page, this.maxSize, smartSearch, this.UserId, MeetingID, Subject,
      StartDatetime, EndDatetime, MeetingType, Location, invitees).subscribe(
        (data: any) => {
          this.progress = false;
          this.meetingList = data;
          this.rows = this.meetingList.Collection;
          this.config.totalItems = this.meetingList.Count;
        }
      );
  }

  viewData(type, value) {
    this.router.navigate(['/app/meeting/view/' + value.MeetingID]);
  }

  dateValidation() {
    let startDate = new Date(this.StartDatetime).getTime();
    let endDate = new Date(this.EndDatetime).getTime();
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

  formatAMPM(date) {
    var time;
    var mins;
    var hours;
    date = new Date(date);
    mins = date.getMinutes();
    hours = date.getHours();
    mins = (parseInt(mins) % 60) < 10 ? '0' + (parseInt(mins) % 60) : (parseInt(mins) % 60);
    hours = (parseInt(hours) % 60) < 10 ? '0' + (parseInt(hours) % 60) : (parseInt(hours) % 60);
    var StartDateVal = this.datepipe.transform(date, 'dd/MM/yyyy');
    time = StartDateVal + "\n" + hours + ":" + mins;
    return time;
  }

  openReport() {
    this.reportFilter = {
      meetingId: null,
      Subject: '',
      Location: '',
      EventRequestor: '',
      StartDatetime: '',
      EndDatetime: '',
      MeetingType: '',
      Invites: '',
      smartSearch:''
    };
    this.bsModalRef = this.modalService.show(this.template, { class: 'modal-lg' });
  }

  DownloadExcel() {
    let MeetingID = '';
    let Subject = '';
    let StartDatetime = '';
    let EndDatetime = '';
    let MeetingType = '';
    let Location = '';
    let Invitees = '';
    let smartSearch = '';
    if (this.reportFilter.meetingId) {
      MeetingID = this.reportFilter.meetingId
    }
    if (this.reportFilter.Subject) {
      Subject = this.reportFilter.Subject
    }
    if (this.reportFilter.Location) {
      Location = this.reportFilter.Location
    }
    if (this.reportFilter.MeetingType) {
      MeetingType = this.reportFilter.MeetingType
    }
    if (this.reportFilter.StartDatetime) {
      StartDatetime = new Date(this.reportFilter.StartDatetime).toJSON();
    }
    if (this.reportFilter.EndDatetime) {
      EndDatetime = new Date(this.reportFilter.EndDatetime).toJSON();
    }
    if (this.reportFilter.Invites) {
      Invitees = this.reportFilter.Invites
    }
    if (this.reportFilter.smartSearch) {
      smartSearch = this.reportFilter.smartSearch
    }

    const reportModel = {
      ReferenceNumber: MeetingID,
      Subject: Subject,
      Location: Location,
      MeetingType: MeetingType,
      Invitees: Invitees,
      UserID: this.currentUser.id,
      SmartSearch: smartSearch
    }
    let date = new Date,
      cur_date = date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
    this.service.getExport(reportModel)
      .subscribe((resultBlob: Blob) => {
        this.initPage();
        var url = window.URL.createObjectURL(resultBlob);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = 'Meeting_Report_' + cur_date + '.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
        this.bsModalRef.hide();
      });
  }

  initPage() {
    this.meetingId = '';
    this.Subject = '';
    this.Location = '';
    this.MeetingType = '';
    this.Invites = '';
    this.smartSearch = '';
  }

  closemodal() {
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
    // this.initPage();
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}
