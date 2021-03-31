import { Component, OnInit, Input, Inject, Renderer2, TemplateRef, ViewChild, ElementRef } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { VehicleMgmtServiceService } from '../../service/vehicle-mgmt-service.service';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { AdminService } from 'src/app/admin/service/admin/admin.service';
import { CitizenAffairService } from 'src/app/citizen-affair/service/citizen-affair.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import * as _ from 'lodash';
import { DatePipe, DOCUMENT } from '@angular/common';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-vehicle-request',
  templateUrl: './vehicle-request.component.html',
  styleUrls: ['./vehicle-request.component.scss']
})
export class VehicleRequestComponent implements OnInit {
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  @Input() screenStatus:string;
  @ViewChild('template') template : TemplateRef<any>;
  multipledropdown: any = [{ index: 1 }];
  DeptID:number;
  bsModalRef: BsModalRef;
  IsOrgHead: any;
  OrgUnitID: any;
  requestorList: any=[];
  lang: any;
  emiratesList: any=[];
  passengersList:any=["test1", "test2"];
  cityList: any=[];
  requestType: any;
  triptype: any;
  destination: any;
  month: any;
  driversList: any=[];
  timeList: Array<any>;
  TripPeriodFromTime: any;
  showValidTimeAlert: boolean = false;
  TripPeriodToTime: any;
  CoPassengerDepartmentID = [];
  CoPassengerID = [];
  DisplayRequestDateTime: any;
  ReferenceNumber: any;
  RadioValue:string="";
  formData = {
    RequestType: null,
    Requestor: null,
    RequestDateTime: null,
    DriverId: null,
    TobeDrivenbyDepartmentID: null,
    TobeDrivenbyDriverID: null,
    TripTypeID: null,
    TripTypeOthers: "",
    Emirates: null,
    City: null,
    Destination: null,
    DestinationOthers: "",
    TripPeriodFrom: null,
    TripPeriodTo: null,
    TripPeriodFromDate: null,
    TripPeriodToDate: null,
    VehicleModelID: 4,
    ApproverDepartment: null,
    ApproverName: null,
    TripCoPassengers: [],
    // TripCoPassengers : [{
    //   CoPassengerDepartment: null,
    //   CoPassengerName: null,
    //   CoPassengerID: null,
    //   OthersCoPassengerName: "",
    //   CoPassengerDepartmentID: null
    // }],
    TripVehicleIssues: [],
    DeleteFlag: false,
    CreatedBy: 0,
    CreatedDateTime: new Date(),
    Action: "Submit",
    Notes: "",
    ReleaseDateTime: null,
    LastMileageReading: null,
    ReleaseLocationID: null,
    ReturnDateTime: null,
    CurrentMileageReading: null,
    HavePersonalBelongings: false,
    PersonalBelongingsText: '',
    VehicleID: null
  }
  vehicleModelList: any=[];
  destinationList: any=[];
  tripTypeList:any=[];
  approverList:any=[];
  departmentList: any=[];
  editMode: boolean = true;
  valid: boolean = false;
  inProgress = false;
  CoPssApproverList: any=[];
  toBedrivenList: any=[];
  message: string;
  vehicleRequestId: number;
  screenTitle: string;
  CoPassengarDepartList: any=[];
  CurrentApproverID: any;
  currentStatus: any;
  isApprover: boolean=false;
  tripTypename = "";
  DestinationName="";
  DisableApproverField: boolean;
  notes = "";
  Reason = "";
  HideFields:boolean=true;
  pdfSrc: string;
  showPdf: boolean = false;
  PrintHide:boolean=true;
  isReturnForm:boolean = false;
  currentdate:any;
  constructor(private route: ActivatedRoute, public router: Router, public util: UtilsService, public datePipe: DatePipe, private renderer: Renderer2, @Inject(DOCUMENT) private document: Document, public utils: UtilsService, public common: CommonService, public vehicleservice: VehicleMgmtServiceService, private adminService:AdminService, public citizenservice: CitizenAffairService, public modalService: BsModalService) { }

  ngOnInit() {
   let LocalStorageData = JSON.parse(localStorage.getItem("User"));
   console.log(LocalStorageData)
   this.DeptID =  LocalStorageData.DepartmentID;
   console.log(this.DeptID)
    this.lang = this.common.currentLang;
    this.getRequestorList();
    this.getDriverList();
    if(this.screenStatus === 'create') {
      this.DisableApproverField = false;
      this.editMode = true;
      this.common.breadscrumChange('Vehicle Management','Create Vehicle Request','');
      this.screenTitle = "Vehicle Request Creation";
      this.formData.RequestDateTime = this.datePipe.transform(new Date(), "dd/MM/yyyy h:mm a");
      this.currentdate=new Date().toJSON();
      this.DisplayRequestDateTime = this.datePipe.transform(new Date(), "dd/MM/yyyy h:mm a");
      if(this.lang == 'ar'){
        this.screenTitle = this.arabic('vehiclerequestcreation');
        this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('createvehiclerequest'),'');
        this.DisplayRequestDateTime = this.datePipe.transform(new Date(), "dd/MM/yyyy h:mm ").concat(this.datePipe.transform(new Date(), 'a')=='AM'?this.common.arabic.words['am']:this.common.arabic.words['pm']);
      }
    }
    this.IsOrgHead = this.currentUser.IsOrgHead ? this.currentUser.IsOrgHead : false;
    this.OrgUnitID = this.currentUser.OrgUnitID ? this.currentUser.OrgUnitID : 13;

