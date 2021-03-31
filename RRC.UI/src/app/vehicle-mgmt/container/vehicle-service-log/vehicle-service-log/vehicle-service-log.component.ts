import { Component, OnInit, ViewChild, TemplateRef, Input } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { BsDatepickerConfig, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { VehicleMgmtServiceService } from 'src/app/vehicle-mgmt/service/vehicle-mgmt-service.service';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilsService } from 'src/app/shared/service/utils.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-vehicle-service-log',
  templateUrl: './vehicle-service-log.component.html',
  styleUrls: ['./vehicle-service-log.component.scss']
})
export class VehicleServiceLogComponent implements OnInit {

  @Input() mode;
  PlateNumber: any;
  progress: boolean;
  tableMessages: { emptyMessage: any; };
  rows: Array<any> = [];
  Servicerows: Array<any> = [];
  ServicTypeID: any = 0;
  VehicleID: any = 0;
  selectedIndex:any;
  ServiceTypeList: Array<any> =
   [{ ServiceTypeName: (this.common.currentLang == 'en') ? "Both" : this.arabic('both'), ServicTypeID: 0 }, 
    { ServiceTypeName: (this.common.currentLang == 'en') ?"Service": this.arabic('service'), ServicTypeID: 1 }, 
    { ServiceTypeName: (this.common.currentLang == 'en') ?"Tyre Change": this.arabic('tyrechange'), ServicTypeID: 2 }];
  
  showCarList: boolean = false;
  showCarServiceList: boolean = false;
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  @ViewChild('Mileage') Mileage: TemplateRef<any>; 
  @ViewChild('Date') Date: TemplateRef<any>; 
  @ViewChild('requestType') requestType: TemplateRef<any>;  
  selected = [];
  selecteditem: any = 0;
  isApiLoading: boolean;
  lang: any;
  public columns: Array<any> = [];
  public Servicecolumns: Array<any> = [];
  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalItems: []
  };

  public Serviceconfig: any = {
    paging: true,
    sorting: { columns: this.Servicecolumns },
    filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered', 'm-b-0'],
    totalItems: []
  };

  constructor(public common: CommonService, public Api: VehicleMgmtServiceService) { }

  ngOnInit() {
    this.lang = this.common.currentLang;
    if(this.common.currentLang == 'en')
    this.common.breadscrumChange('Vehicle Management','Service Log', '');
    else
    this.common.breadscrumChange(this.arabic('vehiclemgmt'),this.arabic('servicelog'), '');
    debugger;
    this.columns = [
      { name: '#', prop: 'sno'},
      { name: (this.common.currentLang == 'en') ?'Vehicle Make':this.arabic('vehiclemake'), prop: 'VehicleMake' },
      // { name: (this.common.currentLang == 'en') ?'Vehicle Name':this.arabic('vehiclename'), prop: 'VehicleName' },
      { name: (this.common.currentLang == 'en') ?'Plate Colour':this.arabic('vehiclename'), prop: 'PlateColour' },
      { name: (this.common.currentLang == 'en') ?'Action':this.arabic('action'), prop: '', cellTemplate: this.actionTemplate },
    ];

    this.tableMessages = {
      emptyMessage: (this.common.currentLang == 'en') ? 'No data to display' : this.arabic('nodatatodisplay')
    };

    this.Servicecolumns = [
      { name: (this.common.currentLang == 'en') ?'Date':this.arabic('date'), prop: 'CreatedDateTime',cellTemplate: this.Date },
      { name: (this.common.currentLang == 'en') ?'Current Mileage':this.arabic('currentmileage'), prop: 'CurrentMileage',cellTemplate: this.Mileage  },
      { name: (this.common.currentLang == 'en') ?'Next Mileage':this.arabic('nextmileage'), prop: 'NextMileage',cellTemplate: this.Mileage },
      { name: (this.common.currentLang == 'en') ?'Type':this.arabic('type'), prop: 'LogType',cellTemplate: this.requestType}
    ];
  }

  arabic(word) {
    return this.common.arabic.words[word];
  }

  onActivate(event){
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

  search() {
    debugger
    this.progress = true;
    this.VehicleID = null;
    var plateNo = "";
    if(this.PlateNumber==undefined || this.PlateNumber==""){
      plateNo = "All"
    }else{
      plateNo = this.PlateNumber
    }
     var param = {
      plateNumber: plateNo
    }
    this.rows = null;
    if (plateNo && plateNo.replace(/\s/g, "")) {
      this.Api.getFineACarList('FineCarList', param).subscribe((res: any) => {
        debugger;
        this.rows = res.Collection;
        if (res.Count > 0) {
          this.showCarList = (res.Count > 0) ? true : false;
          this.progress = false;
          let i = 0;
          this.rows.map(res => {
            this.rows[i]['sno'] = i + 1;
            i++;
          });
          // this.selected = [this.rows[0]];
        } else {
          this.selected=[];
          this.progress = false;
        }
      });
    } else {
      this.progress = false;
    }
  }
  setRow(_index: number){
    this.selectedIndex = _index;
  }

  selectCar(VehicleID) {
    this.VehicleID = VehicleID;
    if (VehicleID) {
      this.Api.getFineACarServiceList('GetVehicleManagementLogService', VehicleID, this.ServicTypeID).subscribe((res: any) => {
        this.Servicerows = res;

        if (res) {
          this.showCarServiceList = (res) ? true : false;
          this.progress = false;
        } else {
          this.progress = false;
        }
      });
    }
    else {
      this.progress = false;
    }
  }

  selectType() {
    if (this.VehicleID) {
      this.Api.getFineACarServiceList('GetVehicleManagementLogService', this.VehicleID, this.ServicTypeID).subscribe((res: any) => {
        this.Servicerows = res;
        if (res) {
          this.showCarServiceList = (res) ? true : false;
          this.progress = false;
        } else {
          this.progress = false;
        }
      });
    }
    else {
      this.progress = false;
    }
  }

  getRequestListName(value) {
    let selectedVal = _.find(this.ServiceTypeList, function (item) { return (item.ServicTypeID == value) });
    return selectedVal.ServiceTypeName;
  }

  ReportDownload()
  {
    let dateVal = new Date(), cur_date = dateVal.getDate() +'-'+(dateVal.getMonth()+1)+'-'+dateVal.getFullYear();
    this.Api.vehicleServiceListExcelDownload(this.VehicleID).subscribe((resultBlob) =>{
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = 'VehicleManagement-'+cur_date+'.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
  });
  }
}

