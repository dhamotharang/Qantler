import { ArabicDataService } from './../../../arabic-data.service';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsDatepickerConfig, BsModalService } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { ItService } from '../../service/it.service';
import { ItReportModalComponent } from 'src/app/modal/it-report-modal/it-report-modal.component';
import { ItRequestType } from 'src/app/shared/enum/it-request-type/it-request-type.enum';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {
  bsModalRef: any;
  lang: string;
  constructor(
    private modalService: BsModalService,
    private common: CommonService,
    private itService: ItService,
    private router: Router,
    public arabic: ArabicDataService
  ) { this.lang = this.common.currentLang; }
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('creationDateTemplate') creationDateTemplate: TemplateRef<any>;
  @ViewChild('priorityTemplate') priorityTemplate: TemplateRef<any>;
  public columns: Array<any> = [];
  public arabicColumns: Array<any> = [];
  public rows: Array<any> = [];
  public page: number = 1;
  status: any;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  colorTheme = 'theme-green';
  changeList: any;
  departmentList: any=[];
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    pageSize: 10,
    page: 1
  };
  filterBy: any = {
    Status: 'All',
    RequestType: null,
    ReqDateFrom: null,
    ReqDateTo: null,
    SourceOU: 'All',
    Subject: null,
    Priority: '',
    SmartSearch: ''
  };
  isItDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 11;
  isItDepartmentTeamUserID = this.currentUser.OrgUnitID == 11 && !this.currentUser.IsOrgHead;
  itSupportSectionIconUrl = 'assets/it-dashboard/support.png';
  itServiceSectionIconUrl = 'assets/it-dashboard/services.png';
  itComponentSectionIconUrl = 'assets/it-dashboard/component.png';
  supportCards = [
    {
      'image': 'assets/maintenance/tabs.png',
      'count': 0,
      'progress': 50,
      'countType': 'SupportNew',
      requestType:ItRequestType['Support New'],
      title: 'New'
    },
    {
      'image': 'assets/it-dashboard/inprogress.png',
      'count': 0,
      'progress': 50,
      'countType': 'SupportNew',
      requestType:ItRequestType['Support Inprogress'],
      title: 'In Progress'
    },
    {
      'image': 'assets/citizen-affair/close.png',
      'count': 0,
      'progress': 50,
      'countType': 'SupportClose',
      requestType:ItRequestType['Support Closed'],
      title: 'Closed'
    }
  ];
  servicesCards = [
    {
      'image': 'assets/maintenance/tabs.png',
      'count': 0,
      'progress': 50,
      'countType': 'ServicesNew',
      requestType:ItRequestType['Services New'],
      title: 'New'
    },
    {
      'image': 'assets/it-dashboard/inprogress.png',
      'count': 0,
      'progress': 50,
      'countType': 'ServicesNew',
      requestType:ItRequestType['Services Inprogress'],
      title: 'In Progress'
    },
    {
      'image': 'assets/citizen-affair/close.png',
      'count': 0,
      'countType': 'ServicesClose',
      'progress': 50,
      requestType:ItRequestType['Services Closed'],
      title: 'Closed'

    }
  ];
  componentsCards = [
    {
      'image': 'assets/maintenance/tabs.png',
      'count': 0,
      'countType': 'ComponentsNew',
      'progress': 50,
      requestType:ItRequestType['Components New'],
      title: 'New'
    },
    {
      'image': 'assets/it-dashboard/inprogress.png',
      'count': 0,
      'countType': 'ComponentsNew',
      'progress': 50,
      requestType:ItRequestType['Components Inprogress'],
      title: 'In Progress'
    },
    {
      'image': 'assets/citizen-affair/close.png',
      'count': 0,
      'countType': 'ComponentsClose',
      'progress': 50,
      requestType:ItRequestType['Components Closed'],
      title: 'closed'
    }
  ];

  userCards = [
    {
      image: 'assets/hr-dashboard/inbox.png',
      title: 'IT Documents',
      pageLink: '/app/it/document'
    }
  ];

  commonRequests:any = {
    MyOwnRequests:{
      'image':'assets/employee-profile/passport.png',
      'count': 0,
      'progress': 0,
      countType:'MyOwnRequest',
      requestType:'MyOwnRequests',
      requestName:'My Own Requests',
      isHRDepartmentRequest:false,
      isReqSelected:false
    },
    MyClosedRequest:{
      'image':'assets/employee-profile/passport.png',
      'count': 0,
      'progress': 0,
      countType:'MyClosedRequest',
      requestType:'MyClosedRequest',
      requestName:'My Closed Request',
      isHRDepartmentRequest:false,
      isReqSelected:false
    }
  };

  reqTypes = [];

  priority = [
    {
      value: '',
      label: 'All'
    },
    {
      value: 'Low',
      label: 'Low'
    },
    {
      value: "High",
      label: 'High'
    }
  ];

  statusList:any = [];
  tableMessages: { emptyMessage: any; };

  ngOnInit() {
    this.tableMessages = {
      emptyMessage: (this.common.language == 'English') ? 'No data to display' : this.arabic.words['nodatatodisplay']
    };
    this.loadRequestList();
    if (this.lang === 'en') {
      this.common.breadscrumChange('IT', 'Home Page', '');
      this.common.topBanner(true, 'IT Dashboard', '+ CREATE REQUEST', '/en/app/it/it-request-create');
      this.supportCards = [
        {
          'image': 'assets/maintenance/tabs.png',
          'count': 0,
          'progress': 50,
          'countType': 'SupportNew',
          requestType:ItRequestType['Support New'],
          title: 'New'
        },
        {
          'image': 'assets/it-dashboard/inprogress.png',
          'count': 0,
          'progress': 50,
          'countType': 'SupportInprogress',
          requestType:ItRequestType['Support Inprogress'],
          title: 'In Progress'
        },
        {
          'image': 'assets/citizen-affair/close.png',
          'count': 0,
          'progress': 50,
          'countType': 'SupportClose',
          requestType:ItRequestType['Support Closed'],
          title: 'Closed'
        }
      ];
      this.servicesCards = [
        {
          'image': 'assets/maintenance/tabs.png',
          'count': 0,
          'progress': 50,
          'countType': 'ServicesNew',
          requestType:ItRequestType['Services New'],
          title: 'New'
        },
        {
          'image': 'assets/it-dashboard/inprogress.png',
          'count': 0,
          'progress': 50,
          'countType': 'ServicesInProgress',
          requestType:ItRequestType['Services Inprogress'],
          title: 'In Progress'
        },
        {
          'image': 'assets/citizen-affair/close.png',
          'count': 0,
          'countType': 'ServicesClose',
          'progress': 50,
          requestType:ItRequestType['Services Closed'],
          title: 'Closed'
    
        }
      ];
      this.componentsCards = [
        {
          'image': 'assets/maintenance/tabs.png',
          'count': 0,
          'countType': 'ComponentsNew',
          'progress': 50,
          requestType:ItRequestType['Components New'],
          title: 'New'
        },
        {
          'image': 'assets/it-dashboard/inprogress.png',
          'count': 0,
          'countType': 'ComponentsInprogress',
          'progress': 50,
          requestType:ItRequestType['Components Inprogress'],
          title: 'In Progress'
        },
        {
          'image': 'assets/citizen-affair/close.png',
          'count': 0,
          'countType': 'ComponentsClose',
          'progress': 50,
          requestType:ItRequestType['Components Closed'],
          title: 'closed'
        }
      ];
      this.userCards = [
        {
          image: 'assets/hr-dashboard/inbox.png',
          title: 'IT Documents',
          pageLink: '/'+this.common.currentLang+'/app/it/document'
        }
      ];
      this.priority = [
        {
          value: '',
          label: 'All'
        },
        {
          value: 'Low',
          label: 'Low'
        },
        {
          value: "High",
          label: 'High'
        }
      ];
      this.filterBy.Status ='All';
      this.filterBy.SourceOU ='All';
    } else {
      this.common.breadscrumChange(this.arabic.words['informationtechnology'], this.arabic.words['homepage'], '');
      this.common.topBanner(true, this.arabic.words['dashboard'], '+ '+this.arabic.words['createrequest'], '/ar/app/it/it-request-create');
      this.supportCards = [
        {
          'image': 'assets/maintenance/tabs.png',
          'count': 0,
          'progress': 50,
          'countType': 'SupportNew',
          requestType:ItRequestType['Support New'],
          title: this.arabic.words.new
        },
        {
          'image': 'assets/it-dashboard/inprogress.png',
          'count': 0,
          'progress': 50,
          'countType': 'SupportInprogress',
          requestType:ItRequestType['Support Inprogress'],
          title: this.arabic.words.inprogress
        },
        {
          'image': 'assets/citizen-affair/close.png',
          'count': 0,
          'progress': 50,
          'countType': 'SupportClose',
          requestType:ItRequestType['Support Closed'],
          title: this.arabic.words.closed
        }
      ];
      this.servicesCards = [
        {
          'image': 'assets/maintenance/tabs.png',
          'count': 0,
          'progress': 50,
          'countType': 'ServicesNew',
          requestType:ItRequestType['Services New'],
          title: this.arabic.words.new
        },
        {
          'image': 'assets/it-dashboard/inprogress.png',
          'count': 0,
          'progress': 50,
          'countType': 'ServicesInProgress',
          requestType:ItRequestType['Services Inprogress'],
          title: this.arabic.words.inprogress
        },
        {
          'image': 'assets/citizen-affair/close.png',
          'count': 0,
          'countType': 'ServicesClose',
          'progress': 50,
          requestType:ItRequestType['Services Closed'],
          title: this.arabic.words.closed

        }
      ];
      this.componentsCards = [
        {
          'image': 'assets/maintenance/tabs.png',
          'count': 0,
          'countType': 'ComponentsNew',
          'progress': 50,
          requestType:ItRequestType['Components New'],
          title: this.arabic.words.new
        },
        {
          'image': 'assets/it-dashboard/inprogress.png',
          'count': 0,
          'countType': 'ComponentsInprogress',
          'progress': 50,
          requestType:ItRequestType['Components Inprogress'],
          title: this.arabic.words.inprogress
        },
        {
          'image': 'assets/citizen-affair/close.png',
          'count': 0,
          'countType': 'ComponentsClose',
          'progress': 50,
          requestType:ItRequestType['Components Closed'],
          title: this.arabic.words.closed
        }
      ];
      this.userCards = [
        {
          image: 'assets/hr-dashboard/inbox.png',
          title: this.arabic.words.itdocuments,
          pageLink: '/'+this.common.currentLang+'/app/it/document'
        }
      ];
      this.priority = [
        {
          value: '',
          label: this.arabic.words.all
        },
        {
          value: 'Low',
          label: this.arabic.words.low
        },
        {
          value: "High",
          label: this.arabic.words.high
        }
      ];
      this.filterBy.Status =this.arabic.words.all;
      this.filterBy.SourceOU =this.arabic.words.all;
    }
    let th = this;
    let requestTypes = Object.keys(ItRequestType);
    if(this.lang == 'en'){
      this.reqTypes = [{"value":'0', "label": "All"}];
    }else{
      this.reqTypes = [{"value":'0', "label": this.arabic.words.all}];
    }
    Object.keys(ItRequestType).slice(requestTypes.length/2).forEach((type)=>{
      if(this.lang == 'en'){
        th.reqTypes.push({value:ItRequestType[type],label:type});
      }else{
        th.reqTypes.push({value:ItRequestType[type],label:this.arabic.words[this.removeWordSpaces(type).trim().toLowerCase()]});
      }
    });
    if(this.isItDepartmentHeadUserID || this.isItDepartmentTeamUserID){
      this.filterBy.RequestType = ItRequestType["Support New"];
    }else{
      this.filterBy.RequestType = '';
    }
    
    this.getItCount();
    this.loadItRequests();
    // this.itService.getById(0, 0).subscribe((res: any) => {
    //   if (res) {
    //     // this.statusList = res.M_LookupsList;
    //     // this.departmentList = res.OrganizationList;
    //     // this.departmentList = [{"value":'', "label": "All"}];
    //     // Object.keys(res.OrganizationList).slice(res.OrganizationList.length/2).forEach((type)=>{
    //     //   console.log("type", type)
    //     //   this.departmentList.push({value:res.OrganizationList[type],label:type});
    //     // });
    //     // console.log("this.departmentList", this.departmentList)
    //   }
    // });
  }

  loadRequestList() {
    this.columns = [
      { name: 'Ref ID', prop: 'ReferenceNumber' },
      { name: 'Source', prop: 'SourceOU' },
      { name: 'Subject', prop: 'Subject' },
      { name: 'Status', prop: 'Status' },
      { name: 'Request Date', cellTemplate: this.creationDateTemplate },
      { name: 'Request Type', prop: 'RequestType' },
      { name: 'Priority', prop: 'Priority', cellTemplate: this.priorityTemplate },
      { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
    ];
    this.arabicColumns = [
      { name: this.arabic.words.refid, prop: 'ReferenceNumber' },
      { name: this.arabic.words.source, prop: 'SourceOU' },
      { name: this.arabic.words.subject, prop: 'Subject' },
      { name: this.arabic.words.status, prop: 'Status' },
      { name: this.arabic.words.requestdate, cellTemplate: this.creationDateTemplate },
      { name: this.arabic.words.requesttype, prop: 'RequestType' },
      { name: this.arabic.words.priority, prop: 'Priority', cellTemplate: this.priorityTemplate },
      { name: this.arabic.words.action, prop: '', cellTemplate: this.actionTemplate },
    ];
    this.rows = [];
  }

  getItCount() {
    this.itService.getItDashboardCount(this.currentUser.id).subscribe((modCountRes: any) => {
      if(modCountRes){
        this.supportCards.forEach((cardObj)=>{
          if(modCountRes[cardObj.countType] && modCountRes[cardObj.countType] > 0){
              cardObj.count = modCountRes[cardObj.countType];
          }else{
            cardObj.count = 0;
          }
        });
        this.servicesCards.forEach((cardObj)=>{
          if(modCountRes[cardObj.countType] && modCountRes[cardObj.countType] > 0){
              cardObj.count = modCountRes[cardObj.countType];
          }else{
            cardObj.count = 0;
          }
        });
        this.componentsCards.forEach((cardObj)=>{
          if(modCountRes[cardObj.countType] && modCountRes[cardObj.countType] > 0){
              cardObj.count = modCountRes[cardObj.countType];
          }else{
            cardObj.count = 0;
          }
        });
        if(modCountRes[this.commonRequests.MyOwnRequests.countType] && modCountRes[this.commonRequests.MyOwnRequests.countType] > 0){
          this.commonRequests.MyOwnRequests.count = modCountRes[this.commonRequests.MyOwnRequests.countType];
        }else{
          this.commonRequests.MyOwnRequests.count = 0;
        }

        if(modCountRes[this.commonRequests.MyClosedRequest.countType] && modCountRes[this.commonRequests.MyClosedRequest.countType] > 0){
          this.commonRequests.MyClosedRequest.count = modCountRes[this.commonRequests.MyClosedRequest.countType];
        }else{
          this.commonRequests.MyClosedRequest.count = 0;
        }
      }
    });
  }

  reqSelect(OwnReq,myproc,requestType?:string){
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.filterBy = {
      Status:'',
      ReqDateFrom:null,
      ReqDateTo:null,
      SourceOU:null,
      Subject: null,
      Priority: ''
    };
    if(this.lang == 'en'){
      this.filterBy.SourceOU = 'All';
      this.filterBy.Status = 'All';
    }else{
      this.filterBy.SourceOU = this.arabic.words.all;
      this.filterBy.Status = this.arabic.words.all;
    }
    if(requestType){
      this.filterBy.RequestType = requestType;
    }else{
      this.filterBy.RequestType = '';
    }
    if(myproc) {
      this.commonRequests.MyOwnRequests.isReqSelected = false;
      this.commonRequests.MyClosedRequest.isReqSelected =true;
      this.loadItRequests();

    }
    if(!OwnReq && !myproc) {
      this.commonRequests.MyOwnRequests.isReqSelected = false;
      this.commonRequests.MyClosedRequest.isReqSelected =false;
      this.loadItRequests();
    }

    if(OwnReq) {
      this.commonRequests.MyOwnRequests.isReqSelected = true;
      this.commonRequests.MyClosedRequest.isReqSelected =false;
      this.filterBy.Creator = this.currentUser.username;
      this.loadItRequests();
    }
    // this.loadItRequests();
    this.itService.triggerScrollTo();
  }

  onChangePage(config,event){
    this.loadItRequests();
  }

  onSearch(){
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.loadItRequests();
  }

  loadItRequests(){
    let SmartSearch= '';
    let Subject = '';
    let Priority = null;
    let SourceOU = '';
    let RequestType = '';
    let ReqDateFrom= '';
    let ReqDateTo= '';
    let Type:any = '';
    let itstatus = ''
    if(this.filterBy.SmartSearch){
      SmartSearch = this.filterBy.SmartSearch;
    }
    if(this.filterBy.Subject){
      Subject = this.filterBy.Subject;
    }

    if(this.filterBy.Priority){
      Priority = this.filterBy.Priority == "Low" || this.filterBy.Priority == this.arabic.words.low ? 0 : 1;
    }
    if(this.filterBy.SourceOU && this.filterBy.SourceOU !='All' && this.filterBy.SourceOU != this.arabic.words.all){
      SourceOU = this.filterBy.SourceOU;
    }
    if(this.filterBy.Status && this.filterBy.Status != 'All' && this.filterBy.Status != this.arabic.words.all){
      itstatus = this.filterBy.Status;
    }
    if(this.filterBy.RequestType){
      RequestType = this.filterBy.RequestType;
    }
    if(this.commonRequests.MyClosedRequest.isReqSelected || this.commonRequests.MyOwnRequests.isReqSelected){       
      if(this.commonRequests.MyClosedRequest.isReqSelected){
        Type = 1;
      }
      if(this.commonRequests.MyOwnRequests.isReqSelected){
        Type = '0';
      }
    }
    
    if(this.filterBy.ReqDateFrom){
      ReqDateFrom = new Date(this.filterBy.ReqDateFrom).toJSON();
    }

    if(this.filterBy.ReqDateTo){
      ReqDateTo = new Date(this.filterBy.ReqDateTo).toJSON();
    }
    Priority = Priority == null ? '' : Priority;
    this.itService.getItRequestList(this.config.page, this.config.pageSize, RequestType, this.currentUser.id, this.currentUser.username, itstatus, SmartSearch, ReqDateFrom, ReqDateTo, SourceOU, Subject, Priority,Type ).subscribe((allOwnRes:any) => {
      if(allOwnRes){
        this.rows = allOwnRes.Collection;
        this.config.totalItems = allOwnRes.Count;
        this.departmentList = allOwnRes.OrganizationList;
        this.statusList = allOwnRes.M_LookupsList;
      }
    });
  }

  openReport() {
    let initialState = {};
    this.bsModalRef = this.modalService.show(ItReportModalComponent,{class:'modal-lg'});
  }

  viewData(value){
    this.router.navigate(['/'+this.lang+'/app/it/it-request-view/' + value.RequestID]);
  }

  removeWordSpaces(words:string){
    return  words.replace(/\s+/g, '');
  }
}