    this.route.params.subscribe(param => {
      this.vehicleRequestId = +param.id;
      if (this.vehicleRequestId > 0) {
        this.getVehicleRequestById(this.vehicleRequestId); // load vehicle request by ID
        if (this.screenStatus == 'view') {
          this.DisableApproverField = true;
          this.screenTitle = "Vehicle Request View";
          this.common.breadscrumChange('Vehicle Management', 'Vehicle Request View','');
          if(this.lang == 'ar'){
            this.screenTitle = this.arabic('vehiclerequestview');
            this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('vehiclerequestview'),'');
          }
          this.editMode = false;
        }
      }
    });
    
    this.getEmiratesList();
    this.getVehicleModel();
    this.getTripType();
    this.getRequestorDepartment();
    this.getTripDestination();
    this.common.topBanner(false, false, '', '');
    if (this.lang == 'en') {
      this.timeList = [
        { "value": "00:00", "label": "12:00 AM" },
        { "value": "00:30", "label": "12:30 AM" },
        { "value": "01:00", "label": "01:00 AM" },
        { "value": "01:30", "label": "01:30 AM" },
        { "value": "02:00", "label": "02:00 AM" },
        { "value": "02:30", "label": "02:30 AM" },
        { "value": "03:00", "label": "03:00 AM" },
        { "value": "03:30", "label": "03:30 AM" },
        { "value": "04:00", "label": "04:00 AM" },
        { "value": "04:30", "label": "04:30 AM" },
        { "value": "05:00", "label": "05:00 AM" },
        { "value": "05:30", "label": "05:30 AM" },
        { "value": "06:00", "label": "06:00 AM" },
        { "value": "06:30", "label": "06:30 AM" },
        { "value": "07:00", "label": "07:00 AM" },
        { "value": "07:30", "label": "07:30 AM" },
        { "value": "08:00", "label": "08:00 AM" },
        { "value": "08:30", "label": "08:30 AM" },
        { "value": "09:00", "label": "09:00 AM" },
        { "value": "09:30", "label": "09:30 AM" },
        { "value": "10:00", "label": "10:00 AM" },
        { "value": "10:30", "label": "10:30 AM" },
        { "value": "11:00", "label": "11:00 AM" },
        { "value": "11:30", "label": "11:30 AM" },
        { "value": "12:00", "label": "12:00 PM" },
        { "value": "12:30", "label": "12:30 PM" },
        { "value": "13:00", "label": "01:00 PM" },
        { "value": "13:30", "label": "01:30 PM" },
        { "value": "14:00", "label": "02:00 PM" },
        { "value": "14:30", "label": "02:30 PM" },
        { "value": "15:00", "label": "03:00 PM" },
        { "value": "15:30", "label": "03:30 PM" },
        { "value": "16:00", "label": "04:00 PM" },
        { "value": "16:30", "label": "04:30 PM" },
        { "value": "17:00", "label": "05:00 PM" },
        { "value": "17:30", "label": "05:30 PM" },
        { "value": "18:00", "label": "06:00 PM" },
        { "value": "18:30", "label": "06:30 PM" },
        { "value": "19:00", "label": "07:00 PM" },
        { "value": "19:30", "label": "07:30 PM" },
        { "value": "20:00", "label": "08:00 PM" },
        { "value": "20:30", "label": "08:30 PM" },
        { "value": "21:00", "label": "09:00 PM" },
        { "value": "21:30", "label": "09:30 PM" },
        { "value": "22:00", "label": "10:00 PM" },
        { "value": "22:30", "label": "10:30 PM" },
        { "value": "23:00", "label": "11:00 PM" },
        { "value": "23:30", "label": "11:30 PM" },
      ];
    } else {
      this.timeList = [
        { "value": "00:00", "label": "12:00" + this.arabic('am') },
        { "value": "00:30", "label": "12:30" + this.arabic('am') },
        { "value": "01:00", "label": "01:00" + this.arabic('am') },
        { "value": "01:30", "label": "01:30" + this.arabic('am') },
        { "value": "02:00", "label": "02:00" + this.arabic('am') },
        { "value": "02:30", "label": "02:30" + this.arabic('am') },
        { "value": "03:00", "label": "03:00" + this.arabic('am') },
        { "value": "03:30", "label": "03:30" + this.arabic('am') },
        { "value": "04:00", "label": "04:00" + this.arabic('am') },
        { "value": "04:30", "label": "04:30" + this.arabic('am') },
        { "value": "05:00", "label": "05:00" + this.arabic('am') },
        { "value": "05:30", "label": "05:30" + this.arabic('am') },
        { "value": "06:00", "label": "06:00" + this.arabic('am') },
        { "value": "06:30", "label": "06:30" + this.arabic('am') },
        { "value": "07:00", "label": "07:00" + this.arabic('am') },
        { "value": "07:30", "label": "07:30" + this.arabic('am') },
        { "value": "08:00", "label": "08:00" + this.arabic('am') },
        { "value": "08:30", "label": "08:30" + this.arabic('am') },
        { "value": "09:00", "label": "09:00" + this.arabic('am') },
        { "value": "09:30", "label": "09:30" + this.arabic('am') },
        { "value": "10:00", "label": "10:00" + this.arabic('am') },
        { "value": "10:30", "label": "10:30" + this.arabic('am') },
        { "value": "11:00", "label": "11:00" + this.arabic('am') },
        { "value": "11:30", "label": "11:30" + this.arabic('am') },
        { "value": "12:00", "label": "12:00" + this.arabic('pm') },
        { "value": "12:30", "label": "12:30" + this.arabic('pm') },
        { "value": "13:00", "label": "01:00" + this.arabic('pm') },
        { "value": "13:30", "label": "01:30" + this.arabic('pm') },
        { "value": "14:00", "label": "02:00" + this.arabic('pm') },
        { "value": "14:30", "label": "02:30" + this.arabic('pm') },
        { "value": "15:00", "label": "03:00" + this.arabic('pm') },
        { "value": "15:30", "label": "03:30" + this.arabic('pm') },
        { "value": "16:00", "label": "04:00" + this.arabic('pm') },
        { "value": "16:30", "label": "04:30" + this.arabic('pm') },
        { "value": "17:00", "label": "05:00" + this.arabic('pm') },
        { "value": "17:30", "label": "05:30" + this.arabic('pm') },
        { "value": "18:00", "label": "06:00" + this.arabic('pm') },
        { "value": "18:30", "label": "06:30" + this.arabic('pm') },
        { "value": "19:00", "label": "07:00" + this.arabic('pm') },
        { "value": "19:30", "label": "07:30" + this.arabic('pm') },
        { "value": "20:00", "label": "08:00" + this.arabic('pm') },
        { "value": "20:30", "label": "08:30" + this.arabic('pm') },
        { "value": "21:00", "label": "09:00" + this.arabic('pm') },
        { "value": "21:30", "label": "09:30" + this.arabic('pm') },
        { "value": "22:00", "label": "10:00" + this.arabic('pm') },
        { "value": "22:30", "label": "10:30" + this.arabic('pm') },
        { "value": "23:00", "label": "11:00" + this.arabic('pm') },
        { "value": "23:30", "label": "11:30" + this.arabic('pm') },
      ];
    }
    this.formData.ApproverDepartment = this.currentUser.DepartmentID;
    if(this.screenStatus === 'create'){
      this.PrintHide=false;
      this.onChangeApproverDept('approver', this.currentUser.DepartmentID);
    }else{
      this.PrintHide=true;
    }


  }

  addCoPassenger() {
    // this.formData.TripCoPassengers.push({
    //   CoPassengerDepartment: null,
    //   CoPassengerName: null,
    //   CoPassengerID: null,
    //   OthersCoPassengerName: "",
    //   CoPassengerDepartmentID: null
    // });
      let index = this.multipledropdown.length;
      this.multipledropdown.push({ index: index + 1 });
  }

  getRequestorDepartment(){
    this.vehicleservice.getVehicleRequestById(0, this.currentUser.id)
    .subscribe((response: any) => {
      if (response != null) {
        this.departmentList = response.OrganizationList;
        this.CoPassengarDepartList = response.M_CoPassengarList;
      }
    });
  }

  getApproverDepartment(){
    this.vehicleservice.getVehicleRequestById(0, this.currentUser.id)
    .subscribe((response: any) => {
      if (response != null) {
        this.departmentList = response.OrganizationList;
        this.CoPassengarDepartList = response.M_CoPassengarList;
      }
    });
  }

  getRequestorList() {
    let params = [{
      'OrganizationID': '',
      'OrganizationUnits': 'string'
    }];
    this.common.getUserList(params, 0).subscribe((data: any) => {
      this.requestorList = data;
    });
    if(this.OrgUnitID != 13){
      this.formData.Requestor = this.currentUser.id;
    }
  }


  getEmiratesList() {
    // getting emirates list
    this.adminService.getMasterData({ Type: 10 }, this.currentUser.id)
    .subscribe((emirates: any) => {
      this.emiratesList = emirates
    });
  }

  
  getDriverList() {
    // getting emirates list
    this.vehicleservice.getdriver()
    .subscribe((datas: any) => {
      this.driversList = datas.Collection;

    });
  }
  getVehicleModel() {
    // getting vehicle model list
    let list = [];
    // list.push({'VehicleModelID': 0, 'VehicleModelName': (this.lang == 'en')?'Any':this.arabic('any'), 'DisplayOrder': null});
    this.vehicleservice.getVehicleModel(this.currentUser.id)
    .subscribe((datas: any) => {
      datas.map(l => list.push(l));
      this.vehicleModelList = list;
    });
  }

  getTripDestination() {
    // getting trip destination list
    this.vehicleservice.getTripDestination(this.currentUser.id)
    .subscribe((datas: any) => {
      this.destinationList = datas
    });
  }

  getTripType() {
    // getting Trip type list
    this.vehicleservice.getTripType(this.currentUser.id)
    .subscribe((datas: any) => {
      this.tripTypeList = datas
    });
  }

  validate() {
    this.valid = true;

    if(this.RadioValue=="3"){
      if (this.utils.isEmptyString(this.formData.Requestor)
      || this.utils.isEmptyString(this.formData.TobeDrivenbyDepartmentID)
      || this.utils.isEmptyString(this.formData.TobeDrivenbyDriverID)
      || this.formData.VehicleModelID == null){
        this.valid = false;
      }
      return this.valid;
    }else{
      if (this.utils.isEmptyString(this.formData.RequestType)
      || this.utils.isEmptyString(this.formData.Requestor)
      // || this.utils.isEmptyString(this.formData.RequestDateTime)
      // || this.utils.isEmptyString(this.formData.DriverId)
      || this.utils.isEmptyString(this.formData.TripTypeID)
      || (this.formData.TripTypeID == 3 && this.utils.isEmptyString(this.formData.TripTypeOthers))
      || this.utils.isEmptyString(this.formData.Emirates)
      || this.utils.isEmptyString(this.formData.City)
      || this.utils.isEmptyString(this.formData.Destination)
      || (this.formData.Destination == 4 && this.utils.isEmptyString(this.formData.DestinationOthers))
      || this.utils.isEmptyString(this.formData.TripPeriodFromDate)
      || this.utils.isEmptyString(this.formData.TripPeriodToDate)
      || this.utils.isEmptyString(this.TripPeriodFromTime)
      || this.utils.isEmptyString(this.TripPeriodToTime)
      || this.formData.VehicleModelID == null
      || this.utils.isEmptyString(this.formData.ApproverDepartment)
      || this.utils.isEmptyString(this.formData.ApproverName))
       {
        this.valid = false;
    }
    return this.valid;
    }
  }

  validateNotes(){
    this.valid = true;
    if(this.isApprover){
      if(this.utils.isEmptyString(this.notes)){
        this.valid = false;
      }
    }
    return this.valid;
  }

  validateReason(){
    this.valid = true;
      if(this.utils.isEmptyString(this.Reason)){
        this.valid = false;
      }
    return this.valid;
  }

  validatePermanantCar() {
    this.valid = true;
    if(this.formData.RequestType == 3) {
      if(this.utils.isEmptyString(this.formData.TobeDrivenbyDepartmentID) 
      || this.utils.isEmptyString(this.formData.TobeDrivenbyDriverID)) {
        this.valid = false;
      }
    }
    return this.valid;
  }

  validateTripCoPassengers() {
    var flag = true;
    for(var i = 0; i< this.formData.TripCoPassengers.length; i++) {
      if(this.utils.isEmptyString(this.formData.TripCoPassengers[i].CoPassengerDepartment) || 
      this.utils.isEmptyString(this.formData.TripCoPassengers[i].CoPassengerID)) {
        flag = false;
      }
      return flag;
    }
  }

  onStartTimeSelect() {
    this.showValidTimeAlert = false;
    this.onEndTimeSelect();
  }

  onEndTimeSelect() {
    this.showValidTimeAlert = false;
    let startDate = new Date(this.formData.TripPeriodFromDate);
    let startDateString = this.dateToStringFormation(startDate);
    let endDate = new Date(this.formData.TripPeriodToDate);
    let endDateString = this.dateToStringFormation(endDate);
    let startTime = this.TripPeriodFromTime;
    let startHour = this.splitHour(startTime);
    let startMinutes = this.splitMinutes(startTime);
    let endTime = this.TripPeriodToTime;
    let endHour = this.splitHour(endTime);
    let endMinutes = this.splitMinutes(endTime);
    if (startDateString == endDateString) {
      if (endHour < startHour) {
        this.TripPeriodToTime = '';
        this.showValidTimeAlert = true;
        this.inProgress = true;
      } else if (endHour == startHour) {
        if (startMinutes > endMinutes) {
          this.TripPeriodToTime = '';
          this.showValidTimeAlert = true;
          this.inProgress = true;
        } else {
          this.inProgress = false;
          this.showValidTimeAlert = false;
        }
      } else {
        this.inProgress = false;
        this.showValidTimeAlert = false;
      }
    }
  }

  maxDate(days){
    // return this.util.maxDateCheck(days);
    if (this.formData.TripPeriodToDate) {
      let endDate = new Date(this.formData.TripPeriodToDate);
      this.month = endDate.getMonth()+1;
      if (this.month < 10) {
        this.month = '0' + (endDate.getMonth() + 1);
      }
      let dateLimit = (endDate.getFullYear()) + '/' + this.month + '/' + (endDate.getDate() + days);
      let dates = this.datePipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }
  minDate(days){
    // return this.util.minDateCheck(date);
    if(this.formData.TripPeriodFromDate){
      let today = new Date(this.formData.TripPeriodFromDate);
      this.month = today.getMonth()+1;
      if (today.getMonth() < 10) {
        this.month = '0' + (today.getMonth() + 1);
      }
      let dateLimit = (today.getFullYear()) + '/' + this.month + '/' + (today.getDate() + days);
      let dates = this.datePipe.transform(dateLimit, 'yyyy-MM-dd');
      return new Date(dates);
    }
  }

  dateToStringFormation(date) {
    let day = date.getDate();
    if (day < 10) {
      day = '0' + day;
    }
    let month = date.getMonth() + 1;
    if (month < 10) {
      month = '0' + month;
    }
    let year = date.getFullYear();
    let formattedDate = year + '/' + month + '/' + day;
    return formattedDate;
  }

  splitHour(hour) {
    if(hour){
      let Hour = hour.substring(0, 2);
      return Hour;
    }
  }

  splitMinutes(hour) {
    if(hour){
      let Minutes = hour.substring(3, 5);
      return Minutes;
    }
  }

  saveVehiclerequest() {
    this.inProgress = true;
    // this.formData.TripPeriodFrom = this.formData.TripPeriodFrom ? new Date(this.formData.TripPeriodFrom) : "";
    this.formData.CreatedBy = this.currentUser.id;
    this.formData.RequestDateTime = this.currentdate;
    this.formData.TripPeriodFrom = this.utils.concatDateAndTime(this.formData.TripPeriodFromDate, this.TripPeriodFromTime);
    this.formData.TripPeriodTo = this.utils.concatDateAndTime(this.formData.TripPeriodToDate, this.TripPeriodToTime);
    this.multipledropdown.forEach((data, index) => {
      if(index < this.CoPassengerDepartmentID.length && index < this.CoPassengerID.length){
        if(typeof this.CoPassengerID[index] == 'number'){
        //   this.formData.TripCoPassengers.push({
        //     "CoPassengerID": this.CoPassengerID[index]
        //   });
        // }else{
        //     this.formData.TripCoPassengers.push({
        //       "CoPassengerID": 0,
        //       "OthersCoPassengerName": this.CoPassengerID[index]
        //     });
        //   }
        this.formData.TripCoPassengers.push({
          "CoPassengerDepartment": this.CoPassengerDepartmentID[index],
          "CoPassengerDepartmentID":  this.CoPassengerDepartmentID[index],
          "CoPassengerID": this.CoPassengerID[index],
          "CoPassengerName": null,
          "OthersCoPassengerName": ""
        });
        }else{
          this.formData.TripCoPassengers.push({
            "CoPassengerDepartment": this.CoPassengerDepartmentID[index],
            "CoPassengerDepartmentID":  this.CoPassengerDepartmentID[index],
            "CoPassengerID": null,
            "CoPassengerName": null,
            "OthersCoPassengerName": this.CoPassengerID[index]
          });
        }
      }
    });
    this.vehicleservice.saveVehicleReqeset(this.formData)
    .subscribe((response: any) => {
      if(response.VehicleReqID){
      if(this.lang == "ar") {
        this.message = this.arabic('vechilerequestsubmit');
      } else {
        this.message = "Vehicle Request Submitted Successfully";
      }
      this.bsModalRef = this.modalService.show(SuccessComponent);
      this.bsModalRef.content.message = this.message;
      let newSubscriber = this.modalService.onHide.subscribe(() => {
        newSubscriber.unsubscribe();
        if (this.lang == 'en')
          this.router.navigate(['/en/app/vehicle-management/dashboard']);
        else
          this.router.navigate(['/ar/app/vehicle-management/dashboard']);
      });
      this.inProgress = false;
    }
    });
  }

  getVehicleRequestById(vehicleRequestId) {
    debugger
    this.vehicleservice.getVehicleRequestById(vehicleRequestId, this.currentUser.id)
    .subscribe((response: any) => {

      // if(this.DeptID !=3){
      //   this.PrintHide=true;
      // }
      // else{
      //   this.PrintHide=false;
      // }
      if(response.RequestType==3){
        this.HideFields=false;
      }else{
        this.HideFields=true;
      }
      this.formData = response;
      // for(var i = 0; i< response.TripCoPassengers.length; i++) {
      //   this.formData.TripCoPassengers[i].CoPassengerDepartment = parseInt(response.TripCoPassengers[i].CoPassengerDepartmentID);
      //   this.formData.TripCoPassengers[i].CoPassengerDepartmentID = parseInt(response.TripCoPassengers[i].CoPassengerDepartmentID);
      //   this.onChangeCoApproverDept(this.formData.TripCoPassengers[i].CoPassengerDepartmentID);
      // }
      this.ReferenceNumber = response.ReferenceNumber;
      this.CurrentApproverID = response.ApproverName;
      this.currentStatus = response.Status;
      this.checkIsApprover();
      this.formData.TripCoPassengers = response.TripCoPassengers;
      this.formData.DriverId = response.DriverID;
      this.formData.RequestType = response.RequestType.toString();
      this.notes = response.Notes;
      if(response.RequestType == 2){
        let requestorDriverList = [];
        let requestedDriverList = this.requestorList.find(user => user.UserID == this.formData.DriverId);
        requestorDriverList.push({'DriverID': requestedDriverList.UserID, 'DriverName': requestedDriverList.EmployeeName});
        this.driversList = requestorDriverList;
      }
      this.formData.RequestDateTime = this.datePipe.transform(new Date(response.RequestDateTime), "dd/MM/yyyy h:mm a");
      this.DisplayRequestDateTime = this.formData.RequestDateTime;
      if(this.lang == 'ar'){
        this.formData.RequestDateTime = this.datePipe.transform(new Date(response.RequestDateTime), "dd/MM/yyyy h:mm ").concat(this.datePipe.transform(new Date(response.RequestDateTime), 'a')=='AM'?this.common.arabic.words['am']:this.common.arabic.words['pm']);
        this.DisplayRequestDateTime = this.formData.RequestDateTime;
      }
      this.formData.TripPeriodFromDate = new Date(response.TripPeriodFrom);
      this.formData.TripPeriodToDate = new Date(response.TripPeriodTo);
      this.TripPeriodFromTime = this.formatAMPM(new Date(response.TripPeriodFrom));
      // if(this.lang == 'ar'){
      //   this.TripPeriodFromTime = this.datePipe.transform(new Date(response.TripPeriodFrom), "h:mm ").concat(this.datePipe.transform(new Date(response.TripPeriodFrom), 'a')=='AM'?this.common.arabic.words['am']:this.common.arabic.words['pm']);
      // }
      this.TripPeriodToTime = this.formatAMPM(new Date(response.TripPeriodTo));
      // if(this.lang == 'ar'){
      //   this.TripPeriodToTime = this.datePipe.transform(new Date(response.TripPeriodTo), "h:mm ").concat(this.datePipe.transform(new Date(response.TripPeriodTo), 'a')=='AM'?this.common.arabic.words['am']:this.common.arabic.words['pm']);
      // }
      this.formData.ApproverName = response.ApproverName;
      this.formData.TripTypeID = response.TripTypeID;
      this.formData.TripTypeOthers = response.TripTypeOthers;
      let type;
      let action;
      if(response.ApproverDepartment) {
        this.getApproverList('approver', response.ApproverDepartment);
        this.formData.ApproverDepartment = response.ApproverDepartment;
        this.formData.ApproverName = response.ApproverName;
      } 
      if(response.TobeDrivenbyDepartmentID) {
        this.getApproverList('driverdepart', response.TobeDrivenbyDepartmentID);
        this.formData.TobeDrivenbyDepartmentID = response.TobeDrivenbyDepartmentID;
        this.formData.TobeDrivenbyDriverID = response.TobeDrivenbyDriverID;
      }
      // this.onChangeApproverDept(type, action);    
      this.onChangeEmirates(response.Emirates, response.City);
      this.getCoPassengerDetails(response.TripCoPassengers);
      this.getDestinationName(response.Destination);
      // this.getTripTypeName(response.TripTypeID);
    });
  }
  ngOnDestroy(): void {
    this.PrintHide=false;
    
  }

  getCoPassengerDetails(TripCoPassengers) {
    this.multipledropdown = [];
    const passengerdept = [];
    const passengerid = [];
    TripCoPassengers.forEach((department, index)=> {
      this.multipledropdown.push({ index: index + 1 });
      if(department.CoPassengerDepartmentID == 0){
        passengerid.push(department.OthersCoPassengerName);
      }else{
      passengerid.push(department.CoPassengerID);}
      passengerdept.push(department.CoPassengerDepartmentID);
        // this.formData.TripCoPassengers[i].CoPassengerDepartmentID = parseInt(TripCoPassengers[i].CoPassengerDepartmentID);
        let params = [{
          "OrganizationID": TripCoPassengers[index].CoPassengerDepartmentID,
          "OrganizationUnits": ""
        }];
        this.vehicleservice.getUserList(params, 0).subscribe((data: any) => {
          this.CoPssApproverList[index] = data;
          // this.formData.TripCoPassengers[i].CoPassengerID = parseInt(TripCoPassengers[i].CoPassengerID);
        });
      });
      this.CoPassengerID = passengerid;
      this.CoPassengerDepartmentID = passengerdept;
  }

  checkIsApprover() {
    if ((this.CurrentApproverID == this.currentUser.id) && this.currentStatus == 218) {
      this.isApprover = true;
      this.screenTitle = "Vehicle Reqest Approval";
      this.common.breadscrumChange('Vehicle Management', 'Vehicle Reqest Approval','');
      if(this.lang == 'ar'){
        this.screenTitle = this.arabic('vehiclerequestapproval');
        this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('vehiclerequestapproval'),'');
      }
    }
  }

  onChangeEmirates(EmiratesID, city?:string) {
    this.citizenservice
      .getCityListbyID(this.currentUser.id, EmiratesID)
      .subscribe(res => {
        this.cityList = res;
        this.formData.City = city;
      });
  }

  onChangeApproverDept(type, department) {
    this.formData.TobeDrivenbyDriverID = null;
    let params = [{
      "OrganizationID": department,
      "OrganizationUnits": ""
    }];
    let id = 0;
    if(type == 'approver') {
      this.formData.ApproverName = null;
      if(this.formData.Requestor){
        id = this.formData.Requestor;
      }
      // id = this.currentUser.id;
      // if (this.CurrentApproverID == this.currentUser.id) {
      //   id = 0;
      // } 
    }
    // else {
    //   id = this.currentUser.id;
    // }
    // this.CoPssApproverList=[];
    this.common.getmemoUserList(params, id).subscribe((data: any) => {
      if(type == 'driverdepart') {
        this.toBedrivenList = data;
      } else if(type == 'approver') {
        this.approverList = data;
      }
      // this.formData.ApproverName = this.CurrentApproverID;
    });
  }

  getApproverList(type, approverDepartmentID){
    let params = [{
      "OrganizationID": approverDepartmentID,
      "OrganizationUnits": ""
    }];
    this.common.getmemoUserList(params, 0).subscribe((data: any) => {
      if(type == 'driverdepart') {
        this.toBedrivenList = data;
      } else if(type == 'approver') {
        this.approverList = data;
      }
    });
  }

  onChangeCoApproverDept(event,index) {
    // this.formData.TripCoPassengers[index].CoPassengerID = null;
    // if(this.formData.TripCoPassengers[index].CoPassengerDepartment == 0){
    //   this.formData.TripCoPassengers[index].OthersCoPassengerName = '';
    // }
    this.CoPassengerID[index] = null;
    let params = [{
      "OrganizationID": event.OrganizationID,
      "OrganizationUnits": ""
    }];
    // this.CoPssApproverList=[];
    this.vehicleservice.getUserList(params, this.currentUser.id).subscribe((data: any) => {
      this.CoPssApproverList[index] = data;
    });
  }

  radioChange(event){
    if(event.target.value == '1'){
      this.RadioValue="1"
      this.HideFields=true
      this.formData.ApproverDepartment = this.currentUser.DepartmentID;
      this.formData.ApproverName = null;
      this.DisableApproverField = false;
      this.formData.DriverId = null;
    }else if(event.target.value == '2'){
      this.RadioValue="2"
      let defaultDriverList = [];
      this.HideFields=true
      this.formData.ApproverDepartment = this.currentUser.DepartmentID;
      this.formData.ApproverName = null;
      this.DisableApproverField = false;
      defaultDriverList.push({'DriverID': this.currentUser.id, 'DriverName': this.currentUser.DisplayName});
      if(this.formData.Requestor){
      let requestedDriverList = this.requestorList.find(user => user.UserID == this.formData.Requestor);
      defaultDriverList.push({'DriverID': requestedDriverList.UserID, 'DriverName': requestedDriverList.EmployeeName});
      }
      this.driversList = defaultDriverList;
      if(this.formData.Requestor){
        this.formData.DriverId = this.formData.Requestor;
      }else{
        this.formData.DriverId = this.currentUser.id;
      }
    }else if(event.target.value == '3'){
      this.RadioValue="3"
      this.DisableApproverField = true;
      this.formData.TripTypeID = null;
      this.formData.Emirates = null;
      this.formData.City = null;
      this.formData.Destination = null;
      this.formData.TripPeriodFromDate = null;
      this.TripPeriodFromTime = "";
      this.formData.TripPeriodToDate = null;
      this.TripPeriodToTime = "";
      this.formData.TripPeriodFromDate = null;
      this. CoPassengerDepartmentID = [];this.CoPassengerID=[];
      this.HideFields=false
      this.formData.TobeDrivenbyDepartmentID = null;
      this.formData.TobeDrivenbyDriverID = null;
      this.toBedrivenList = [];
      this.formData.ApproverDepartment = 3;
      let params = [{
        "OrganizationID": 3,
        "OrganizationUnits": ""
      }];
      let vehicleTeamApproverID;
      this.vehicleservice.getUserList(params, 0).subscribe((data: any) => {
        this.approverList = data;
        vehicleTeamApproverID = data.filter(x => x.IsHOD == true);
        this.formData.ApproverName = vehicleTeamApproverID[0].UserID;
      });
      
    }
  }

  onChangeRequestor(requestor){
    if(this.formData.RequestType == '2'){
      if(this.formData.Requestor){
        let defaultDriverList = [];
        let requestedDriverList = this.requestorList.find(user => user.UserID == this.formData.Requestor);
        defaultDriverList.push({'DriverID': requestedDriverList.UserID, 'DriverName': requestedDriverList.EmployeeName});
        this.driversList = defaultDriverList;
        this.formData.DriverId = this.formData.Requestor;
      }else{
      this.formData.DriverId = this.currentUser.id;
      }
    }
  }

  setCoPassengerID(index, CoPassengerID) {
    if(CoPassengerID) {
      let selectedCoPassenger = _.find(this.CoPssApproverList, function (item) { return (item.UserID == CoPassengerID) });
      this.formData.TripCoPassengers[index].CoPassengerID = selectedCoPassenger.UserID
      this.formData.TripCoPassengers[index].CoPassengerName = selectedCoPassenger.EmployeeName
    }
  }

  getTripTypeName(id) {
    this.formData.TripTypeOthers = null;
    if(this.tripTypeList && this.tripTypeList.length >0) {
      let selectedVal = _.find(this.tripTypeList, function (item) { return (item.TripTypeID == id) });
      this.tripTypename = selectedVal.TripTypeName
    }
  }

  getDestinationName(id) {
    if(this.destinationList && this.destinationList.length >0) {
      let selectedVal = _.find(this.destinationList, function (item) { return (item.TripDestinationID == id) });
      this.DestinationName = selectedVal.TripDestinationName
    }
  }

  showCancelModal(template){
    this.bsModalRef = this.modalService.show(template);
  }

  closemodal() {
    this.bsModalRef.hide();
  }
  
  updateAction(action: string) {
    this.inProgress = true;
    if(action != "Cancel"){
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "replace"
      }, {
        "value": this.notes,
        "path": "Notes",
        "op": "replace",
      }, {
        "value": this.currentUser.id,
        "path": "UpdatedBy",
        "op": "replace"
      }, {
        "value": new Date(),
        "path": "UpdatedDateTime",
        "op": "replace"
      }
    ];
    this.updateRequest(dataToUpdate, action);
  }else{
    this.closemodal();
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "replace"
      }, {
        "value": this.Reason,
        "path": "Reason",
        "op": "replace",
      }, {
        "value": this.currentUser.id,
        "path": "UpdatedBy",
        "op": "replace"
      }, {
        "value": new Date(),
        "path": "UpdatedDateTime",
        "op": "replace"
      }
    ];
    this.updateRequest(dataToUpdate, action);
  }
  }

  updateRequest(dataToUpdate: any, action: string) {
    if (this.lang == 'en') {
      switch (action) {
        case 'Approve':
          this.message = "Vehicle Request Approved Successfully";
          break;
        case 'Reject':
          this.message = "Vehicle Request Rejected Successfully";
          break;
        case 'Cancel':
          this.message = "Vehicle Request Cancelled Successfully";
          break;
      }
    } else {
      switch (action) {
        case 'Approve':
          this.message = this.arabic('vehiclerequestapprove');
          break;
        case 'Reject':
          this.message = this.arabic('vehiclerequestreject');
          break;
        case 'Cancel':
          this.message = this.arabic('vehiclereqeustcancel');
          break;
      }
    }

    this.vehicleservice.updateRequest(this.vehicleRequestId, dataToUpdate)
      .subscribe((response: any) => {
        if (response.VehicleReqID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            if (this.common.language == 'English')
            this.router.navigate(['/en/app/vehicle-management/dashboard']);
            else
            this.router.navigate(['/ar/app/vehicle-management/dashboard']);
          });
        }
        this.inProgress = false;
      });
  }
  formatAMPM(date) {
    let time;
    let mins;
    let hours;
    mins = date.getMinutes();
    hours = date.getHours();
    mins = (parseInt(mins) % 60) < 10 ? '0' + (parseInt(mins) % 60) : (parseInt(mins) % 60);
    hours = (parseInt(hours) % 60) < 10 ? '0' + (parseInt(hours) % 60) : (parseInt(hours) % 60);
    time = hours+":"+ mins;
    return time;
  }

  // --Print Return form--

  print(template: TemplateRef<any>, buttonclicktext) {
    this.inProgress = true;
    if(buttonclicktext == 'Release'){
      this.isReturnForm = false;
    }else{
      this.isReturnForm = true;
    }
    this.vehicleservice.printPreview('vehicleRequest/preview', this.vehicleRequestId, this.currentUser.id, this.isReturnForm).subscribe(res => {
      if (res) {
        this.vehicleservice.pdfToJson(this.ReferenceNumber).subscribe((data: any) => {
          this.showPdf = true;
          this.pdfSrc = data;
          this.bsModalRef = this.modalService.show(template, { class: 'modal-xl' });
          this.inProgress = false;
          this.common.deleteGeneratedFiles('files/delete', this.ReferenceNumber + '.pdf').subscribe(result => {
            console.log(result);
          });
        });
      }
    });
  }

  printPdf(html: ElementRef<any>) {
    debugger
    this.inProgress = false;
    // let isReturnForm = false;
    // if(this.screenStatus == 'return'){
    //   isReturnForm = true;
    // }
    this.vehicleservice.printPreview('vehicleRequest/preview', this.vehicleRequestId, this.currentUser.id, this.isReturnForm).subscribe(res => {
      if (res) {
        this.common.printPdf(this.ReferenceNumber);
      }
    });
  }

  downloadPrint() {
    this.inProgress = true;
    // let isReturnForm = false;
    // if(this.screenStatus == 'return'){
    //   isReturnForm = true;
    // }
    this.vehicleservice.printPreview('vehicleRequest/preview', this.vehicleRequestId, this.currentUser.id, this.isReturnForm).subscribe(res => {
      if (res) {
        this.common.previewPdf(this.ReferenceNumber)
          .subscribe((data: Blob) => {
            this.inProgress = false;
            var url = window.URL.createObjectURL(data);
            var a = document.createElement('a');
            document.body.appendChild(a);
            a.setAttribute('style', 'display: none');
            a.href = url;
            a.download = this.ReferenceNumber + '.pdf';
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
            this.common.deleteGeneratedFiles('files/delete', this.ReferenceNumber + '.pdf').subscribe(result => {
              console.log(result);
            });
          });
      }
    });
  }

  closePrintPop() {
    this.inProgress = false;
    this.bsModalRef.hide();
  }

  // --Print Return form--



 








  arabic(word) {
    return this.common.arabic.words[word];
  }
}
