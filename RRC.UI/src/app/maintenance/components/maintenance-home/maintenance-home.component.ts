import { ArabicDataService } from './../../../arabic-data.service';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { MaintenanceService } from '../../service/maintenance.service';
import { CommonService } from 'src/app/common.service';
import { Router } from '@angular/router';
import { MaintenanceReportModalComponent } from 'src/app/modal/maintenance-report-modal/maintenance-report-modal.component';


@Component({
  selector: 'app-maintenance-home',
  templateUrl: './maintenance-home.component.html',
  styleUrls: ['./maintenance-home.component.scss']
})
export class MaintenanceHomeComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('dateColumn') dateColumn: TemplateRef<any>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  type:any;
  statusList:Array<any> = [];
  status:any;
  sourceOUList:Array<any> = [];
  sourceOU: any;
  priorityList: any;
  priority: any;
  subject: string = '';
  dateTo: Date;
  dateFrom: Date;
  attendedBy: string = '';
  smartSearch: string ='';
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  colorTheme = 'theme-green';
  homeCards: Array<any> = [];
  userCards:any;
  public columns: Array<any> = [];
  public arabicColumns: Array<any> = [];
  public rows: Array<any> = [];
  public page: Number = 1;
  public pageSize: Number = 10;
  public pageCount: Number;
  public maxSize: Number = 10;
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  OrgUnitID: Number;
  IsOrgHead:boolean = false;
  userType:string = 'NON_M_TEAM';
  bsModalRef: BsModalRef;
  username:string = '';
  lang: string;
  arWords: any;
  tableMessages:any;
  pendingForResubmit :string  = 'في انتظار إعادة التقديم';

  constructor(
    public datePipe: DatePipe,
    private maintenanceService: MaintenanceService,
    public router: Router,
    private modalService: BsModalService,
    private common: CommonService,
    public arabic: ArabicDataService
  ) {
    this.lang = this.common.currentLang;
    this.arWords = this.arabic.words;
    this.common.breadscrumChange(
      this.lang === 'en' ? 'Maintenance' : this.arWords.maintenance,
      this.lang === 'en' ? 'Home Page' : this.arWords.homepage, 
      '');
    this.common.topBanner(
      true,
      this.lang === 'en' ? 'Maintenance' : this.arWords.dashboard,
      this.lang === 'en' ? '+ CREATE REQUESTS' : this.arWords.plusCreateRequests,
      'maintenance/create');
      this.priorityList = [{
        id: 0, 
        name: this.lang === 'en' ? 'All' : this.arWords.all,
        value: ''
      }, {
        id: 1,
        name: this.lang === 'en' ? 'High' : this.arWords.high,
        value: this.lang === 'en' ? 'High' : this.arWords.high
      }, {
        id: 2,
        name: this.lang === 'en' ? 'Low' : this.arWords.low,
        value: this.lang === 'en' ? 'Low' : this.arWords.low
      }];
      this.tableMessages = {
        emptyMessage: this.lang === 'en' ? 'No Items Found' : this.arWords.noItemsFound
      }
      this.userCards = [
        {
          image: 'assets/hr-dashboard/inbox.png',
          title: this.lang === 'ar' ? this.arWords.maintenancedocuments : 'Maintenance Documents'
        }
      ];
      this.pendingForResubmit = this.lang === 'ar' ? 'في انتظار إعادة التقديم' : 'Pending for Resubmission';
    }

  ngOnInit() {
    this.username = this.currentUser.username;
    this.formHomeCards();
    this.loadDashboad();
    this.loadList();
  }

  formHomeCards() {
    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? parseInt(this.currentUser.OrgUnitID) : 0;
    this.type = this.OrgUnitID == 12 ? 1 : 4
    if (this.OrgUnitID == 12) {
      this.homeCards.push({
        'type': 1,
        'image': 'assets/maintenance/tabs.png',
        'count': 0,
        'progress': 50,
        'requestType':'New',
        'displayName': this.lang === 'ar' ? this.arWords.new : 'New',
      }, {
        'type': 7,
        'image': 'assets/maintenance/tabs.png',
        'count': 0,
        'progress': 50,
        'requestType':'InProgressRequest',
        'displayName': this.lang === 'ar' ? this.arWords.inprogresscard : 'In Progress',
      }, {
        'type': 2,
        'image': 'assets/maintenance/info.png',
        'count': 0,
        'progress': 50,
        'requestType':'NeedMoreInfo',
        'displayName': this.lang === 'ar' ? this.arWords.needmoreinfo : 'Need more Info',
      }, {
        'type': 3,
        'image': 'assets/maintenance/close.png',
        'count': 0,
        'progress': 50,
        'requestType':'Closed',
        'displayName': this.lang === 'ar' ? this.arWords.closedrejectedcard : 'Closed/Rejected',
      });
    } else {
      this.homeCards.push({
        'type': 4,
        'image': 'assets/maintenance/tabs.png',
        'count': 0,
        'progress': 50,
        'requestType':'MyPendingRequest',
        'displayName': this.lang === 'ar' ? this.arWords.mypendingactions : 'My Pending Actions',
      },{
        'type': 5,
        'image': 'assets/maintenance/tabs.png',
        'count': 0,
        'progress': 50,
        'requestType':'MyOwnRequest',
        'displayName': this.lang === 'ar' ? this.arWords.myownrequests : 'My Own Requests',
      },
      {
        'type': 6,
        'image': 'assets/maintenance/tabs.png',
        'count': 0,
        'progress': 50,
        'requestType':'MyProcessedRequest',
        'displayName': this.lang === 'ar' ? this.arWords.myprocessedrequest : 'My Processed Request',
      });
    }
  }

  onSearch() {
    this.loadList();
  }

  onCardClick(type:Number) {
    this.type = type;
    // clear params on card change
    this.status = '';
    this.sourceOU = '';
    this.subject = '';
    this.dateFrom = undefined;
    this.dateTo = undefined;
    this.attendedBy = '';
    this.priority = '';
    this.smartSearch = '';
    this.loadList();
    this.maintenanceService.triggerScrollTo();
  }

  loadDashboad() {
    this.maintenanceService.getMaintenanceDashboard(this.currentUser.id)
      .subscribe((dashboard:any) => {
        this.homeCards.forEach((cardItem:any) => {
          switch(cardItem.requestType) {
            case 'New': cardItem.count = dashboard.New || 0;break;
            case 'NeedMoreInfo': cardItem.count = dashboard.NeedMoreInfo || 0;break;
            case 'Closed': cardItem.count = dashboard.Closed || 0;break;
            case 'MyOwnRequest': cardItem.count = dashboard.MyOwnRequest || 0;break;
            case 'MyPendingRequest': cardItem.count = dashboard.MyPendingRequest || 0;break;
            case 'InProgressRequest': cardItem.count = dashboard.InProgressRequest || 0;break;
            case 'MyProcessedRequest' : cardItem.count = dashboard.MyProcessedRequest || 0;break;
          }
        });
      });
  }

  loadList() {
    this.columns.push({ name: this.lang === 'en' ? 'Ref ID' : this.arWords.refid, prop: 'ReferenceNumber' });
    this.columns.push({ name: this.lang === 'en' ? 'Source' : this.arWords.source, prop: 'Source' });
    this.columns.push({ name: this.lang === 'en' ? 'Subject' : this.arWords.subject, prop: 'Subject' });
    this.columns.push({ name: this.lang === 'en' ? 'Status' : this.arWords.status, prop: 'Status' });
    if (this.currentUser.OrgUnitID == 12) {
      this.columns.push({ name: this.lang === 'en' ? 'Assigned To' : this.arWords.assignedto ,prop:'AssignedTo'});
    }
    this.columns.push({ name: this.lang === 'en' ? 'Request Date' : this.arWords.requestdate, prop: 'RequestDate', cellTemplate: this.dateColumn });
    this.columns.push({ name: this.lang === 'en' ? 'Submitted By' : this.arWords.legalsubmittedby, prop: 'AttendedBy' });
    this.columns.push({ name: this.lang === 'en' ? 'Priority' : this.arWords.priority, prop: 'Priority' });
    this.columns.push({ name: this.lang === 'en' ? 'Action' : this.arWords.action, prop: '', cellTemplate: this.actionTemplate });

    //param validation
    if (!this.type) this.type = '';
    if (!this.status) this.status = '';
    if (!this.sourceOU) this.sourceOU = '';
    if (!this.priority) this.priority = '';
    let fromDate = this.dateFrom ? new Date(this.dateFrom).toJSON() : '';
    let toDate = this.dateTo ? new Date(this.dateTo).toJSON() : '';

    // query formation
    let queryType = `?Type=${this.type}`;
    let queryStatus = `${queryType}&Status=${this.status}`;
    let querySourceOU = `${queryStatus}&SourceOU=${this.sourceOU}`;
    let querySourceName = `${querySourceOU}&SourceName=${this.currentUser.username}`;
    let queryPriority = `${querySourceName}&Priority=${this.priority}`;
    let queryLable = `${queryPriority}&Lable=`;
    let querySubject = `${queryLable}&Subject=${this.subject}`;
    let queryDateFrom = `${querySubject}&ReqDateFrom=${fromDate}`;
    let queryDateTo = `${queryDateFrom}&ReqDateTo=${toDate}`;
    let queryAttendedBy = `${queryDateTo}&AttendedBy=${this.attendedBy}`;
    let queryFinal = `${queryAttendedBy}&SmartSearch=${this.smartSearch}&UserID=${this.currentUser.id}`;

    this.maintenanceService.getMaintenanceList(this.page, this.pageSize, queryFinal)
      .subscribe((response:any) => {
        this.rows = response.Collection;
        this.statusList = [{
          LookupsID: '', 
          DisplayName: this.common.currentLang == 'ar' ? this.arWords.all : 'All'
        }].concat(response.M_LookupsList);
        this.sourceOUList = [{
          OrganizationID: '', 
          OrganizationUnits: this.common.currentLang == 'ar' ? this.arWords.all : 'All'
        }].concat(response.OrganizationList);
        this.pageCount = parseInt(response.Count);
      });
  }

  viewData(value:any) {
    this.router.navigate(['/'+this.common.currentLang+'/app/maintenance/view/' + value.MaintenanceID]);
  }

  onChangePage(page:any) {
    this.page = page;
    this.loadList();
  }

  openReport() {
    this.bsModalRef = this.modalService.show(MaintenanceReportModalComponent,{class:'modal-lg'});
  }
}
