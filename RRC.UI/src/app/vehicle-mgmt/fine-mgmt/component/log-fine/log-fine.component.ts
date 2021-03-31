import { Component, OnInit, ViewChild, TemplateRef, Input } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { VehicleMgmtServiceService } from 'src/app/vehicle-mgmt/service/vehicle-mgmt-service.service';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilsService } from 'src/app/shared/service/utils.service';
import {Renderer2 } from '@angular/core';

@Component({
  selector: 'app-log-fine',
  templateUrl: './log-fine.component.html',
  styleUrls: ['./log-fine.component.scss']
})
export class LogFineComponent implements OnInit {
  @Input() mode;
  lang: any;
  length: any;
  maxSize: any;
  page: any;
  StatusList: Array<any> = [{ DisplayName: 'All' }];
  Status: any;
  public columns: Array<any> = [];
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('snoTemplate') snoTemplate: TemplateRef<any>;
  isApiLoading: boolean;
  rows: Array<any> = [];
  VehicleModel: any;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalItems: []
  };
  fineNumber: any;
  Date: any;
  Time: any;
  Location: any;
  BlackPoints: any;
  FineDescription: any;
  PlateNumber: any;

  formModel: {
    VehicleId: Number
    VehicleFineID: Number
    VehicleModelID: any
    FineNumber: Number
    FinedDate: any
    Location: Number
    BlackPoints: number
    Status: Number
    Description: String
    EmailTo: String
    EmailCCDepartmentID: Number
    EmailCCUserID: Number
    DeleteFlag: Number
    CreatedBy: Number
    CreatedDateTime: any
    UpdatedBy: Number
    UpdatedDateTime: any
  }
  User: any;
  message: any;
  screenStatus: string;
  vehicleFineID: any;
  showCarList: boolean = false;
  progress: boolean;
  showFineForm: boolean;
  timeList: Array<any>;
  queryParam: any;
  fromManagement: boolean = false;
  hideDetailView: boolean = false;
  maxFineDate: any;
  minFineDate: any;
  paramSubscription: any;
  indx:any;
  selected = [];
  selecteditem: any = 0;
  selectedIndex:any;
  color = ['#726236']
  constructor(
    public common: CommonService,
    public Api: VehicleMgmtServiceService,
    public bsModalRef: BsModalRef,
    public modalService: BsModalService,
    public route: ActivatedRoute,
    public util: UtilsService,
    public router: Router,
    private rd: Renderer2
  ) {

    this.User = JSON.parse(localStorage.getItem('User'));

    this.page = 1;
    this.maxSize = 10;
    this.length = 0;

    // formModel definition
    this.formModel = {
      VehicleId: 0,
      VehicleFineID: 0,
      VehicleModelID: 0,
      FineNumber: 0,
      FinedDate: '',
      Location: 0,
      BlackPoints: 0,
      Status: 0,
      Description: "",
      EmailTo: null,
      EmailCCDepartmentID: null,
      EmailCCUserID: null,
      DeleteFlag: 0,
      CreatedBy: 0,
      CreatedDateTime: '',
      UpdatedBy: 0,
      UpdatedDateTime: ''
    }
    // -------->

    // param routing
    this.screenStatus = 'Create';

    this.route.params.subscribe(param => {
      // var data = this.util.decryption(param.data);
      // this.queryParam = data;
      // var id = data.VehicleFineID;
      var id = param.ID;
      if (id) {
        this.PlateNumber = param.PlateNumber;
        this.loadData(id);
        this.screenStatus = 'View';
        this.hideDetailView = true;
      } else if (param.PlateNumber) {
        this.fromManagement = true;
        this.PlateNumber = param.PlateNumber;
        this.search();
        this.hideDetailView = true;
      } else {
        this.hideDetailView = false;
        this.screenStatus = 'Create';
      }

    });
    // this.paramSubscription = this.route.queryParams.subscribe(re => {
    //   if (re) {
    //     if (re.from == 'List') {
    //       route.params.subscribe(param => {
    //         // var data = this.util.decryption(param.data);
    //         // this.queryParam = data;
    //         // var id = data.VehicleFineID;
    //         var id = param.ID;
    //         this.PlateNumber = param.PlateNumber;
    //         if (id > 0) {
    //           this.loadData(id);
    //           this.screenStatus = 'View';
    //         } else {
    //           this.screenStatus = 'Create'
    //         }
    //       });
    //     } else {
    //       route.params.subscribe(param => {
    //       // if (re.PlateNumber) {
    //         this.fromManagement = true;
    //         this.PlateNumber = param.PlateNumber;
    //         this.search();
    //       });
    //     }
    //   }
    // })

    // ----------->

  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    if (this.lang == 'en') {
      this.common.breadscrumChange('Vehicle Management', 'Log a fine', '');
      this.columns = [
        { name: '#', prop: 'sno', width: 20 },
        { name: 'Vehicle Make', prop: 'VehicleMake' },
        // { name: 'Vehicle Name', prop: 'VehicleName' },
        { name: 'Plate Colour', prop: 'PlateColour' },
        { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
      ];
      if (this.screenStatus == 'View' || this.fromManagement == true) {
        this.columns = [
          { name: '#', prop: 'sno', width: 20 },
          { name: 'Vehicle Make', prop: 'VehicleMake' },
          // { name: 'Vehicle Name', prop: 'VehicleName' },
          { name: 'Plate Colour', prop: 'PlateColour' },
          // { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
        ];
      }
    } else {
      this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('logafine'), '');
      this.columns = [
        { name: '#', prop: 'sno', width: 20 },
        { name: this.arabic('vehiclemake'), prop: 'VehicleMake' },
        // { name: this.arabic('vehiclename'), prop: 'VehicleName' },
        { name: this.arabic('platecolour'), prop: 'PlateColour' },
        { name: this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
      ];
      if (this.screenStatus == 'View' || this.fromManagement == true) {
        this.columns = [
          { name: '#', prop: 'sno', width: 20 },
          { name: this.arabic('vehiclemake'), prop: 'VehicleMake' },
          // { name: this.arabic('vehiclename'), prop: 'VehicleName' },
          { name: this.arabic('platecolour'), prop: 'PlateColour' },
          // { name: 'Action', prop: '', cellTemplate: this.actionTemplate },
        ];
      }
    }


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

    this.getStatus();
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
  setRow(_index: number){
    this.selectedIndex = _index;
  }

  initPage() {
    this.formModel = {
      VehicleFineID: 0,
      VehicleId: 0,
      VehicleModelID: 0,
      FineNumber: 0,
      FinedDate: '',
      Location: 0,
      BlackPoints: 0,
      Status: 0,
      Description: "",
      EmailTo: "",
      EmailCCDepartmentID: 0,
      EmailCCUserID: 0,
      DeleteFlag: 0,
      CreatedBy: 0,
      CreatedDateTime: '',
      UpdatedBy: 0,
      UpdatedDateTime: ''
    }
  }

  maxDate() {
    return new Date();
  }
  // getRowClass(){
  //   if(this.indx !=null){
  //     return 'mobilerowSelected'
  //   }
  // }

  // setClickedRow(index){
  //   this.indx = index
  //   return 'mobilerowSelected'
  // }

  getStatus() {
    this.Api.getStatus('VehicleRequestStatus', this.User.id).subscribe((res: any) => {
      this.StatusList = res;
    });
  }

  search() {
    debugger
    this.progress = true;
    var plateNo = "";
    if (this.PlateNumber == undefined || this.PlateNumber == "") {
      plateNo = "All"
    } else {
      plateNo = this.PlateNumber
    }
    var param = {
      plateNumber: plateNo
    }
    if (plateNo && plateNo.replace(/\s/g, "")) {
      this.Api.getFineACarList('FineCarList', param).subscribe((res: any) => {
        this.rows = res.Collection;
        if (res.Count > 0) {
          this.formModel.VehicleId = parseInt(res.Collection[0].VehicleID);
          this.VehicleModel = res.Collection[0].ModelName;
          if (this.formModel.VehicleId != 0) {
            this.Api.getVehicleRequestById(this.formModel.VehicleId, this.User.id)
              .subscribe((response: any) => {
                if (response.TripPeriodFrom != null) { this.minFineDate = new Date(response.TripPeriodFrom); }
                else { this.minFineDate = new Date(); }
                if (response.TripPeriodTo != null) { this.maxFineDate = new Date(response.TripPeriodTo); }
                else { this.maxFineDate = new Date(); }
              });
          }
          this.showCarList = (res.Count > 0) ? true : false;
          this.progress = false;
          let i = 0;
          this.rows.map(res => {
            this.rows[i]['sno'] = i + 1;
            i++;
          });
        
        } else {
          this.progress = false;
        
        }
      });
    } else {
      this.progress = false;
    }
  }

  selectCar(row) {
    this.hideDetailView = true;
    this.formModel.VehicleId = row.VehicleID;
    this.VehicleModel = row.ModelName;
  }

  validate() {
    var flag = false;
    if (
      this.VehicleModel && this.Date && this.util.isValidDate(this.Date) &&
      this.Location && this.fineNumber && this.FineDescription &&
      this.Location.trim() && this.Time &&
      this.fineNumber.trim() && this.Status &&
      this.FineDescription.trim() && this.formModel.VehicleId
    ) {
      flag = true;
    }
    return flag;
  }

  prepareData(type) {
    this.formModel.VehicleModelID = this.VehicleModel;
    this.formModel.FineNumber = this.fineNumber;
    this.formModel.FinedDate = this.util.concatDateAndTime(this.Date, this.Time);
    this.formModel.Location = this.Location;
    this.formModel.Status = this.Status;
    this.formModel.Description = this.FineDescription;
    this.formModel.BlackPoints = this.BlackPoints;
    if (type == 'Create') {
      this.formModel.CreatedBy = this.User.id;
      this.formModel.CreatedDateTime = new Date;
    } else {
      this.formModel.VehicleFineID = this.vehicleFineID;
      this.formModel.UpdatedBy = this.User.id;
      this.formModel.UpdatedDateTime = new Date;
    }
    return this.formModel;
  }

  setData(data) {
    this.formModel.VehicleId = data.VehicleID;
    this.vehicleFineID = data.VehicleFineID;
    this.VehicleModel = data.VehicleModelID;
    this.fineNumber = data.FineNumber;
    this.Date = (data.FinedDate) ? new Date(data.FinedDate) : '';
    this.Time = this.util.formatAMPM(new Date(data.FinedDate));
    this.Location = data.Location;
    this.Status = data.Status;
    this.BlackPoints = data.BlackPoints;
    this.FineDescription = data.Description;
  }

  fineSave() {
    this.isApiLoading = true;
    var param = this.prepareData(this.screenStatus);
    var lang = this.common.currentLang;
    if (this.screenStatus == 'Create') {
      this.Api.saveFine("Fine", param, this.User.id).subscribe(res => {
        this.message = (this.common.language == 'English') ? "Fine Submitted Successfully" : this.arabic('finesubmittedmsg');
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        this.isApiLoading = false;
        this.bsModalRef.content.message = this.message;
        this.bsModalRef.content.pagename = 'vehicle-rent-car';
        this.bsModalRef.content.closeEvent.subscribe(res => {
          if (this.fromManagement) {
            this.router.navigate([lang + '/app/vehicle-management/vehicle-list']);
          } else {
            this.router.navigate([lang + '/app/vehicle-management/fine-management/dashboard']);
          }
        });
      });
    } else {
      this.Api.updateFine("Fine", param).subscribe(res => {
        this.message = (this.common.language == 'English') ? "Fine Updated Successfully" : this.arabic('fineupdatedmsg');
        this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
        this.isApiLoading = false;
        this.bsModalRef.content.message = this.message;
        this.bsModalRef.content.pagename = 'vehicle-rent-car';
        this.bsModalRef.content.closeEvent.subscribe(res => {
          if (this.fromManagement) {
            this.router.navigate([lang + '/app/vehicle-management/vehicle-list']);
          } else {
            this.router.navigate([lang + '/app/vehicle-management/fine-management/dashboard']);
          }
        });
      });
    }
  }

  fineDelete() {
    this.isApiLoading = true;
    var lang = this.common.currentLang;
    this.Api.deleteFine('Fine', this.vehicleFineID).subscribe(res => {
      this.message = (this.common.language == 'English') ? "Fine Deleted Successfully" : this.arabic('finedeletedmsg');
      this.bsModalRef = this.modalService.show(SuccessComponent, this.config);
      this.isApiLoading = false;
      this.bsModalRef.content.message = this.message;
      this.bsModalRef.content.pagename = 'vehicle-rent-car';
      this.bsModalRef.content.closeEvent.subscribe(res => {
        this.router.navigate([lang + '/app/vehicle-management/fine-management/dashboard']);
      });
    });
  }

  getFineList() {

  }

  loadData(id) {
    // this.PlateNumber = this.queryParam.PlateNumber;
    this.search();
    this.Api.getFineListById('Fine', id, this.User.id).subscribe(res => {
      this.showCarList = true;
      this.setData(res);
    });
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

}
