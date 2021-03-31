import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { ReportsService } from 'src/app/shared/service/reports.service';
import { VehicleMgmtServiceService } from '../../service/vehicle-mgmt-service.service';
import { MaintenanceService } from 'src/app/maintenance/service/maintenance.service';
import * as _ from 'lodash';
import { async } from 'q';

@Component({
  selector: 'app-vehicle-mgmt-list',
  templateUrl: './vehicle-mgmt-list.component.html',
  styleUrls: ['./vehicle-mgmt-list.component.scss']
})
export class VehicleMgmtListComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('dateColumn') dateColumn: TemplateRef<any>;
  @ViewChild('requestType') requestType: TemplateRef<any>;  
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  public rows:Array<any> = [];
  public columns:Array<any> = [];
  public page:number = 1;
  public itemsPerPage:number = 10;
  public maxSize:number = 10;
  public numPages:number = 1;
  public length:number = 0;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  public config:any = {
    paging: true,
    sorting: {columns: this.columns},
    filtering: {filterString: ''},
    className: ['table-striped', 'table-bordered', 'm-b-0']
  };
  filterBy:any = {
    RequestType: this.common.currentLang == 'en' ? 'All' : this.arabic('all'),
    Status: this.common.currentLang == 'en' ? 'All' : this.arabic('all'),
    Requestor: this.common.currentLang == 'en' ? 'All' : this.arabic('all'),
    TripDateFrom: null,
    TripDateTo: null,
    Destination: null,
    Smartsearch: '',
    RequestorOfficeDepartment: this.common.currentLang == 'en' ? 'All' : this.arabic('all'),
    Type: 1
  };

  Reportfilter:any = {
    RequestType: this.common.currentLang == 'en' ? 'All' : this.arabic('all'),
    Status: this.common.currentLang == 'en' ? 'All' : this.arabic('all'),
    Requestor: this.common.currentLang == 'en' ? 'All' : this.arabic('all'),
    TripDateFrom: null,
    TripDateTo: null,
    Destination: null,
    Smartsearch: '',
    RequestorOfficeDepartment: this.common.currentLang == 'en' ? 'All' : this.arabic('all'),
    Type: 0,
    UserID:0
  };
  AllDepartment:any = {
    OrganizationID: null,
    OrganizationUnits: this.common.currentLang == 'en' ? 'All' : this.arabic('all')
  };
  AllUser:any = {
    UserID: null,
    EmployeeName: this.common.currentLang == 'en' ? 'All' : this.arabic('all')
  };
  cardDetails = [];
  mgmtDetails = [
    {
      'image': 'assets/vehicle-management/driving-test.png',
      'count': 10,
      'ontrip': 0,
      'offtrip':0,
      'name': this.common.currentLang == 'en' ? 'Driver Management' : this.arabic('drivermanagement'),
      'reqtype': 1,
      'redirectTo': this.common.currentLang+'/app/vehicle-management/driver-management',      
      'orderBy': 1,
    },
    {
      'image': 'assets/vehicle-management/car-1.png',
      'count': 11,
      'ontrip': 0,
      'offtrip':0,
      'name': this.common.currentLang == 'en' ? 'Vehicle Management' : this.arabic('vehiclemgmt'),
      'redirectTo': this.common.currentLang+'/app/vehicle-management/vehicle-list',
      'reqtype': 1,
      'orderBy': 1,
    }
  ];

  AllstatusOptions:any = {'DisplayName': this.common.currentLang == 'en' ? 'All' : this.arabic('all'), 'value': null};

  statusOptions:any = [
    // {'DisplayName': "All", 'value': ""},
    // {'DisplayName': "To be Assigned", 'value': "To be Assigned"},
    // {'DisplayName': "To be Released", 'value': "To be Released"},
    // {'DisplayName': "Released", 'value': "Released"},
    // {'DisplayName': "Returned", 'value': "Returned"},
    // {'DisplayName': "Waiting Receiver Confirmation", 'value': "Waiting Receiver Confirmation"},
    // {'DisplayName': "Waiting for Return Confirmation", 'value': "Waiting for Return Confirmation"},
    // {'DisplayName': "Closed", 'value': "Closed"},
    // {'DisplayName': "Rejected", 'value': "Rejected"},
    // {'DisplayName': "Waiting For Approval", 'value': "Waiting For Approval"},
  ];
  requestorList:any = [];
  lang: any;
  progress = false;
  tableMessages: { emptyMessage: any; };
  isVehicleDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 13;
  isVehicleDepartmentTeamUserID = this.currentUser.OrgUnitID == 13 && !this.currentUser.IsOrgHead;
  isVehicleTeam: boolean;
  ownRequestsList:any = [];
  pendingRequests:any = [];
  processedRequests:any = []
  IsOrgHead: any;
  OrgUnitID: any;
  departmentList: any=[];
  UserId: any;
  vehicleList:any=[];
  Type: number;
  reportTitle: any;
  
  reqTypeList = [
    {
      'name': this.common.currentLang == 'en' ? 'All' : this.arabic('all'),
      'reqtype': null,
    },
    {
      'name': this.common.currentLang == 'en' ? 'Temporary Car With Driver' : this.arabic('tempcarwithdriver'),
      'reqtype': 1,
    },
    {
      'name': this.common.currentLang == 'en' ? 'Temporary Car Without Driver' : this.arabic('tempcarwithoutdriver'),
      'reqtype': 2,
    },
    {
      'name': this.common.currentLang == 'en' ? 'Permanent Car' : this.arabic('permanantcar') ,
      'reqtype': 3,
    },
  ];

  constructor(public common: CommonService,private reportsService: ReportsService, public router:Router, public vehicleservice: VehicleMgmtServiceService, private maintenanceService: MaintenanceService,  public bsModalRef: BsModalRef,public modalService: BsModalService,) { 

  }

  ngOnInit() {
    this.UserId = this.currentUser.id;
    this.lang = this.common.currentLang;
    this.getVehicleRequestList();
    this.getVehicleRequestCount();
    this.getRequestorList();
    this.GetstatusOptions();
    this.reportTitle = (this.common.language == 'English') ? "Report" : this.arabic('report');
    debugger;
    console.log(this.arabic('drivermanagement'));   
    console.log(this.mgmtDetails); 
    this.mgmtDetails[0].name = this.common.currentLang == 'en' ? 'Driver Management' : this.arabic('drivermanagement');
    console.log(this.arabic('this.mgmtDetails[0].name'));
    if(this.isVehicleDepartmentHeadUserID || this.isVehicleDepartmentTeamUserID) {
      this.isVehicleTeam = true;
      this.filterBy.Type = 0;
    } else {
      this.isVehicleTeam = false;
      this.filterBy.Type = 1;
    }
    this.common.breadscrumChange('Vehicle Management','List Page','');
    this.common.topBanner(true, 'Vehicle Management Dashboard', '+ CREATE REQUEST', '/en/app/vehicle-management/vehicle-request');
    if(this.common.currentLang == 'ar'){
      this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('listpage'),'');
      this.common.topBanner(true, this.arabic('dashboard'), '+ '+this.arabic('createrequest'), '/ar/app/vehicle-management/vehicle-request');
    }
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabic('nodatatodisplay')
    };
    this.columns = [
      { name: this.common.currentLang == 'en' && 'Ref ID' || this.arabic('refid'), prop: 'ReferenceNumber' },
      { name: this.common.currentLang == 'en' && 'Requestor Name' || this.arabic('requestorname'), prop: 'RequestorName' },
      { name: this.common.currentLang == 'en' && 'Request Type' || this.arabic('requesttype'), prop: 'RequestType', cellTemplate: this.requestType },
      { name: this.common.currentLang == 'en' && 'Trip Date From' || this.arabic('tripdatefrom'), prop: 'TripDateFrom', cellTemplate: this.dateColumn },
      { name: this.common.currentLang == 'en' && 'Trip Date To' || this.arabic('tripdateto'), prop: 'TripDateTo', cellTemplate: this.dateColumn },
      { name: this.common.currentLang == 'en' && 'Trip Time From' || this.arabic('triptimefrom'), prop: 'TripTimeFrom' },
      { name: this.common.currentLang == 'en' && 'Trip Time To' || this.arabic('triptimeto'), prop: 'TripTimeTo'},
      { name: this.common.currentLang == 'en' && 'City' || this.arabic('city'), prop: 'City'},
      { name: this.common.currentLang == 'en' && 'Destination' || this.arabic('destination'), prop: 'Destination' },
      { name: this.common.currentLang == 'en' && 'Status' || this.arabic('status'), prop: 'Status' },
      { name: this.common.currentLang == 'en' && 'Action' || this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
    ];
  }

  getRequestListName(value) {
    let selectedVal = _.find(this.reqTypeList, function (item) { return (item.reqtype == value) });
    return selectedVal.name;
  }

  async getVehicleRequestList() {
    let RequestType='';
    let Status='';
    let Requestor='';
    let TripDateFrom='';
    let TripDateTo='';
    let TripTimeTo='';
    let TripTimeFromTo='';
    let Destination='';
    let Smartsearch='';
    let RequestorOfficeDepartment='';
    debugger;
    if(this.filterBy.Status) {
      Status = this.filterBy.Status
    }
    
    if(this.filterBy.Requestor) {
      Requestor = this.filterBy.Requestor
    }
    if(this.filterBy.RequestType) {
      RequestType = this.filterBy.RequestType
    }
    if(this.filterBy.TripDateFrom) {
      TripDateFrom = new Date(this.filterBy.TripDateFrom).toJSON();
    }
    if(this.filterBy.TripDateTo) {
      TripDateTo = new Date(this.filterBy.TripDateTo).toJSON();
    }
    if(this.filterBy.Destination) {
      Destination = this.filterBy.Destination
    }
    if(this.filterBy.Smartsearch) {
      Smartsearch = this.filterBy.Smartsearch
    }
    if(this.filterBy.RequestorOfficeDepartment) {
      RequestorOfficeDepartment = this.filterBy.RequestorOfficeDepartment
    }
    if(this.isVehicleDepartmentHeadUserID || this.isVehicleDepartmentTeamUserID) {
      if(this.filterBy.Type == 1)
        this.filterBy.Type = 0;
    }
    if(this.filterBy.Status == 'All' || this.filterBy.Status == this.arabic('all')) {
      Status = '';
    }
    if(this.filterBy.RequestType == 'All' || this.filterBy.RequestType == this.arabic('all')) {
      RequestType = '';
    }
    if(this.filterBy.RequestorOfficeDepartment == 'All' || this.filterBy.RequestorOfficeDepartment == this.arabic('all')) {
      RequestorOfficeDepartment = '';
    }
    if(this.filterBy.Requestor == 'All' || this.filterBy.Requestor == this.arabic('all')) {
      Requestor = '';
    }
    await this.vehicleservice.getVehicleRequestList(this.page, this.maxSize, RequestType, Status, Requestor, TripDateFrom, TripDateTo, Destination, Smartsearch, RequestorOfficeDepartment, this.UserId, this.filterBy.Type).subscribe(data => {
      this.vehicleList = data;
      this.rows = this.vehicleList.Collection;
      this.config.totalItems = this.vehicleList.Count;
    });
  }

  public onChangePage(config:any, page:any = {page: this.page, itemsPerPage: this.itemsPerPage}):any {
    this.page = page;
    this.getVehicleRequestList();
  }

  getVehicleList(type) {
    this.filterBy.Type = type;
    this.getVehicleRequestList()
    this.vehicleservice.triggerScrollTo();
  }

  goToDocuments(){
    this.router.navigate(['/'+this.common.currentLang+'/app/vehicle-management/documents']);
  }

  async getVehicleRequestCount() {
     await this.vehicleservice.getVehicleRequestCount(this.UserId).subscribe((data:any) => {
       this.mgmtDetails[0].ontrip = data.DriversOnTrip? data.DriversOnTrip : 0;
       this.mgmtDetails[0].offtrip = data.DriversOffTrip? data.DriversOffTrip : 0;
       this.mgmtDetails[1].ontrip = data.VehicleOnTrip? data.VehicleOnTrip : 0;
       this.mgmtDetails[1].offtrip = data.VehicleOffTrip? data.VehicleOffTrip : 0;
        this.cardDetails.push({
          'image': 'assets/vehicle-management/car.png',
          'count': data.Vehicle ? data.Vehicle : 0,
          'name': this.common.currentLang == 'en' && 'Vehicle Requests' || this.arabic('vechilerequests'),
          'progress': 50
        },
        {
          'image': 'assets/vehicle-management/fine.png',
          'count': data.Fine ? data.Fine : 0,
          'name': this.common.currentLang == 'en' && 'Fine Management' || this.arabic('finemanagement'),
          'progress': 50,
          'redirectTo': this.common.currentLang+'/app/vehicle-management/fine-management'      
        },
        {
          'image': 'assets/vehicle-management/taxi1.png',
          'count': data.RentedCar ? data.RentedCar : 0,
          'name':  this.common.currentLang == 'en' && 'Rent a Car Companies Management' || this.arabic('rentacarcompaniesmanagement'),
          'progress': 50,
          'redirectTo': this.common.currentLang+'/app/vehicle-management/rent-car'
        })
        this.ownRequestsList.push({
            'image': 'assets/employee-profile/passport.png',
            'count': data.OwnRequest ? data.OwnRequest : 0,
            'name': this.common.currentLang == 'en' && 'My Own Requests' || this.arabic('myownrequests'),
            'reqtype': 2,
            'progress': 50
          })
        this.pendingRequests.push({
            'image': 'assets/employee-profile/passport.png',
            'count': data.Vehicle ? data.Vehicle : 0,
            'name': this.common.currentLang == 'en' && 'My Pending Actions' || this.arabic('mypendingrequests'),
            'reqtype': 1,
            'progress': 50
          })
        this.processedRequests.push(
          {
            'image': 'assets/employee-profile/passport.png',
            'count': data.MyProcessedRequest ? data.MyProcessedRequest : 0,
            'name': this.common.currentLang == 'en' && 'My Processed Request' || this.arabic('myprocessedrequest'),
            'reqtype': 3,
            'progress': 50
          })
      });
   }
  
  getRequestorList() {
    let params = [{
      'OrganizationID': this.OrgUnitID,
      'OrganizationUnits': 'string'
    }];
    this.common.getUserList(params,0).subscribe((data: any) => {
      this.requestorList = data;
      this.requestorList.splice(0, 0,this.AllUser);
      this.getRequestorDepartment();
    });
  }

  getRequestorDepartment(){
    this.maintenanceService.getById(0, this.currentUser.id)
    .subscribe((response: any) => {
      if (response != null) {
        this.departmentList = response.OrganizationList;
        
        this.departmentList.splice(0, 0,this.AllDepartment);
      }
    });
  }

  async GetstatusOptions(){
    this.vehicleservice.getVehicleRequest(this.page, this.maxSize,1).subscribe(data => {
      this.vehicleList = data;
    this.statusOptions = this.vehicleList.M_LookupsList;
    this.statusOptions.splice(0, 0,this.AllstatusOptions);
  });
}

  reqSelect(redirect, name) {
    if(name == 'Vehicle Requests' || name == this.arabic('vechilerequests')) {
      this.filterBy.Type = 0;
      this.getVehicleRequestList();
      this.vehicleservice.triggerScrollTo();
    }else {
      this.router.navigate(['/'+redirect]);
    }
  }

  dateChange(eve, isStartEnd){

  }

  viewData(type, value){
    if(type == "assignto" || type == "reassign") {
      this.router.navigate([this.lang+'/app/vehicle-management/vehicle-assign/' + value.VehicleReqID]);
    } else if(type == "approve") {
      this.router.navigate([this.lang+'/app/vehicle-management/vehicle-request-view/' + value.VehicleReqID]);
    } else if(type == "release") {
      this.router.navigate([this.lang+'/app/vehicle-management/vehicle-release-form/' + value.VehicleReqID]);
    } else if(type == "return") {
      this.router.navigate([this.lang+'/app/vehicle-management/vehicle-return-form/' + value.VehicleReqID]);
    } else if(type == "releaseconfirmation") {
      this.router.navigate([this.lang+'/app/vehicle-management/vehicle-release-confirmation/' + value.VehicleReqID]);
    } else if(type == "returnconfirmation") {
      this.router.navigate([this.lang+'/app/vehicle-management/vehicle-return-confirmation/' + value.VehicleReqID]);
    }
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  openReport(temp) {
    this.filterInit();
    this.bsModalRef = this.modalService.show(temp, {
      class: 'modal-lg', backdrop: true,
      ignoreBackdropClick: true
    });
  }

  closemodal() {
    this.filterInit();
    this.bsModalRef.hide();
    this.Reportfilter.RequestType = this.common.currentLang == 'en' ? 'All' : this.arabic('all');
    this.Reportfilter.Status =  this.common.currentLang == 'en' ? 'All' : this.arabic('all');
    this.Reportfilter.Requestor =  this.common.currentLang == 'en' ? 'All' : this.arabic('all');
    this.Reportfilter.RequestorOfficeDepartment =  this.common.currentLang == 'en' ? 'All' : this.arabic('all');
    this.Reportfilter.Smartsearch = null,
    this.Reportfilter.TripDateFrom = null,
    this.Reportfilter.TripDateTo = null,
    this.Reportfilter.Destination  = null
  }
  filter: any;
  filterInit() {
    this.filter = {
      status: (this.lang == 'en') ? 'All': this.arabic('all'),
      department: (this.lang == 'en') ? 'All': this.arabic('all'),
      name: (this.lang == 'en') ? 'All': this.arabic('all'),
      from: '',
      to: '',
      plateNumber: '',
      search: ''
    };
  }

  Download() {
    let SourceOU = '';
    let itStatus = '';

    if(this.Reportfilter.Status == 'All' || this.Reportfilter.Status == this.arabic('all')) {
      this.Reportfilter.Status = '';
    }
    if(this.Reportfilter.RequestType == 'All' || this.Reportfilter.RequestType == this.arabic('all')) {
      this.Reportfilter.RequestType = '';
    }
    if(this.Reportfilter.RequestorOfficeDepartment == 'All' || this.Reportfilter.RequestorOfficeDepartment == this.arabic('all')) {
      this.Reportfilter.RequestorOfficeDepartment = '';
    }
    if(this.Reportfilter.Requestor == 'All' || this.Reportfilter.Requestor == this.arabic('all')) {
      this.Reportfilter.Requestor = '';
    }

    this.Reportfilter.UserID = this.currentUser.id;
    let dateVal = new Date(), cur_date = dateVal.getDate() +'-'+(dateVal.getMonth()+1)+'-'+dateVal.getFullYear();
    this.reportsService.downloadVehicleReport(this.Reportfilter).subscribe((resultBlob) =>{
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = 'VehicleManagement-'+cur_date+'.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
      this.bsModalRef.hide();
      this.Reportfilter.RequestType = this.common.currentLang == 'en' ? 'All' : this.arabic('all');
      this.Reportfilter.Status =  this.common.currentLang == 'en' ? 'All' : this.arabic('all');
      this.Reportfilter.Requestor =  this.common.currentLang == 'en' ? 'All' : this.arabic('all');
      this.Reportfilter.RequestorOfficeDepartment =  this.common.currentLang == 'en' ? 'All' : this.arabic('all');
      this.Reportfilter.Smartsearch = null,
      this.Reportfilter.TripDateFrom = null,
      this.Reportfilter.TripDateTo = null,
      this.Reportfilter.Destination  = null
    });
  }
}
