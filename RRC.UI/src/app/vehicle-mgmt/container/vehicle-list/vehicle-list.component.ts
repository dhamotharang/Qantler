import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsDatepickerConfig, BsModalService } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { VehicleMgmtServiceService } from '../../service/vehicle-mgmt-service.service';
import { Router } from '@angular/router';
import { DropdownsService } from 'src/app/shared/service/dropdowns.service';
import { MaintenanceService } from 'src/app/maintenance/service/maintenance.service';
import { ReportsService } from 'src/app/shared/service/reports.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { VehicleManagementComponent } from 'src/app/modal/vehicle-management/vehicle-management.component';
import { VehcileListReportComponent } from 'src/app/modal/vehcile-list-report/vehcile-list-report.component';
import { DatePipe, DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.scss']
})
export class VehicleListComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('contractEndDate') contractEndDate: TemplateRef<any>;
  @ViewChild('vehicleRegExpiry') vehicleRegExpiry: TemplateRef<any>;
  bsConfig: Partial<BsDatepickerConfig>;
  rows:Array<any> = [];
  columns:Array<any> = [];
  page:number = 1;
  itemsPerPage:number = 10;
  maxSize:number = 10;
  numPages:number = 1;
  length:number = 0;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  public config:any = {
    paging: true,
    sorting: {columns: this.columns},
    filtering: {filterString: ''},
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    page: 1,
    maxSize: 10,
    itemsPerPage:10,
    totalItems:0
  };
  cardDetails:any = [];
  statusOptions:any = [
    {'DisplayName': "All", 'value': ""},
    {'DisplayName': "Under Process", 'value': "Under Process"},
    {'DisplayName': "Pending for Resubmission", 'value': "Pending for Resubmission"},
    {'DisplayName': "Closed", 'value': "Closed"},
    {'DisplayName': "Waiting for Approval", 'value': "Waiting for Approval"},
    {'DisplayName': "Rejected", 'value': "Rejected"}
  ];
  requestorList:any = [
    {'EmployeeName': "test", 'UserID': "12"},

  ];
  lang: any;
  progress = false;
  tableMessages: { emptyMessage: any; };
  isVehicleDepartmentHeadUserID = this.currentUser.IsOrgHead && this.currentUser.OrgUnitID == 13;
  isVehicleDepartmentTeamUserID = this.currentUser.OrgUnitID == 13 && !this.currentUser.IsOrgHead;
  showMgmtCard: boolean;
  ownRequestsList:any = [
    {
      'image': 'assets/employee-profile/passport.png',
      'count': 123,
      'name': 'My Own Requests',
      'reqtype': 7,
      'orderBy': 7,
      'progress': 50,
      'Reqname' :'DisplayChange',
      'isMyOwnReSelected': true
    }
  ];
  IsOrgHead: any;
  OrgUnitID: any;
  departmentList: any=[];
  alternativeVehicleList: any=[];
  bsModalRef: any;
  plateNumberList:any = [];
  plateColourList:any = [];
  filterBy:any = {
    PlateColor:this.common.currentLang == 'en' && "All" || this.arabic('all'),
    PlateNumber:this.common.currentLang == 'en' && "All" || this.arabic('all'),
    UserID:'',   
    DepartmentOffice:this.common.currentLang == 'en' && "All" || this.arabic('all'),
    Destination:'',
    RequestorDepartment:'',
    AlternativeVehicle: this.common.currentLang == 'en' && "No" || this.arabic('no')
  };
  validateStartEndDate:any = {
    isValid:true,
    msg:''
  };
  componentRef:any;
  constructor(public common: CommonService,
    public router:Router,
    public vehicleservice: VehicleMgmtServiceService,
    public modalService: BsModalService,
    private maintenanceService:MaintenanceService,
    private reportService:ReportsService,
    private utilsService:UtilsService,
    public datePipe: DatePipe) {

  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    this.lang = this.common.currentLang;
    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : 0;
    this.getRequestorList();
   
    this.componentRef = this;


    // if(this.isVehicleDepartmentHeadUserID || this.isVehicleDepartmentTeamUserID) {
    //   this.showMgmtCard = true;
    // } else {
    //   this.showMgmtCard = false;
    // }
    if(this.common.currentLang != 'en'){
      this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('managevehicles'),'');
      this.common.topBanner(true, '', '+ '+this.arabic('newvehicle'), '/ar/app/vehicle-management/vehicle-details-create');
      this.alternativeVehicleList = [this.arabic('yes'), this.arabic('no'), this.arabic('both')];
      this.cardDetails = [
        // {
        //   'image': 'assets/vehicle-management/taxi.png',
        //   'name': this.arabic('managevehicles'),
        //   'redirectTo': this.common.currentLang+'/app/vehicle-management/fine-management'
        // },
        {
          'image': 'assets/vehicle-management/technical-support.png',
          'name': this.arabic('servicelogs'),
          'redirectTo': this.common.currentLang+'/app/vehicle-management/fine-management'
        }
      ];
      this.columns = [
        { name: this.arabic('platenumber'), prop: 'plateNumber' },
        { name: this.arabic('platecolour'), prop: 'plateColor' },
        { name: this.arabic('vehiclemodel'), prop: 'vehicleModel' },
        // { name: this.arabic('contractenddate'), prop: 'contractEndDate', cellTemplate: this.contractEndDate },
        { name: this.arabic('contractenddate'), prop: 'contractEndDate' },
        { name: this.arabic('vehicleregistrationexpiry'), prop: 'vehicleRegExpiry', cellTemplate: this.vehicleRegExpiry },
        { name: this.arabic('nextserviceslashtyrechange'), prop: 'nextService' },
        { name: this.arabic('departmentslashoffice'), prop: 'nameofDepartment' },
        { name: this.arabic('nameoftheuser'), prop: 'nameofuser'},
        { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate }
      ];
    }else{
      this.common.breadscrumChange('Vehicle Management','Manage Vehicles','');
      this.common.topBanner(true, 'Vehicle List', '+ NEW VEHICLE', '/en/app/vehicle-management/vehicle-details-create');
      this.alternativeVehicleList = ['Yes','No','Both'];
      this.cardDetails = [
        // {
        //   'image': 'assets/vehicle-management/taxi.png',
        //   'name': 'Manage Vehicles',
        //   'redirectTo': this.common.currentLang+'/app/vehicle-management/fine-management'
        // },
        {
          'image': 'assets/vehicle-management/technical-support.png',
          'name': 'Service Logs',
          'redirectTo': this.common.currentLang+'/app/vehicle-management/fine-management'
        }
      ];
      this.columns = [
        { name: 'Plate Number', prop: 'plateNumber' },
        { name: 'Plate Colour', prop: 'plateColor' },
        { name: 'Vehicle Model', prop: 'vehicleModel' },
        // { name: 'Contract End Date', prop: 'contractEndDate', cellTemplate: this.contractEndDate },
        { name: 'Contract End Date', prop: 'contractEndDate' },
        { name: 'Vehicle Registration Expiry', prop: 'vehicleRegExpiry', cellTemplate: this.vehicleRegExpiry },
        { name: 'Next Service / Tyre Change', prop: 'nextService' },
        { name: 'Department / office', prop: 'nameofDepartment' },
        { name: 'Name of the user', prop: 'nameofuser'},
        { name: 'Action', prop: '', headerClass: 'action-width', cellClass: 'action-width', cellTemplate: this.actionTemplate }
      ];
    }
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabic('nodatatodisplay')
    };
    this.loadVehicleList();
     this.getPlateNumberAndColorList();
  }

  getRequestorList() {
    let params = [{
      'OrganizationID': this.OrgUnitID,
      'OrganizationUnits': 'string'
    }];
    this.common.getUserList(params,0).subscribe((data: any) => {
      this.requestorList = data;
      this.getRequestorDepartment();
    });
  }

  getRequestorDepartment(){
    this.maintenanceService.getById(0, this.currentUser.id)
    .subscribe((response: any) => {
      if (response) {
        this.departmentList = response.OrganizationList;
        switch(this.common.currentLang)
        {
          case "en": {this.departmentList.splice(0, 0, "All");break;}
          case "ar": {this.departmentList.splice(0,0,this.arabic('all'));break;}
        }     
      }
    });
  }

 async getPlateNumberAndColorList(){
    this.vehicleservice.getPlateNumberAndColorList().subscribe((List:any) => {
      if(List){
        if(List.plateNumber)
        {
          this.plateNumberList = List.plateNumber;               
        }
        if(List.plateColor)
        {
          this.plateColourList = List.plateColor;         
        }  
        switch(this.common.currentLang)
          {
            case "en": {this.plateNumberList.splice(0, 0, "All");this.plateColourList.splice(0, 0, "All"); break;}
            case "ar": {this.plateNumberList.splice(0,0,this.arabic('all'));this.plateColourList.splice(0,0,this.arabic('all'));  break;}
          }     
          
      }
    });
  }

  reqSelect(redirect, isTrue) {
    this.router.navigate(['/'+this.common.currentLang+'/app/vehicle-management/vehicle-servicelog-view/']);
  }


  arabic(word) {
    return this.common.arabic.words[word];
  }

  downloadExcel() {
    // let ReferenceNumber = '';
    // let EventType = '';
    // let EventRequestor = '';
    // let EventDetails = '';
    // let DateFrom = '';
    // let DateTo = '';
    // let Status = '';
    // let Location = '';

    let toSendReportOptions:any = {
    };

    // if(toSendReportOptions.PlateNumber == "All" || toSendReportOptions.PlateNumber == this.arabic('all'))
    // {
    //   toSendReportOptions.PlateNumber = null;
    // }
    // if(toSendReportOptions.PlateColor == "All" || toSendReportOptions.PlateColor == this.arabic('all'))
    // {
    //   toSendReportOptions.PlateColor = null;
    // }

    //   if(!this.filterBy.SmartSearch){
    //     toSendReportOptions.SmartSearch = '';
    //   }

    //   if(!this.filterBy.DepartmentOffice){
    //     toSendReportOptions.DepartmentOffice = '';
    //   }

    //   if(!this.filterBy.RequestorDepartment){
    //     toSendReportOptions.RequestorDepartment = '';
    //   }

    //   if(!this.filterBy.PlateNumber){
    //     toSendReportOptions.PlateNumber = '';
    //   }

    //   if(!this.filterBy.PlateColor){
    //     toSendReportOptions.PlateColor = '';
    //   }

    //   if(!this.filterBy.Destination){
    //     toSendReportOptions.Destination = '';
    //   }

    //   // if(!this.filterBy.SmartSearch){
    //   //   toSendReportOptions.DepartmentOffice = '';
    //   // }


    toSendReportOptions.UserID = this.currentUser.id;
    let dateVal = new Date(), cur_date = dateVal.getDate() +'-'+(dateVal.getMonth()+1)+'-'+dateVal.getFullYear();
    // this.reportService.downloadModuleReport(toSendReportOptions,'vehicle_management').subscribe((data) => {
      this.vehicleservice.vehicleListExcelDownload(toSendReportOptions).subscribe((data) => {
      var url = window.URL.createObjectURL(data);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = 'VehicleManagementReport.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
        // this.bsModalRef.hide();
    });
  }

  arabicfn(word) {
    return this.common.arabic.words[word];
  }

  onChangePage(config,event){
    this.loadVehicleList();
  }

  onSearch(){
    this.config.page = 1;
    this.config.itemsPerPage = 10;
    this.config.maxSize = 10;
    this.config.paging = true;
    this.loadVehicleList();
  }

  viewData(value:any){
    console.log(value);
    this.router.navigate(['/'+this.common.currentLang+'/app/vehicle-management/vehicle-details-view/' + value.vehicleID]);
  }

  openReport() {
    let initialState = {};
    this.bsModalRef = this.modalService.show(VehcileListReportComponent,Object.assign({class:'modal-lg'}, {}, { initialState }));
  }

  removeWordSpaces(words:string){
   return  words.replace(/\s+/g, '');
  }


  dateChange(eve,isStartEnd){
    if(eve){
      if(isStartEnd == 'start'){
        this.filterBy.ReqDateFrom = eve;
      }
      if(isStartEnd == 'end'){
        this.filterBy.ReqDateTo = eve;
      }
    }
  }

  checkStartEndDiff(){
    let toRetVal = true;
    if(this.utilsService.isValidDate(this.filterBy.ReqDateFrom)
    && this.utilsService.isValidDate(this.filterBy.DateTo)){
      if(this.filterBy.ReqDateFrom.getTime() <= this.filterBy.DateTo.getTime()){
        toRetVal =  true;
      }else{
        toRetVal = false;
      }
    }else{
      if(!this.utilsService.isDate(this.filterBy.ReqDateFrom) || !this.utilsService.isDate(this.filterBy.DateTo)){
        toRetVal = false;
      }
    }
    this.validateStartEndDate.isValid = toRetVal;
    if(this.validateStartEndDate.isValid){
      this.validateStartEndDate.msg = '';
    }else{
      this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date': this.arabicfn('errormsgvalidenddate');
    }
  }

  dateValidation() {
    this.validateStartEndDate.msg = this.common.currentLang == 'en' ? 'Please select a valid Start/ End Date': this.arabicfn('errormsgvalidenddate');
    let showDateValidationMsg = false;
    if (this.filterBy.ReqDateFrom && this.filterBy.ReqDateTo) {
      let startDate = new Date(this.filterBy.ReqDateFrom).getTime();
      let endDate = new Date(this.filterBy.ReqDateTo).getTime();
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

  loadVehicleList(){
    this.progress = true;
    let toSendReq:any = {
      PageNumber:this.config.page,
      PageSize:this.config.itemsPerPage,
      UserID:this.currentUser.id,
      UserName:this.currentUser.username,
      SmartSearch:this.filterBy.SmartSearch,
      DepartmentOffice:this.filterBy.DepartmentOffice,
      RequestorDepartment: this.filterBy.RequestorDepartment,
      PlateNumber:this.filterBy.PlateNumber,
      PlateColor:this.filterBy.PlateColor,
      Destination:this.filterBy.Destination,
      AlternativeVehicle:this.filterBy.AlternativeVehicle
    };
    if(toSendReq.PlateNumber == "All" || toSendReq.PlateNumber == this.arabic('all'))
    {
      toSendReq.PlateNumber = null;
    }
    if(toSendReq.PlateColor == "All" || toSendReq.PlateColor == this.arabic('all'))
    {
      toSendReq.PlateColor = null;
    }
    if(toSendReq.DepartmentOffice == "All" || toSendReq.DepartmentOffice == this.arabic('all'))
    {
      toSendReq.DepartmentOffice = null;
    }
    if(toSendReq.AlternativeVehicle == "Both" || toSendReq.AlternativeVehicle == this.arabic('both'))
    {
      toSendReq.AlternativeVehicle = "Both";
    }
    if(toSendReq.AlternativeVehicle == "Yes" || toSendReq.AlternativeVehicle == this.arabic('yes'))
    {
      toSendReq.AlternativeVehicle = "Yes";
    }
    if(toSendReq.AlternativeVehicle == "No" || toSendReq.AlternativeVehicle == this.arabic('no'))
    {
      toSendReq.AlternativeVehicle = "No";
    }
    // if(!this.filterBy.Status){
    //   this.filterBy.Status = 'All';
    //   if(this.lang !='en'){
    //     this.filterBy.Status = this.arabicfn('all');
    //   }
    // }
    // if(this.filterBy.Status && this.filterBy.Status != "All" && this.filterBy.Status != this.arabicfn('all')){
    //   toSendReq.Status = this.filterBy.Status;
    // }


    // if(this.filterBy.ReqDateFrom){
    //   toSendReq.ReqDateFrom = new Date(this.filterBy.ReqDateFrom).toJSON();
    // }

    // if(this.filterBy.ReqDateTo){
    //   toSendReq.ReqDateTo = new Date(this.filterBy.ReqDateTo).toJSON();
    // }

    
    this.vehicleservice.getVehicleList(toSendReq.PageNumber,toSendReq.PageSize,toSendReq).subscribe((vehicleModuleRes:any) => {
      if(vehicleModuleRes){
        this.rows = vehicleModuleRes.collection;
        this.config.totalItems = vehicleModuleRes.count;         
      }
      if(this.common.currentLang == 'en'){
        this.rows.map(res => res.contractEndDate = (res.contractEndDate != null) ? this.datePipe.transform(res.contractEndDate, "dd/MM/yyyy"):'Temporary');
      }else{
      this.rows.map(res => res.contractEndDate = (res.contractEndDate != null) ? this.datePipe.transform(res.contractEndDate, "dd/MM/yyyy"): this.arabic('temporary'));
      }
      this.progress = false;
    });
    // let currentDate = new Date(2019,11,26).getTime();
    // let previousDate = new Date(2019,10,27).getTime();
    // console.log(currentDate-previousDate);
  }

  openLogModal(row: any, type: string) {
    let initialState: any = {
      id: row.vehicleID
    };
    if (type == 'service') {
      initialState.Message = '';
      initialState.Title = 'LOG A SERVICE';
    } else if (type == 'tyre') {
      initialState.Message = '';
      initialState.Title = 'LOG A TYRE CHANGE';
    } else {
      initialState.Message = '';
      initialState.Title = 'LOG A FINE';
    }
    // if (this.common.currentLang == 'ar') {
    //   initialState.message = this.arabic('leaveescalatemsg');
    // }
    this.modalService.show(VehicleManagementComponent, Object.assign({}, {}, { initialState }));
  }

  getRowClass = (row) => {
    let toRetClass='';
    let currenDate = new Date();
    let ContractEndDate = (row.contractEndDate != 'Temporary' && row.contractEndDate != this.arabic('temporary')) ? new Date(row.contractEndDate): null;
    let VehicleRegistrationEndDate = new Date(row.vehicleRegistrationExpiry);
    if(ContractEndDate != null){
      if(ContractEndDate.getTime() < currenDate.getTime() || VehicleRegistrationEndDate.getTime() < currenDate.getTime()){
        return 'document-expired';
      }else if(monthDiffCalculator(currenDate,ContractEndDate) < 60 || monthDiffCalculator(currenDate,VehicleRegistrationEndDate) < 30){
        return 'document-expiry-soon';
      }else if(((row.nextService -row.currentMileage) < 500) || ((row.tyreChange - row.currentMileage) < 500)){
        return 'change-due';
      }
    }else {
      if(VehicleRegistrationEndDate.getTime() < currenDate.getTime()){
        return 'document-expired';
      }else if(monthDiffCalculator(currenDate,VehicleRegistrationEndDate) < 30){
        return 'document-expiry-soon';
      }
    }
    function monthDiffCalculator(startDate,endDate){
      let year1 = startDate.getFullYear();
      let year2 = endDate.getFullYear();
      let month1 = startDate.getMonth();
      let month2 = endDate.getMonth();
      // if(month1===0){ //Have to take into account
      //   month1++;
      //   month2++;
      // }
      // let numberOfMonths = (year2 - year1) * 12 + (month2 - month1) + 1;
      let numberOfDays = (endDate.getTime() - startDate.getTime())  / (1000 * 3600 * 24);
      return numberOfDays;
    }    
  }

  monthDiffCalculation(startDate,endDate){
    let year1 = startDate.getFullYear();
    let year2 = endDate.getFullYear();
    let month1 = startDate.getMonth();
    let month2 = endDate.getMonth();
    if(month1===0){ //Have to take into account
      month1++;
      month2++;
    }
    let numberOfMonths = (year2 - year1) * 12 + (month2 - month1) + 1;
    return numberOfMonths;
  }
}
