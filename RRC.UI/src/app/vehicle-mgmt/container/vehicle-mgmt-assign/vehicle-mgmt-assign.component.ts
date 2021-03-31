import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { DatePipe, DOCUMENT } from '@angular/common';
import { BsDatepickerConfig, BsModalService, BsModalRef } from 'ngx-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleMgmtServiceService } from '../../service/vehicle-mgmt-service.service';
import * as _ from 'lodash';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { CitizenAffairService } from 'src/app/citizen-affair/service/citizen-affair.service';
import { data } from 'src/app/task/container/component/task-dashboard/data';

@Component({
  selector: 'app-vehicle-mgmt-assign',
  templateUrl: './vehicle-mgmt-assign.component.html',
  styleUrls: ['./vehicle-mgmt-assign.component.scss']
})
export class VehicleMgmtAssignComponent implements OnInit {
  rows:any=[];
  trip_same_city: string;
  other_trip: string;
  bsConfig: Partial<BsDatepickerConfig>;
  protected bsModalRef: BsModalRef;
  tableMessages: { emptyMessage: any; };
  columns: any;
  drivercolumns: any;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  public config:any = {
    paging: true,
    sorting: {columns: this.columns},
    filtering: {filterString: ''},
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalItems: []
  };

  public config1:any = {
    paging: true,
    sorting: {columns: this.drivercolumns},
    filtering: {filterString: ''},
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalDriverItems: []
  };
  from: any;
  to: any;
  requesterName: any;
  city: any;
  destination: any;
  driverList: any;
  driver: any;
  screenStatus:any;
  timeList: Array<any>;
  formLabels: {
    title: any
    from: any,
    to: any,
    requestname: any,
    city: any,
    destination: any,
    drivername: any,
    assign: any
  }
  vehicleRequestId: number;
  screenTitle: string;
  lang: any;
  requestorDestination: any;
  requestorCity: any;
  requestorName: any;
  TripPeriodTo: any;
  TripPeriodFrom: any;
  TripPeriodFromTime: any;
  TripPeriodToTime: any;
  DriverId = null;
  EnableAssignButton:boolean=true;
  inProgress: boolean=false;
  message: any;
  driverRows: any=[];
  isDriverHaveOtherTripsOnSamePeriod: number;
  constructor(public citizenservice: CitizenAffairService, public vehicleservice: VehicleMgmtServiceService, public datePipe: DatePipe, private route: ActivatedRoute, public router: Router,public common: CommonService, protected modalService: BsModalService) { }
  ngOnInit() {
    this.lang = this.common.currentLang;
    // Language changer....
    this.languageSupport();
    this.isDriverHaveOtherTripsOnSamePeriod = 0;
    this.route.params.subscribe(param => {
      this.vehicleRequestId = +param.id;
      if (this.vehicleRequestId > 0) {
        this.getVehicleRequestById(this.vehicleRequestId); // load vehicle request by ID
          this.screenTitle = "Vehicle Reqest Approval";
          if(this.lang == 'ar'){
            this.screenTitle = this.arabic('vehiclerequestapproval');
          }
      }
    });
    this.getDriverList();
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
  }
  // Language changer....

  languageSupport() {
    if (this.common.language == "English")
      this.common.breadscrumChange("Vehicle Management", "Assign", '');
    else
      this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('assign'), '');

    this.trip_same_city = (this.common.language == 'English') ?
      "Trips on the same period (From) and same city" :
      this.arabic('thisonthesameperiodfromandsamecity');

    this.other_trip = (this.common.language == 'English') ?
      "Other Trips assigned to the driver on the same day" :
      this.arabic('othertripsassignedtothedriveronthesameday');

    this.tableMessages = {
      emptyMessage: (this.common.language == 'English') ? 'No data to display' : this.arabic('nodatatodisplay')
    };

    this.columns = [
      { name: (this.lang == 'en') ? 'Requestor': this.arabic('requestor'), prop: 'Requestor' },
      { name: (this.lang == 'en') ? 'Driver Name': this.arabic('drivername'), prop: 'DriverName' },
      { name: (this.lang == 'en') ? 'Destination': this.arabic('destination'), prop: 'Destination' },
      { name: (this.lang == 'en') ? 'Co-Passengers': this.arabic('copassengers'), prop: 'CoPassenger' }
    ];

    this.drivercolumns = [
      { name: (this.lang == 'en') ? 'Trip Time - From': this.arabic('tripperiodfrom'), prop: 'TripTimeFrom' },
      { name: (this.lang == 'en') ? 'Trip Time - To': this.arabic('tripperiodto'), prop: 'TripTimeTo' },
      { name: (this.lang == 'en') ? 'City': this.arabic('city'), prop: 'City' },
      { name: (this.lang == 'en') ? 'Destination': this.arabic('destination'), prop: 'Destination' },
      { name: (this.lang == 'en') ? 'Requestor': this.arabic('requestor'), prop: 'Requestor' }
    ];

