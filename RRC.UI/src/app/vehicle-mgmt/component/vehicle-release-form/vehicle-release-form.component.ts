import { Component, OnInit, TemplateRef, ViewChild, Input, ElementRef } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { VehicleMgmtServiceService } from '../../service/vehicle-mgmt-service.service';
import { CitizenAffairService } from 'src/app/citizen-affair/service/citizen-affair.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService, BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { UtilsService } from 'src/app/shared/service/utils.service';
import * as _ from 'lodash';
import { debug } from 'util';
import { DatePipe, DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-vehicle-release-form',
  templateUrl: './vehicle-release-form.component.html',
  styleUrls: ['./vehicle-release-form.component.scss']
})
export class VehicleReleaseFormComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('template') template: TemplateRef<any>;
  @ViewChild('printContent') printContent: ElementRef<any>;
  bsConfig: Partial<BsDatepickerConfig>= {
    dateInputFormat:'DD/MM/YYYY'
  };
  bsModalRef: BsModalRef;
  lang: any;
  columns: any = [];
  returnColumns: any = []
  rows: any = [];
  carissues = [];
  personalbelonging: any;
  vehicleRequestId: number;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  inProgress: boolean = false;
  message: string;
  PlateNumber = "";
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalItems: []
  };
  page = 1;
  maxSize = 10;
  length = 0;
  releaseDate: any;
  releaseDateTime: any;
  releaseTime: any;
  lastMileageReading: number;
  notes = "";
  vehicleId: any;
  valid: boolean = false;
  LastMileageReading: any;
  DriverID: any;
  ReleaseLocationID: any;
  ReturnDate = null;
  ReturnDateTime = null;
  returnTime: any;
  personalbelongingText = "";
  DriverName = "";
  currentMileageReading: number;
  ReturnLocationID: any;
  VehicleID: any;
  @Input() screenStatus: string;
  Status: any;
  requestType: any;
  vehicleDetails: any = [];
  PersonalBelongingsText: any;
  HavePersonalBelongings: any;
  TripVehicleIssues: any = [];
  selectedCarIssues: any = [];
  releaseReturnCity: Array<any>;
  releaseCityName: string;
  returnCityName: string;
  vehicleIssues = "";
  tableMessages: { emptyMessage: any; };
  timeList: Array<any>;
  Requestor: any;
  pdfSrc: string;
  showPdf: boolean = false;
  ReferenceNumber: any;
  selected = []; selecteditem: any = 0;
  selectedIndex:any;
  constructor(public utils: UtilsService, public datePipe: DatePipe, public citizenservice: CitizenAffairService, public vehicleservice: VehicleMgmtServiceService, private route: ActivatedRoute, public router: Router, public common: CommonService, protected modalService: BsModalService) {

  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.releaseDate = new Date();
    if (this.screenStatus == "return") {
      this.ReturnDate = new Date();
      this.returnTime = null;
      this.common.breadscrumChange('Vehicle Management', 'Return', '');
      this.common.topBanner(true, false, '', '');
      if (this.lang == 'ar') {
        this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('return'), '');
      }
      this.common.topBanner(false, false, '', '');
    } else if (this.screenStatus == "release") {
      this.common.breadscrumChange('Vehicle Management', 'Release', '');
      this.common.topBanner(true, false, '', '');
      if (this.lang == 'ar') {
        this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('release'), '');
      }
      this.common.topBanner(false, false, '', '');
    } else if (this.screenStatus == "releaseConfirm") {
      this.common.breadscrumChange('Vehicle Management', 'Vehicle Release Confirmation', '');
      this.common.topBanner(true, false, '', '');
      if (this.lang == 'ar') {
        this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('vehiclereleaseconfirmation'), '');
      }
      this.common.topBanner(false, false, '', '');
    } else if (this.screenStatus == "returnConfirm") {
      this.common.breadscrumChange('Vehicle Management', 'Vehicle Return Confirmation', '');
      this.common.topBanner(true, false, '', '');
      if (this.lang == 'ar') {
        this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('vehiclereturnconfirmation'), '');
      }
      this.common.topBanner(false, false, '', '');
    } else {
      this.common.breadscrumChange('Vehicle Management', 'Release', '');
      this.common.topBanner(true, false, '', '');
      if (this.lang == 'ar') {
        this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('release'), '');
      }
      this.common.topBanner(false, false, '', '');
    }
    this.columns = [
      { name: '#', prop: 'No' },
      { name: (this.lang == 'en') ? 'Vehicle Make' : this.arabic('vehiclemake'), prop: 'VehicleMake' },
      // { name: (this.lang == 'en') ? 'Vehicle Name' : this.arabic('vehiclename'), prop: 'VehicleName' },
      { name: (this.lang == 'en') ? 'Plate Colour' : this.arabic('platenumber'), prop: 'PlateColour' },
      { name: (this.lang == 'en') ? 'Action' : this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
    ];
    this.returnColumns = [
      { name: (this.lang == 'en') ? 'Plate Number' : this.arabic('platenumber'), prop: 'PlateNumber' },
      { name: (this.lang == 'en') ? 'Vehicle Make' : this.arabic('vehiclemake'), prop: 'VehicleMake' },
      { name: (this.lang == 'en') ? 'Vehicle Model' : this.arabic('vehiclemodel'), prop: 'ModelName' },
      { name: (this.lang == 'en') ? 'Plate Colour' : this.arabic('platecolour'), prop: 'PlateColour' },
    ];

    this.releaseReturnCity = [
      { 'name': (this.lang == 'en') ? 'Abu Dhabi' : this.arabic('abhudabhi'), 'value': '1' },
      { 'name': (this.lang == 'en') ? 'Madinat Zayed' : this.arabic('madinatzayed'), 'value': '2' }];

    this.route.params.subscribe(param => {
      this.vehicleRequestId = +param.id;
      if (this.vehicleRequestId > 0) {
        this.getVehicleRequestById(this.vehicleRequestId); // load vehicle request by ID
        // this.screenTitle = "Vehicle Reqest Approval";
        if (this.lang == 'ar') {
          // this.screenTitle = this.arabic('vehiclerequestapproval');
        }
      }
    });
    if (this.screenStatus == 'release') {
      this.getVehicleIssues(this.TripVehicleIssues);
    }
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabic('nodatatodisplay')
    };

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

  getVehicleRequestById(vehicleRequestId) {
    this.ReleaseLocationID = '2';
    this.ReturnLocationID = '2';
    this.vehicleservice.getVehicleRequestById(vehicleRequestId, this.currentUser.id)
      .subscribe((response: any) => {
        this.ReferenceNumber = response.ReferenceNumber;
        this.Requestor = response.Requestor;
        this.Status = response.Status;
        this.DriverID = response.DriverID;
        this.requestType = response.RequestType;
        if (this.screenStatus != 'release') {
          this.TripVehicleIssues = response.TripVehicleIssues;
          this.lastMileageReading = response.LastMileageReading;
          if (response.ReleaseLocationID != null) {
            this.ReleaseLocationID = response.ReleaseLocationID.toString();
          }
          if (response.ReleaseLocationID) {
            this.releaseCityName = this.getCityName(response.ReleaseLocationID)
          }
          if (response.ReturnLocationID) {
            this.returnCityName = this.getCityName(response.ReturnLocationID)
          }
          if (response.ReturnLocationID != null) {
            this.ReturnLocationID = response.ReturnLocationID.toString();
          }
          this.PersonalBelongingsText = response.PersonalBelongingsText;
          this.personalbelongingText = response.PersonalBelongingsText;
          // this.HavePersonalBelongings = response.HavePersonalBelongings;
          this.HavePersonalBelongings = response.HavePersonalBelongings == true ? "Yes" : "No";
          this.personalbelonging = response.HavePersonalBelongings == true ? "1" : "2";
          if (this.screenStatus == 'return' && this.Status == 212) {
            this.personalbelonging = '';
          }
          this.currentMileageReading = response.CurrentMileageReading;
          this.releaseDate = new Date(response.ReleaseDateTime);
          // if (this.screenStatus == 'releaseConfirm' || this.screenStatus == 'returnConfirm' || this.screenStatus == 'return') {
          //   this.releaseDate = this.datePipe.transform(response.ReleaseDateTime, "dd/MM/yyyy");
          // }
          this.releaseTime = this.formatAMPM(new Date(response.ReleaseDateTime));
          // if(this.lang == 'ar'){
          //   this.releaseTime = this.datePipe.transform(new Date(response.ReleaseDateTime), "h:mm ").concat(this.datePipe.transform(new Date(response.ReleaseDateTime), 'a')=='AM'?this.common.arabic.words['am']:this.common.arabic.words['pm']);
          // }
          if (response.ReturnDateTime != null) {
            this.ReturnDate = new Date(response.ReturnDateTime);
            // if (this.screenStatus == 'returnConfirm') {
            //   this.ReturnDate = this.datePipe.transform(response.ReturnDateTime, "dd/MM/yyyy");
            // }
            this.returnTime = this.formatAMPM(new Date(response.ReturnDateTime));
          }
          // if(this.lang == 'ar'){
          //   this.returnTime = this.datePipe.transform(new Date(response.ReturnDateTime), "h:mm ").concat(this.datePipe.transform(new Date(response.ReturnDateTime), 'a')=='AM'?this.common.arabic.words['am']:this.common.arabic.words['pm']);
          // }
          this.vehicleId = response.VehicleID;
          this.notes = response.Notes
          this.getVehicleList(response.VehicleID);
          this.getVehicleIssues(this.TripVehicleIssues);
        }
        this.getDriverList(response.DriverID);
      });
  }

  getCityName(id) {
    return _.find(this.releaseReturnCity, function (item) { return (item.value == id) }).name;
  }
  setRow(_index: number){
    this.selectedIndex = _index;
  }

  selectVechile(value) {
    this.vehicleId = value.VehicleID;
    this.lastMileageReading = value.CurrentMileage;
    this.getVehicleIssues(value.VehicleIssues);
  }
  getDriverList(driverId) {
    // getting getDriverList
    if (driverId) {
      this.vehicleservice.getdriver()
        .subscribe((datas: any) => {
          let selectedVal = _.find(datas.Collection, function (item) { return (item.DriverID == driverId) });
          this.DriverName = selectedVal ? selectedVal.DriverName : "";
        });
    }
  }

  onActivate(event) {
    let rowItem = event.row;
    let rowIndex = this.rows.indexOf(rowItem);
    if (event.type === 'keydown' && (event.event.code === 'ArrowDown')) {
      // Get Selected Row Index
      this.selecteditem = 1 + rowIndex;
      this.selected = [this.rows[this.selecteditem]]
    } else if (event.type === 'keydown' && (event.event.code === 'ArrowUp')) {
      // Get Selected Row Index
      this.selecteditem = rowIndex - 1;
      this.selected = [this.rows[this.selecteditem]]
    } else if (event.type == 'click') {
      let row = event.row;
      debugger
      // Get Selected Row Index
      this.selecteditem = this.rows.indexOf(row);
      this.selected = [this.rows[this.selecteditem]]

    }
  }


  search(VehicleID) {
    debugger
    var plateNo = "";
    if (this.PlateNumber == undefined || this.PlateNumber == "") {
      plateNo = "All"
    } else {
      plateNo = this.PlateNumber
    }
    var param = {
      plateNumber: plateNo,
      VehicleID: VehicleID
    }
    this.rows = [];
    if (plateNo && plateNo.replace(/\s/g, "")) {
      this.vehicleservice.getFineACarList('FineCarList', param)
        .subscribe((response: any) => {
          this.rows = response.Collection;
          this.vehicleDetails = this.rows[0];
          this.vehicleId = this.vehicleDetails.VehicleID;
          let i = 0;
          this.rows.map(res => {
            this.rows[i]['No'] = i + 1;
            i++;
          });
          this.length = response.Count;
        });
    }
  }

  getVehicleList(VehicleID) {
    var param = {
      plateNumber: this.PlateNumber,
      VehicleID: VehicleID
    }
    this.rows = [];
    this.vehicleservice.getFineACarList('FineCarList', param)
      .subscribe((response: any) => {
        this.rows = response.Collection;
        this.vehicleDetails = this.rows[0];
        this.vehicleId = this.vehicleDetails.VehicleID;
        let i = 0;
        this.rows.map(res => {
          this.rows[i]['No'] = i + 1;
          i++;
        });
        this.length = response.Count;
      });
  }

  getVehicleIssues(currentVehicleIssues) {
    this.carissues = [];
    this.vehicleservice.getVehicleIssues(this.currentUser.id)
      .subscribe((response: any) => {
        this.carissues = response;
        for (let i = 0; i < this.carissues.length; i++) {
          this.carissues[i].checked = false;
          for (let j = 0; j < currentVehicleIssues.length; j++) {
            if (currentVehicleIssues[j].IssueID == this.carissues[i].IssueID) {
              this.carissues[i].checked = true;
              this.carissues[i].disabled = true;
            }
          }
        }
        if (this.carissues && this.carissues.length > 0) {
          this.vehicleIssues = this.carissues.filter((ch) => {
            return ch.checked
          }).map((ch) => { return ch.IssueName; }).toString();
        }
      });
  }

  updateAction(action: string) {
    this.inProgress = true;
    // if(action != 'ReleaseConfirm'){
    this.releaseDateTime = this.utils.concatDateAndTime(this.releaseDate, this.releaseTime);
    // }
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "replace"
      }, {
        "value": this.vehicleId,
        "path": "VehicleID",
        "op": "replace"
      }, {
        "value": this.releaseDateTime,
        "path": "ReleaseDateTime",
        "op": "replace"
      }, {
        "value": this.lastMileageReading,
        "path": "LastMileageReading",
        "op": "replace"
      }, {
        "value": this.ReleaseLocationID,
        "path": "ReleaseLocationID",
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

  updateActionCommon(action: string) {
    this.inProgress = true;
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "replace"
      },
      {
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

  updateActionReturn(action: string) {
    this.inProgress = true;
    // if(action != 'ReturnConfirm'){
    this.releaseDateTime = this.utils.concatDateAndTime(this.releaseDate, this.releaseTime);   
    this.ReturnDateTime = this.utils.concatDateAndTime(this.ReturnDate, this.returnTime);
    // }
    const dataToUpdate = [
      {
        "value": action,
        "path": "Action",
        "op": "replace"
      }, {
        "value": this.releaseDateTime,
        "path": "ReleaseDateTime",
        "op": "replace"
      }, {
        "value": this.ReturnDateTime,
        "path": "ReturnDateTime",
        "op": "replace"
      }, {
        "value": this.currentMileageReading,
        "path": "CurrentMileageReading",
        "op": "replace"
      }, {
        "value": this.lastMileageReading,
        "path": "LastMileageReading",
        "op": "replace"
      },
      {
        "value": this.ReturnLocationID,
        "path": "ReturnLocationID",
        "op": "replace"
      },
      {
        "value": this.personalbelonging == "1" ? true : false,
        "path": "HavePersonalBelongings",
        "op": "replace"
      }, {
        "value": this.personalbelongingText,
        "path": "PersonalBelongingsText",
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
  }

  updateRequest(dataToUpdate: any, action: string) {
    if (this.lang == 'en') {
      switch (action) {
        case 'Release':
          this.message = "Vehicle Request Released Successfully";
          break;
        case 'Return':
          this.message = "Vehicle Request Returned Successfully";
          break;
        case 'ReleaseConfirm':
          this.message = "Vehicle Request Release Confirmed Successfully";
          break;
        case 'ReturnConfirm':
          this.message = "Vehicle Request Return Confirmed Successfully";
          break;
        case 'Reject':
          this.message = "Vehicle Request Rejected Successfully";
          break;
      }
    } else {
      switch (action) {
        case 'Release':
          this.message = this.arabic('vehiclerequestreleased');
          break;
        case 'Return':
          this.message = this.arabic('vehiclerequestreturn');
          break;
        case 'ReleaseConfirm':
          this.message = this.arabic('vehiclerequestreleaseconfirm');
          break;
        case 'ReturnConfirm':
          this.message = this.arabic('vehiclerequestreturnconfirm');
          break;
        case 'Reject':
          this.message = this.arabic('vehiclerequestreject');
          break;

      }
    }

    this.vehicleservice.updateRequest(this.vehicleRequestId, dataToUpdate)
      .subscribe((response: any) => {
        if (response.VehicleReqID) {
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          this.inProgress = false;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            if (this.common.language == 'English')
              this.router.navigate(['/en/app/vehicle-management/dashboard']);
            else
              this.router.navigate(['/ar/app/vehicle-management/dashboard']);
          });
        }
      });
  }

  addIssues(index) {
    if (this.carissues[index].checked) {
      this.carissues[index].checked = false;
    } else {
      this.carissues[index].checked = true;
    }
  }

  formatAMPM(date) {
    let time;
    let mins;
    let hours;
    mins = date.getMinutes();
    hours = date.getHours();
    mins = (parseInt(mins) % 60) < 10 ? '0' + (parseInt(mins) % 60) : (parseInt(mins) % 60);
    hours = (parseInt(hours) % 60) < 10 ? '0' + (parseInt(hours) % 60) : (parseInt(hours) % 60);
    time = hours + ":" + mins;
    return time;
  }

  sendVehicleIssues(action) {
    this.inProgress = true;
    let val = []
    let selectedCarIssues = this.carissues.filter((ch) => { return ch.checked }).map((ch) => { return ch });
    for (let i = 0; i < selectedCarIssues.length; i++) {
      val.push({
        VehicleReqID: this.vehicleRequestId,
        IssueID: selectedCarIssues[i].IssueID,
        DeleteFlag: false
      })
    }
    this.vehicleservice.sendVehicleIssues(this.vehicleRequestId, val)
      .subscribe((response: any) => {
        switch (action) {
          case 'Release':
            this.updateAction('Release');
            break;
          case 'Return':
            this.updateActionReturn('Return');
            break;
        }
      });
  }

  validateMileage() {
    if (this.currentMileageReading) {
      if (this.currentMileageReading <= this.lastMileageReading) {
        return true;
      }
    }
    return false;
  }

  validate() {
    this.valid = true;
    if (this.utils.isEmptyString(this.vehicleId)
      || (!this.releaseDate)
      || (!this.releaseTime)
      || (this.utils.isEmptyString(this.ReleaseLocationID))
      || (!this.lastMileageReading)) {
      this.valid = false;
    }
    return this.valid;
  }

  print(template: TemplateRef<any>) {
    this.inProgress = true;
    let isReturnForm = false;
    if (this.screenStatus == 'return') {
      isReturnForm = true;
    }
    this.vehicleservice.printPreview('vehicleRequest/preview', this.vehicleRequestId, this.currentUser.id, isReturnForm).subscribe(res => {
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
    this.inProgress = false;
    let isReturnForm = false;
    if (this.screenStatus == 'return') {
      isReturnForm = true;
    }
    this.vehicleservice.printPreview('vehicleRequest/preview', this.vehicleRequestId, this.currentUser.id, isReturnForm).subscribe(res => {
      if (res) {
        this.common.printPdf(this.ReferenceNumber);
      }
    });
  }

  downloadPrint() {
    this.inProgress = true;
    let isReturnForm = false;
    if (this.screenStatus == 'return') {
      isReturnForm = true;
    }
    this.vehicleservice.printPreview('vehicleRequest/preview', this.vehicleRequestId, this.currentUser.id, isReturnForm).subscribe(res => {
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

  validateReturn() {
    this.valid = true;
    if (
      (!this.ReturnDate) ||
      (!this.returnTime) ||
      this.utils.isEmptyString(this.ReturnLocationID) ||
      (!this.currentMileageReading)
      || this.utils.isEmptyString(this.personalbelonging)
    ) {
      this.valid = false;
    }
    return this.valid;
  }

  validateReturnPersonalBelonging() {
    this.valid = true;
    if (this.personalbelonging == "1") {
      if (this.utils.isEmptyString(this.personalbelongingText)) {
        this.valid = false;
      }
    }
    return this.valid;
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }
}