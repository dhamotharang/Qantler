import { Component, OnInit, ViewChild, TemplateRef,HostListener, ElementRef, Renderer2, Inject } from '@angular/core';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import Chart from 'chart.js';
import { CommonService } from '../../../common.service';
import { DatePipe, DOCUMENT } from '@angular/common';
import { Router } from '@angular/router';
import { MediaService } from '../../service/media.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { UtilsService } from 'src/app/shared/service/utils.service';

@Component({
  selector: 'app-media-protocol-requests',
  templateUrl: './media-protocol-requests.component.html',
  styleUrls: ['./media-protocol-requests.component.scss']
})
export class MediaProtocolRequestsComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('pieChart') pieChart: ElementRef;
  @ViewChild('barChart') barChart: ElementRef;
  @ViewChild('dateColumn') dateColumn: TemplateRef<any>;
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  bsConfig1: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  public rows:Array<any> = [];
  public columns:Array<any> = [];
  public page:number = 1;
  public itemsPerPage:number = 10;
  public maxSize:number = 10;
  public numPages:number = 1;
  public length:number = 0;
  public processedreq:boolean=false;
  public alreadyExist:boolean=false;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  public config:any = {
    paging: true,
    sorting: {columns: this.columns},
    filtering: {filterString: ''},
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  private data:any;
  filter_data:any=[];
  media_id ='';
  filter = false;
  status:any = '';
  statusM:any = '';
  source = '';
  destination = '';
  date_from:any = '';
  date_fromM:any = '';
  date_to:any = '';
  date_toM:any = '';
  private = '';
  priority = '';
  smartSearch = '';
  smartSearchM = '';
  statusOptions:any = [
    // {'DisplayName': "All", 'value': ""},
    // {'DisplayName': "Under Process", 'value': "Under Process"},
    // {'DisplayName': "Pending for Resubmission", 'value': "Pending for Resubmission"},
    // {'DisplayName': "Closed", 'value': "Closed"},
    // // {'DisplayName': "Draft", 'value': "Draft"},
    // {'DisplayName': "Waiting for Approval", 'value': "Waiting for Approval"},
    // {'DisplayName': "Rejected", 'value': "Rejected"}
  ];
  userList =[];
  sourceouOptions:any= [];
  destinationOptions = [];
  reqtype:any = '';
  reqtypeM:any = '';
  mediaList:any=[];
  SourceOU = 'All';
  SourceOUM = 'All';
  UserID: any;
  dashBoard: { name: string; count: any; }[];
  cardDetails: any=[];
  RequestTypes: any=[];
  OrgUnitID: any;
  isProtocolDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 4;
  isProtocolDepartmentTeamUserID = this.currentUser.OrgUnitID == 4 && !this.currentUser.IsOrgHead;
  isMediaDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 17;
  isMediaDepartmentTeamUserID = this.currentUser.OrgUnitID == 17 && !this.currentUser.IsOrgHead;
  cardDetailsList = [
    {
      'name': 'All',
      'reqtype': '',
    },
    {
      'name': 'Request For Photo',
      'reqtype': 1,
      'viewLink': this.common.currentLang+'/app/media/media-request-photo/view/',
      'editLink': this.common.currentLang+'/app/media/media-request-photo/edit/'
    },
    {
      'name': 'Request For Design',
      'reqtype': 2,
      'viewLink': this.common.currentLang+'/app/media/media-request-design/',
      'editLink': this.common.currentLang+'/app/media/media-request-design/edit/'
    },
    {
      'name': 'Request For PressRelease',
      'reqtype': 3,
      'viewLink':this.common.currentLang+'/app/media/media-press-release/view/',
      'editLink':this.common.currentLang+'/app/media/media-press-release/view/',
    },
    {
      'name': 'Request For Campaign',
      'reqtype': 4,
      'viewLink':this.common.currentLang+'/app/media/campaign/view/',
      'editLink':this.common.currentLang+'/app/media/campaign/edit/'
    },
    {
      'name': 'Request For Photographer',
      'reqtype': 5,
      'viewLink':this.common.currentLang+'/app/media/photographer/view/',
      'editLink':this.common.currentLang+'/app/media/photographer/edit/'
    },
    {
      'name': 'Request For Diwan Identity',
      'viewLink':this.common.currentLang+'/app/media/diwan-identity/request-view/',
      'editLink':this.common.currentLang+'/app/media/diwan-identity/request-view/',
      'reqtype': 6
    },
    // { 'name': 'My Own Requests', 'reqtype': 7,},
    // { 'name': 'My Pending Requests', 'reqtype': 0,}
  ];
  progress = false;
  requestName: string;
  @ViewChild('template') template : TemplateRef<any>;
  bsModalRef: BsModalRef;
  ownRequestsList: any=[];
  pendingRequests: any=[];
  processedRequests: any=[];
  ownRequests: boolean = false;
  ownreqType: any;
  lang: any;
  tableMessages: { emptyMessage: any; };
  validateModalStartEndDate: any = {
    isValid:true,
    msg:''
  };
  validateStartEndDate: any = {
    isValid:true,
    msg:''
  };

  constructor(public common: CommonService,
    public router:Router,
    private service: MediaService,
    public datePipe:DatePipe,
    public modalService: BsModalService,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document,
    private utilsService:UtilsService) {
     this.common.breadscrumChange('Protocol','Media Requests','');
     this.common.topBanner(true,'Protocol Dashboard','+ CREATE REQUEST','media-request-list');
     if(this.common.currentLang == 'ar'){
      this.common.breadscrumChange(this.arabic('protocol'),this.arabic('mediarequests'),'');
      this.common.topBanner(true,this.arabic('dashboard'),'+  ' + this.arabic(('CREATE REQUEST').replace(/ +/g, "").toLowerCase()),'media-request-list');
     }
     this.UserID = this.currentUser.id;
     this.OrgUnitID = this.currentUser.OrgUnitID;

     if(!this.isProtocolDepartmentHeadUserID && !this.isProtocolDepartmentTeamUserID && !this.isMediaDepartmentHeadUserID && !this.isMediaDepartmentTeamUserID) {
      this.reqtype = 0;
      this.ownRequests = false;
      this.ownreqType = 7;
     }else{
      this.reqtype = 1;
     }
  }

  ngOnInit() {
    //this.bsConfig1 = Object.assign({}, { adaptivePosition: true });
    this.lang = this.common.currentLang;
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabic('nodatatodisplay')
    };
    this.getMediaListCount();
    this.changeList(this.reqtype);
    if(this.common.currentLang == 'en'){
      if(this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID){
        this.columns = [
          { name: 'Ref ID', prop: 'RefID' },
          { name: 'Source', prop: 'Source' },
          { name: 'Status', prop: 'Status' },
          { name: 'Assigned To', prop: 'AssignedTo' },
          { name: 'Request Type', prop: 'RequestType' },
          { name: 'Request Date', prop: 'RequestDate', cellTemplate: this.dateColumn },
          { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
        ];

      }else{
        this.columns = [
          { name: 'Ref ID', prop: 'RefID' },
          { name: 'Source', prop: 'Source' },
          { name: 'Status', prop: 'Status' },
          { name: 'Request Type', prop: 'RequestType' },
          // { name:  'Assigned To', prop:'AssignedTo' },
          { name: 'Request Date', prop: 'RequestDate', cellTemplate: this.dateColumn },
          { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
        ];

      }
      this.status = 'All';
      this.statusM = 'All';
      this.reqtype = '';
      this.reqtypeM = '';
      this.SourceOU = 'All';
      this.SourceOUM = 'All';

      // this.statusOptions = [
      //   {'DisplayName': "All", 'value': ""},
      //   {'DisplayName': "Under Process", 'value': "Under Process"},
      //   {'DisplayName': "Pending for Resubmission", 'value': "Pending for Resubmission"},
      //   {'DisplayName': "Closed", 'value': "Closed"},
      //   // {'DisplayName': "Draft", 'value': "Draft"},
      //   {'DisplayName': "Waiting for Approval", 'value': "Waiting for Approval"},
      //   {'DisplayName': "Rejected", 'value': "Rejected"}
      // ];

      this.cardDetailsList = [
        {
          'name': 'All',
          'reqtype': '',
        },
        {
          'name': 'Request For Photo',
          'reqtype': 1,
          'viewLink': this.common.currentLang+'/app/media/media-request-photo/view/',
          'editLink': this.common.currentLang+'/app/media/media-request-photo/edit/'
        },
        {
          'name': 'Request For Design',
          'reqtype': 2,
          'viewLink': this.common.currentLang+'/app/media/media-request-design/',
          'editLink': this.common.currentLang+'/app/media/media-request-design/edit/'
        },
        {
          'name': 'Request For PressRelease',
          'reqtype': 3,
          'viewLink':this.common.currentLang+'/app/media/media-press-release/view/',
          'editLink':this.common.currentLang+'/app/media/media-press-release/edit/',
        },
        {
          'name': 'Request For Campaign',
          'reqtype': 4,
          'viewLink':this.common.currentLang+'/app/media/campaign/view/',
          'editLink':this.common.currentLang+'/app/media/campaign/edit/'
        },
        {
          'name': 'Request For Photographer',
          'reqtype': 5,
          'viewLink':this.common.currentLang+'/app/media/photographer/view/',
          'editLink':this.common.currentLang+'/app/media/photographer/edit/'
        },
        {
          'name': 'Request For Diwan Identity',
          'viewLink':this.common.currentLang+'/app/media/diwan-identity/request-view/',
          'editLink':this.common.currentLang+'/app/media/diwan-identity/request-view/',
          'reqtype': 6
        },
        // { 'name': 'My Own Requests', 'reqtype': 7,},
        // { 'name': 'My Pending Requests', 'reqtype': 0,}
      ];
    }else{
      if(this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID){
        this.columns = [
          { name: this.arabic(('Ref ID').replace(/ +/g, "").toLowerCase()), prop: 'RefID' },
          { name: this.arabic(('Source').replace(/ +/g, "").toLowerCase()), prop: 'Source' },
          { name: this.arabic(('Status').replace(/ +/g, "").toLowerCase()), prop: 'Status' },
          { name: this.arabic(('Request Type').replace(/ +/g, "").toLowerCase()), prop: 'RequestType' },
          { name: this.arabic('assignedto'), prop:'AssignedTo' },
          { name: this.arabic(('Request Date').replace(/ +/g, "").toLowerCase()), prop: 'RequestDate', cellTemplate: this.dateColumn },
          { name: this.arabic(('Action').replace(/ +/g, "").toLowerCase()), prop: '', cellTemplate: this.actionTemplate },
        ];
      }else{
        this.columns = [
          { name: this.arabic(('Ref ID').replace(/ +/g, "").toLowerCase()), prop: 'RefID' },
          { name: this.arabic(('Source').replace(/ +/g, "").toLowerCase()), prop: 'Source' },
          { name: this.arabic(('Status').replace(/ +/g, "").toLowerCase()), prop: 'Status' },
          { name: this.arabic(('Request Type').replace(/ +/g, "").toLowerCase()), prop: 'RequestType' },
          { name: this.arabic(('Request Date').replace(/ +/g, "").toLowerCase()), prop: 'RequestDate', cellTemplate: this.dateColumn },
          { name: this.arabic(('Action').replace(/ +/g, "").toLowerCase()), prop: '', cellTemplate: this.actionTemplate },
        ];
      }
      this.status = this.arabic('all');
      this.statusM = this.arabic('all');
      this.reqtype = '';
      this.reqtypeM = '';
      this.SourceOU = this.arabic('all');
      this.SourceOUM = this.arabic('all');
      this.cardDetailsList = [
        {
          'name': this.arabic(('All').replace(/ +/g, "").toLowerCase()),
          'reqtype': '',
        },
        {
          'name': this.arabic(('Request For Photo').replace(/ +/g, "").toLowerCase()),
          'reqtype': 1,
          'viewLink': this.common.currentLang+'/app/media/media-request-photo/view/',
          'editLink': this.common.currentLang+'/app/media/media-request-photo/edit/'
        },
        {
          'name': this.arabic(('Request For Design').replace(/ +/g, "").toLowerCase()),
          'reqtype': 2,
          'viewLink': this.common.currentLang+'/app/media/media-request-design/',
          'editLink': this.common.currentLang+'/app/media/media-request-design/edit/'
        },
        {
          'name': this.arabic(('Request For PressRelease').replace(/ +/g, "").toLowerCase()),
          'reqtype': 3,
          'viewLink':this.common.currentLang+'/app/media/media-press-release/view/',
          'editLink':this.common.currentLang+'/app/media/media-press-release/edit/',
        },
        {
          'name': this.arabic(('Request For Campaign').replace(/ +/g, "").toLowerCase()),
          'reqtype': 4,
          'viewLink':this.common.currentLang+'/app/media/campaign/view/',
          'editLink':this.common.currentLang+'/app/media/campaign/edit/'
        },
        {
          'name': this.arabic(('Request For Photographer').replace(/ +/g, "").toLowerCase()),
          'reqtype': 5,
          'viewLink':this.common.currentLang+'/app/media/photographer/view/',
          'editLink':this.common.currentLang+'/app/media/photographer/edit/'
        },
        {
          'name': this.arabic(('Request For Diwan Identity').replace(/ +/g, "").toLowerCase()),
          'viewLink':this.common.currentLang+'/app/media/diwan-identity/request-view/',
          'editLink':this.common.currentLang+'/app/media/diwan-identity/request-view/',
          'reqtype': 6
        },
        // { 'name': 'My Own Requests', 'reqtype': 7,},
        // { 'name': 'My Pending Requests', 'reqtype': 0,}
      ];
      // this.statusOptions = [
      //   {'DisplayName': this.arabic(("All").replace(/ +/g, "").toLowerCase()), 'value': ""},
      //   {'DisplayName': this.arabic(("Under Process").replace(/ +/g, "").toLowerCase()), 'value': this.arabic(("Under Process").replace(/ +/g, "").toLowerCase())},
      //   {'DisplayName': this.arabic(("Pending for Resubmission").replace(/ +/g, "").toLowerCase()), 'value': this.arabic(("Pending for Resubmission").replace(/ +/g, "").toLowerCase())},
      //   {'DisplayName': this.arabic(("Closed").replace(/ +/g, "").toLowerCase()), 'value': this.arabic(("Closed").replace(/ +/g, "").toLowerCase())},
      //   // {'DisplayName': this.arabic(("Draft").replace(/ +/g, "").toLowerCase()), 'value': this.arabic(("Draft").replace(/ +/g, "").toLowerCase())},
      //   {'DisplayName': this.arabic(("Waiting for Approval").replace(/ +/g, "").toLowerCase()), 'value': this.arabic(("Waiting for Approval").replace(/ +/g, "").toLowerCase())},
      //   {'DisplayName': this.arabic(("Rejected").replace(/ +/g, "").toLowerCase()), 'value': this.arabic(("Rejected").replace(/ +/g, "").toLowerCase())}
      // ];
    }


  }

  commonRequestList(ownreqType, cond){
    this.checkStartEndDiff();
    if(this.validateStartEndDate.isValid){
      this.requestName = '';
      this.ownreqType = ownreqType;
      this.ownRequests = true;
      if(cond == false){
        this.reqtype = '';
        this.getRequestName(this.reqtype);
      }else {
        this.getRequestName(this.reqtype);
        // this.reqtype = null;
      }
      this.progress = true;
      let SourceOU = '';
      let status = '';
      let ReqDateFrom = '';
      let ReqDateTo = '';
      let smartSearch = '';
      let requestName= '';

      if(this.date_from){
        ReqDateFrom = new Date(this.date_from).toJSON();
      }
      if(this.date_to){
        ReqDateTo = new Date(this.date_to).toJSON();
      }
      if (this.status) {
        status = this.status;
      }

      if(this.status == 'All' || this.status == this.arabic('all')){
        status = '';
      }

      if(!this.status){
        this.status = 'All';
        if(this.lang !='en'){
          this.status = this.arabic('all');
        }
      }

      if (this.SourceOU && this.SourceOU != 'All' && this.SourceOU != this.arabic('all')) {
        SourceOU = this.SourceOU;
        SourceOU = SourceOU.replace("&", "amp;");
      }

      if(!this.SourceOU){
        this.SourceOU = 'All';
        if(this.lang !='en'){
          this.SourceOU = this.arabic('all');
        }
      }

      if (this.smartSearch) {
        smartSearch = this.smartSearch;
      }
      if(this.requestName && (this.requestName !='All') && (this.requestName !=this.arabic('all'))){
        requestName = this.requestName;
      }
      if(!this.reqtype){
        this.reqtype = '';
      }

      this.callMediaList(ownreqType, status, smartSearch, ReqDateFrom, ReqDateTo, SourceOU, requestName);
      this.service.triggerScrollTo();
    }
  }

  async changeList(type,allowScroll?){
    // this.reqtype = null;
    this.checkStartEndDiff();
    if(this.validateStartEndDate.isValid){
      this.requestName = '';
      if(type == 7){
        this.reqtype = type;
        this.ownRequests = true;
      }else{
        this.ownRequests = false;
      }
      this.getRequestName(this.reqtype);
      this.progress = true;
      // this.reqtype = 0;
      let SourceOU = '';
      let status = '';
      let ReqDateFrom = '';
      let ReqDateTo = '';
      let smartSearch = '';
      let requestName= '';

      if(this.date_from){
        ReqDateFrom = new Date(this.date_from).toJSON();
      }
      if(this.date_to){
        ReqDateTo = new Date(this.date_to).toJSON();
      }
      if (this.status) {
        status = this.status;
      }

      if(this.status == 'All' || this.status == this.arabic('all')){
        status = '';
      }

      if(!this.status){
        this.status = 'All';
        if(this.lang !='en'){
          this.status = this.arabic('all');
        }
      }

      if (this.SourceOU && this.SourceOU != 'All' && this.SourceOU != this.arabic('all')) {
        SourceOU = this.SourceOU;
        SourceOU = SourceOU.replace("&", "amp;");
      }

      if(!this.status){
        this.status = 'All';
        if(this.lang !='en'){
          this.status = this.arabic('all');
        }
      }

      if(!this.SourceOU){
        this.SourceOU = 'All';
        if(this.lang !='en'){
          this.SourceOU = this.arabic('all');
        }
      }

      if (type) {
        this.reqtype = type;
      }else{
        this.reqtype = '';
      }
      if (this.smartSearch) {
        smartSearch = this.smartSearch;
      }
      if(this.requestName && (this.requestName !='All') && (this.requestName !=this.arabic('all'))){
        requestName = this.requestName
      }
      this.callMediaList(this.reqtype, status, smartSearch, ReqDateFrom, ReqDateTo, SourceOU, requestName);
      if(allowScroll){
        this.service.triggerScrollTo();
      }
    }
  }

  callMediaList(reqtype, status, smartSearch, ReqDateFrom, ReqDateTo, SourceOU, requestName){
    this.checkStartEndDiff();
    if(this.validateStartEndDate.isValid){
      if(reqtype== null){
        reqtype = '';
      }

      if(SourceOU == "All" || SourceOU == this.arabic('all')){
        SourceOU = '';
      }
      SourceOU = SourceOU.replace("&", "amp;");
      this.processedreq=false;

      this.service.getMediaList(this.page, this.maxSize, reqtype, this.UserID, status, smartSearch, ReqDateFrom, ReqDateTo, SourceOU, requestName).subscribe((data:any) => {
        if(reqtype==8)
        {
        this.processedreq=true;
        }
        if(reqtype == 1){
          this.reqtype = 1;
        }
        this.progress = false;
        this.mediaList = data;
        this.rows = this.mediaList.Collection;
        this.statusOptions = data.M_LookupsList;
        this.statusOptions = this.statusOptions.filter(list => list.LookupsID != 67);
        this.sourceouOptions = data.OrganizationList;
        this.config.totalItems = this.mediaList.Count;
        this.setRequestType();
        this.getRequestName(this.reqtype);

      });
    }
  }

  getRequestName(reqtype){
    this.requestName = '';
    this.cardDetailsList.forEach((cardObj)=>{
      if((cardObj.name !='All' && cardObj.name != this.arabic('all')) && (cardObj.reqtype == reqtype)){
        this.requestName = cardObj.name;
      }else{
        if(this.reqtype == 7 || this.reqtype == 0){
          //this.reqtype='';
        }
      }
    });
  }

  async getMediaListCount() {
    await this.service.getMediaListCount(this.currentUser.id).subscribe(data => {
      const mapped = Object.keys(data).map(key => ({name: key, count: data[key]}));
      this.dashBoard = mapped;
      for(var i = 0; i < this.dashBoard.length; i++) {
        if(this.common.currentLang == 'en'){
          if(this.dashBoard[i].name == "RequestforPhoto" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/image.png',
                'count': this.dashBoard[i].count,
                'name': 'Request For Photo',
                'reqtype': 1,
                'orderBy': 1,
                'viewLink': this.common.currentLang+'/app/media/media-request-photo/',
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "RequestforDesign" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/sketch.png',
                'count': this.dashBoard[i].count,
                'name': 'Request For Design',
                'reqtype': 2,
                'orderBy': 2,
                'viewLink': this.common.currentLang+'/app/media/media-request-design/',
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "RequestforPressRelease" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/newspaper-folded.png',
                'count': this.dashBoard[i].count,
                'name': 'Request For Press Release',
                'reqtype': 3,
                'orderBy': 3,
                'viewLink':this.common.currentLang+'/app/media/media-press-release/view/',
                'progress': 50 })
          }

          if(this.dashBoard[i].name == "RequestforCampaign" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/professional-condenser-microphone-outline.png',
                'count': this.dashBoard[i].count,
                'name': 'Request For Campaign',
                'viewLink':this.common.currentLang+'/app/media/campaign/view/',
                'reqtype': 4,
                'orderBy': 4,
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "RequestforPhotographer" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/photo-camera.png',
                'count': this.dashBoard[i].count,
                'name': 'Request For Photographer',
                'viewLink':this.common.currentLang+'/app/media/photographer/view/',
                'reqtype': 5,
                'orderBy': 5,
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "RequesttouseDiwanIdentity" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/name.png',
                'count': this.dashBoard[i].count,
                'name': 'Request For Diwan Identity',
                'viewLink':this.common.currentLang+'/app/media/diwan-identity/view/',
                'reqtype': 6,
                'orderBy': 6,
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "MyOwnRequest" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.ownRequestsList.push({
                'image': 'assets/employee-profile/passport.png',
                'count': this.dashBoard[i].count,
                'name': 'My Own Requests',
                'reqtype': 7,
                'orderBy': 7,
                'progress': 50,
                'Reqname' :'',
                'isMyOwnReSelected': true
            })
          }
          if(this.dashBoard[i].name == "MyOwnRequest" && !((this.currentUser.department == 'Protocol' && (this.currentUser.isOrgHead)) || this.currentUser.department == 'Media'))
          {
            this.ownRequestsList.push({
              'image': 'assets/employee-profile/passport.png',
              'count': this.dashBoard[i].count,
              'name': 'My Own Requests',
              'reqtype': 7,
              'orderBy': 7,
              'progress': 50,
              'Reqname' :'DisplayChange',
              'isMyOwnReSelected': true
          })
          }
          if(this.dashBoard[i].name == "MyPendingRequest" && !((this.currentUser.department == 'Protocol' && (this.currentUser.IsOrgHead)) || this.currentUser.department == 'Media')) {
            this.pendingRequests.push({
                'image': 'assets/employee-profile/passport.png',
                'count': this.dashBoard[i].count,
                'name': 'My Pending Actions',
                'reqtype': 0,
                'orderBy': 8,
                'progress': 50,
                'isMyProcessSelected': true
            })
          }
          if(this.dashBoard[i].name == "MyProcessedRequest" && !((this.currentUser.department == 'Protocol' && (this.currentUser.IsOrgHead)) || this.currentUser.department == 'Media')) {
            this.processedRequests.push({
                'image': 'assets/employee-profile/passport.png',
                'count': this.dashBoard[i].count,
                'name': 'My Processed Request',
                'reqtype': 8,
                'orderBy': 8,
                'progress': 50,
                'isMyPenReSelected': true
            })
          }
        }else if(this.common.currentLang == 'ar'){
          if(this.dashBoard[i].name == "RequestforPhoto" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/image.png',
                'count': this.dashBoard[i].count,
                'name': this.arabic(('Request For Photo').replace(/ +/g, "").toLowerCase()),
                'reqtype': 1,
                'orderBy': 1,
                'viewLink': this.common.currentLang+'/app/media/media-request-photo/',
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "RequestforDesign" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/sketch.png',
                'count': this.dashBoard[i].count,
                'name': this.arabic(('Request For Design').replace(/ +/g, "").toLowerCase()),
                'reqtype': 2,
                'orderBy': 2,
                'viewLink': this.common.currentLang+'/app/media/media-request-design/',
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "RequestforPressRelease" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/newspaper-folded.png',
                'count': this.dashBoard[i].count,
                'name': this.arabic(('Request For Press Release').replace(/ +/g, "").toLowerCase()),
                'reqtype': 3,
                'orderBy': 3,
                'viewLink':this.common.currentLang+'/app/media/media-press-release/view/',
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "RequestforCampaign" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/professional-condenser-microphone-outline.png',
                'count': this.dashBoard[i].count,
                'name': this.arabic(('Request For Campaign').replace(/ +/g, "").toLowerCase()),
                'viewLink':this.common.currentLang+'/app/media/campaign/view/',
                'reqtype': 4,
                'orderBy': 4,
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "RequestforPhotographer" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/photo-camera.png',
                'count': this.dashBoard[i].count,
                'name': this.arabic(('Request For Photographer').replace(/ +/g, "").toLowerCase()),
                'viewLink':this.common.currentLang+'/app/media/photographer/view/',
                'reqtype': 5,
                'orderBy': 5,
                'progress': 50 })
          }
          if(this.dashBoard[i].name == "RequesttouseDiwanIdentity" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.cardDetails.push({
                'image': 'assets/media/name.png',
                'count': this.dashBoard[i].count,
                'name': this.arabic(('Request For Diwan Identity').replace(/ +/g, "").toLowerCase()),
                'viewLink':this.common.currentLang+'/app/media/diwan-identity/view/',
                'reqtype': 6,
                'orderBy': 6,
                'progress': 50 })
          }


          if(this.dashBoard[i].name == "MyOwnRequest" && (this.isProtocolDepartmentHeadUserID || this.isMediaDepartmentHeadUserID || this.isMediaDepartmentTeamUserID)) {
            this.ownRequestsList.push({
                'image': 'assets/employee-profile/passport.png',
                'count': this.dashBoard[i].count,
                'name': this.arabic('myownrequests'),
                'reqtype': 7,
                'orderBy': 7,
                'progress': 50,
                'Reqname' :'',
                'isMyOwnReSelected': true
            })
          }
          if(this.dashBoard[i].name == "MyOwnRequest" && !((this.currentUser.department == 'Protocol' && (this.currentUser.isOrgHead)) || this.currentUser.department == 'Media'))
          {
            this.ownRequestsList.push({
              'image': 'assets/employee-profile/passport.png',
              'count': this.dashBoard[i].count,
              'name': this.arabic('myownrequests'),
              'reqtype': 7,
              'orderBy': 7,
              'progress': 50,
              'Reqname' :'DisplayChange',
              'isMyOwnReSelected': true
            })
          }
          if(this.dashBoard[i].name == "MyPendingRequest" && !((this.currentUser.department == 'Protocol' && (this.currentUser.IsOrgHead)) || this.currentUser.department == 'Media')) {
            this.pendingRequests.push({
                'image': 'assets/employee-profile/passport.png',
                'count': this.dashBoard[i].count,
                'name': this.arabic(('My Pending Actions').replace(/ +/g, "").toLowerCase()),
                'reqtype': 0,
                'orderBy': 8,
                'progress': 50,
                'isMyPenReSelected': true
            })
          }
          if(this.dashBoard[i].name == "MyProcessedRequest" && !((this.currentUser.department == 'Protocol' && (this.currentUser.IsOrgHead)) || this.currentUser.department == 'Media')) {
            this.processedRequests.push({
                'image': 'assets/employee-profile/passport.png',
                'count': this.dashBoard[i].count,
                'name': this.arabic(('My Processed Request').replace(/ +/g, "").toLowerCase()),
                'reqtype': 8,
                'orderBy': 8,
                'progress': 50,
                'isprocessedReSelected': true
            })
          }
        }

      }
      this.cardDetails.sort(function(a, b) {
        return parseFloat(a.orderBy) - parseFloat(b.orderBy);
      });
    });
  }

  setRequestType(){
    for(var i = 0; i< this.cardDetails.length; i++){
      this.RequestTypes.push({value: this.cardDetails[i].reqtype, label: this.cardDetails[i].name});
    }
  }

  goToDocuments(){
    this.router.navigate(['/'+this.common.currentLang+'/app/media/documents']);
  }

  public onChangePage(config:any, page:any = {page: this.page, itemsPerPage: this.itemsPerPage}):any {
    this.page = page;
    if(this.ownRequests) {
      // this.reqtype = this.ownreqType;
      this.commonRequestList(this.ownreqType, true);
    }else{
      this.reqtype = this.reqtype;
      this.changeList(this.reqtype);
    }
  }

  viewData(value, type, reqtype){
    let viewRouterUrl='';
    this.cardDetailsList.forEach((cardObj)=>{
      if(cardObj.name == value.RequestType && type == 'view'){
        viewRouterUrl = cardObj.viewLink;
      }
      if(cardObj.name == value.RequestType && type == 'edit'){
        viewRouterUrl = cardObj.editLink;
      }
    });
    if(viewRouterUrl != ''){
      this.router.navigate([viewRouterUrl + value.RequestID]);
    }
  }


  openReport(){
    this.validateModalStartEndDate.msg = '';
    this.bsModalRef = this.modalService.show(this.template,{class:'modal-lg'});
    this.initPage();
  }


  downloadDownload(){
    this.checkModalStartEndDiff();
    if(this.validateModalStartEndDate.isValid){
      if(!this.reqtypeM){
        this.reqtypeM = '';
      }
    let reportModel:any =  {
        requestType: this.reqtypeM,
        status: this.statusM,
        sourceOU: this.SourceOUM,
        smartSearch: this.smartSearchM,
        userID: this.currentUser.id,
        reqDateFrom: this.date_fromM,
        reqDateTo: this.date_toM
      };
      if(!this.SourceOUM || this.SourceOUM == 'All' || this.SourceOUM == this.arabic('all')){
        reportModel.sourceOU = '';
      }
      if(!this.SourceOUM){
        this.SourceOUM = 'All';
        if(this.lang !='en'){
          this.SourceOUM = this.arabic('all');
        }
      }
      if(!this.statusM || this.statusM == 'All' || this.statusM == this.arabic('all')){
        reportModel.status = '';
      }
      if(!this.statusM){
        this.statusM = 'All';
        if(this.lang !='en'){
          this.statusM = this.arabic('all');
        }
      }
      reportModel.sourceOU = reportModel.sourceOU.replace("&", "amp;");
      let date = new Date,
      cur_date = date.getDate() +'-'+(date.getMonth()+1)+'-'+date.getFullYear();
      this.service.getExport(reportModel)
      .subscribe((resultBlob: Blob)=>{
        this.initPage();
        var url = window.URL.createObjectURL(resultBlob);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = 'Media_Protocol_Report_'+cur_date+'.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
        this.bsModalRef.hide();
      });
    }
  }

  initPage(){
    if(this.common.currentLang == 'en'){
      this.reqtypeM = '';
      this.statusM = 'All';
      this.SourceOUM= 'All';
      this.smartSearchM= '';
      this.date_fromM= '';
      this.date_toM='';
    }else{
      this.reqtypeM = '';
      this.statusM = this.arabic('all');
      this.SourceOUM= this.arabic('all');
      this.smartSearchM= '';
      this.date_fromM= '';
      this.date_toM='';
    }
  }

  closemodal(){
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  dateChange(eve,isStartEnd){
    if(eve){
      if(isStartEnd == 'start'){
        this.date_from = eve;
      }
      if(isStartEnd == 'end'){
        this.date_to = eve;
      }      
    }else{
      if(isStartEnd == 'start'){
        this.date_from = '';
      }
      if(isStartEnd == 'end'){
        this.date_to = '';
      }
    }
    this.checkStartEndDiff();
  }

  modalDateChange(eve,isStartEnd){
    if(eve){
      if(isStartEnd == 'start'){
        this.date_fromM = eve;
      }
      if(isStartEnd == 'end'){
        this.date_toM = eve;
      }      
    }else{
      if(isStartEnd == 'start'){
        this.date_fromM = '';
      }
      if(isStartEnd == 'end'){
        this.date_toM = '';
      }
    }
    this.checkModalStartEndDiff();
  }

  checkStartEndDiff(){
    let toRetVal = true;
    if(this.utilsService.isValidDate(this.date_from)
    && this.utilsService.isValidDate(this.date_to)){
      if(this.date_from.getTime() > this.date_to.getTime()){
        toRetVal =  false;
      }else{
        toRetVal = true;
      }
    }else{
      if(!this.utilsService.isDate(this.date_from) || !this.utilsService.isDate(this.date_to)){
        toRetVal = false;
      }else{
        toRetVal = true;
      }
    }
    
    this.validateStartEndDate.isValid = toRetVal;
    if(this.validateStartEndDate.isValid){
      this.validateStartEndDate.msg = '';
    }else{
      this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date': this.arabic('errormsgvalidenddate');
    }
  }

  checkModalStartEndDiff(){
    let toRetVal = true;
    if(this.utilsService.isValidDate(this.date_fromM)
    && this.utilsService.isValidDate(this.date_toM)){
      if(this.date_fromM.getTime() > this.date_toM.getTime()){
        toRetVal =  false;
      }else{
        toRetVal = true;
      }
    }else{
      if(!this.utilsService.isDate(this.date_fromM) || !this.utilsService.isDate(this.date_toM)){
        toRetVal = false;
      }else{
        toRetVal = true;
      }
    }
    this.validateModalStartEndDate.isValid = toRetVal;
    if(this.validateModalStartEndDate.isValid){
      this.validateModalStartEndDate.msg = '';
    }else{
      this.validateModalStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date': this.arabic('errormsgvalidenddate');
    }
  }

  reqSelect(reqType,isTrue,isUserCard){
    this.status = (this.lang == 'en') ? 'All': this.arabic('all');
    this.SourceOU = (this.lang == 'en') ? 'All': this.arabic('all');
    this.date_from = '';
    this.date_to = '';
    this.smartSearch = '';
    if(!isUserCard){
      this.changeList(reqType,isTrue);
    }else{
      this.commonRequestList(reqType,isTrue);
    }
  }



}