    this.formLabels = {
      title: (this.common.language == "English") ? "ASSIGN TRIP" : this.arabic('assigntrip'),
      from: (this.common.language == "English") ? "From" : this.arabic('from'),
      to: (this.common.language == "English") ? "To" : this.arabic('to'),
      requestname: (this.common.language == "English") ? "Requestor Name" : this.arabic('requestorname'),
      city: (this.common.language == "English") ? "City" : this.arabic('city'),
      destination: (this.common.language == "English") ? "Destination" : this.arabic('destination'),
      drivername: (this.common.language == "English") ? "Driver's Name" : this.arabic('drivername'),
      assign: (this.common.language == "English") ? "ASSIGN" : this.arabic('assign')
    }

  }
  getDriverList() {
    // getting emirates list
    this.vehicleservice.getdriver()
    .subscribe((datas: any) => {
      this.driverList = datas.Collection
    });
  }
  
  getVehicleRequestById(vehicleRequestId) {
    this.vehicleservice.getVehicleRequestById(vehicleRequestId, this.currentUser.id)
    .subscribe((response: any) => {
      this.DriverId = response.DriverID;
      this.TripPeriodFrom = new Date(response.TripPeriodFrom);
      this.TripPeriodTo = new Date(response.TripPeriodTo);
      this.TripPeriodFromTime = this.formatAMPM(new Date(response.TripPeriodFrom));
      this.TripPeriodToTime = this.formatAMPM(new Date(response.TripPeriodTo));
      this.rows = response.M_TripOnSameDay;
      // this.config.totalItems = response.count;

      this.getRequestorName(response.Requestor)
      this.getDestinationName(response.Destination, response.DestinationOthers);
      this.getCityName(response.Emirates, response.City)
      this.getVehicleDriverTrips(response.DriverID);
    });
  }

  getRequestorName(id) {
    let params = [{
      'OrganizationID': '',
      'OrganizationUnits': 'string'
    }];
    this.common.getUserList(params, 0).subscribe((data: any) => {
      let selectedVal = _.find(data, function (item) { return (item.UserID == id) });
      this.requestorName = selectedVal.EmployeeName;
    });
  }

  getDestinationName(id, otherString) {
    if(id == 4){
      this.requestorDestination = otherString;
    }else{
      this.vehicleservice.getTripDestination(this.currentUser.id)
      .subscribe((datas: any) => {
        let selectedVal = _.find(datas, function (item) { return (item.TripDestinationID == id) });
        this.requestorDestination = selectedVal.TripDestinationName;
      });
    }
  }

  getVehicleDriverTrips(id) {
    this.isDriverHaveOtherTripsOnSamePeriod = 0;
    if(id) {
      this.inProgress = true;
      this.vehicleservice.getVehicleDriverTrips(id, this.vehicleRequestId)
      .subscribe((datas: any) => {
        if(datas.length){
        this.isDriverHaveOtherTripsOnSamePeriod = 0;
        this.inProgress = false;
        this.driverRows = datas;
        this.driverRows.forEach(val => {
          val['checkTripTimeFrom'] = val.TripTimeFrom;
          val['checkTripTimeTo'] = val.TripTimeTo;
        });
        if(this.lang == 'en'){
          this.driverRows.map(res => res.TripTimeFrom = this.datePipe.transform(res.TripTimeFrom, "dd/MM/yyyy,h:mma"));
          this.driverRows.map(res => res.TripTimeTo = this.datePipe.transform(res.TripTimeTo, "dd/MM/yyyy,h:mma"));
        }
        if(this.lang == 'ar'){
          this.driverRows.map(res => res.TripTimeFrom = this.datePipe.transform(res.TripTimeFrom, "h:mm,dd/MM/yyyy").concat(this.datePipe.transform(res.TripTimeFrom, 'a')=='AM'?this.common.arabic.words['am']:this.common.arabic.words['pm']));
          this.driverRows.map(res => res.TripTimeTo = this.datePipe.transform(res.TripTimeTo, "h:mm,dd/MM/yyyy").concat(this.datePipe.transform(res.TripTimeTo, 'a')=='AM'?this.common.arabic.words['am']:this.common.arabic.words['pm']));
        }
      }else{
        this.inProgress = false;
        this.driverRows = [];
        this.isDriverHaveOtherTripsOnSamePeriod = 0;
      }
      });

      this.EnableAssignButton=false;
    }else {
      this.driverRows = [];
      this.EnableAssignButton=true;
    }
    
  }

  getRowClass = (row) => {
    let isSameDay, assignedFromTrips, assignedToTrips, actualFromTrip, actualToTrip;
    let isSameDayTripThere = 0;
    assignedFromTrips = new Date(row.checkTripTimeFrom);
    assignedToTrips =  new Date(row.checkTripTimeTo);
    actualFromTrip = this.TripPeriodFrom;
    actualToTrip = this.TripPeriodTo;
    if((assignedFromTrips >= actualFromTrip || assignedToTrips >= actualFromTrip) && (assignedFromTrips <= actualToTrip || assignedToTrips <= actualToTrip)){
      isSameDay = true;
      this.isDriverHaveOtherTripsOnSamePeriod = this.isDriverHaveOtherTripsOnSamePeriod + 1;
    }else{ 
      isSameDay = false; 
    }
   return {
     'row-color': isSameDay
   };
  }

  
  getCityName(EmiratesID, cityId) {
    this.citizenservice
      .getCityListbyID(this.currentUser.id, EmiratesID)
      .subscribe((datas:any) => {
        let selectedVal = _.find(datas, function (item) { return (item.CityID == cityId) });
        this.requestorCity = selectedVal.CityName;  
      });
  }

  updateAction(action: string) {
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "replace"
      }, {
        "value": this.DriverId,
        "path": "DriverID",
        "op": "replace"
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

  updateRequest(dataToUpdate: any, action: string) {

    this.inProgress = true;
    if (this.lang == 'en') {
      switch (action) {
        case 'Assign':
          this.message = "Vehicle Request Assigned Successfully";
          break;
      }
    } else {
      switch (action) {
        case 'Assign':
          this.message = this.arabic('vehiclerequestassignmsg');
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
  
  onChangePage(page){

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


  arabic(word) {
    return this.common.arabic.words[word];
  }

}
