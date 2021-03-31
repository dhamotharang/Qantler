import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { BsModalService, BsDatepickerConfig, BsModalRef } from 'ngx-bootstrap';
import { CommonService } from 'src/app/common.service';
import { UtilsService } from 'src/app/shared/service/utils.service';
import { DatePipe } from '@angular/common';
import { VehicleMgmtServiceService } from '../../service/vehicle-mgmt-service.service';
import { VehicleRentCarCreateModalComponent } from 'src/app/modal/vehicle-rent-car-create-modal/vehicle-rent-car-create-modal.component';

@Component({
  selector: 'app-vehicle-mgmt-rent-car',
  templateUrl: './vehicle-mgmt-rent-car.component.html',
  styleUrls: ['./vehicle-mgmt-rent-car.component.scss']
})
export class VehicleMgmtRentCarComponent implements OnInit {
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>
  length: any;
  maxSize: any
  tableMessages: { emptyMessage: any; };
  formLabels: {
    title: any
    from: any,
    to: any,
    search: any,
    rentcarcompany: any,
    report: any,
    reportTitle: any,
    smartSearch: any
  };
  from: any;
  to: any;
  columns: any;
  dateMsg: string;
  rows: any;
  page: any;

  gridLabel: {
    companyname: any
    date: any
    conatactname: any
    contactnumber: any
    action: any
    carcompanyid: any
  };
  lang: any;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  progress: boolean;
  saveEvent: any;
  smartSearch: string;
  createEvent: any;
  Currentuser: any;
  disableBtn:boolean;
  config = {
    class: 'modal-lg',
    backdrop: true,
    ignoreBackdropClick: true
  };

  constructor(
    public common: CommonService,
    protected modalService: BsModalService,
    public util: UtilsService,
    public datePipe: DatePipe,
    public bsModalRef: BsModalRef,
    public api: VehicleMgmtServiceService
  ) { }

  ngOnInit() {
    this.Currentuser = JSON.parse(localStorage.getItem('User'));
    this.languageSupport();
    this.getVehicleList();
    this.createEvent = this.common.createCarPopup$.subscribe(res => {
      this.filterInit();
      this.create();
    });
    this.saveEvent = this.api.reloadRentCar$.subscribe(res => {
      this.filterInit();
      this.getVehicleList();
    });
  }

  filterInit() {
    this.from = '';
    this.to = '';
    this.smartSearch = '';
  }

  getVehicleList() {
    this.disableBtn = true;
    this.progress = true;
    var from = (this.from) ? new Date(this.from).toJSON() : '',
      to = (this.to) ? new Date(this.to).toJSON() : '';
    this.api.getVehicleCarCompanyList("VehicleCarCompany", this.page, this.maxSize, from, to, this.smartSearch).subscribe((res: any) => {
      this.progress = false;
      this.disableBtn = false;
      this.rows = res.Collection;
      this.rows.map(res => res.CreatedDateTime = (res.CreatedDateTime) ? this.datePipe.transform(res.CreatedDateTime, "dd/MM/yyyy") :
        this.datePipe.transform(res.UpdatedDateTime, "dd/MM/yyyy"));
      this.length = res.Count;
    });
  }


  maxDate(date) {
    return this.util.maxDateCheck(date);
  }

  dateValidation() {
    if (this.from && this.to) {
      var msg = this.util.dateValidation(this.from, this.to);
      if (msg && msg.flag) {
        this.dateMsg = msg.msg;
        return msg.flag;
      } else {
        return false;
      }
    }
  }

  onChangePage(page) {
    this.page = page;
    this.getVehicleList();
  }

  viewData(type, data) {
    this.bsModalRef = this.modalService.show(VehicleRentCarCreateModalComponent, this.config);
    this.bsModalRef.content.showType = type;
    this.bsModalRef.content.showData = data;
  }

  create() {
    this.bsModalRef = this.modalService.show(VehicleRentCarCreateModalComponent, this.config);
    this.bsModalRef.content.showType = 'Create';
    //this.bsModalRef.content.showData = data;
  }

  showReport(temp) {
    this.disableBtn = true;
    this.filterInit();
    this.bsModalRef = this.modalService.show(temp, this.config);
  }

  closemodal() {
    this.filterInit();
    this.bsModalRef.hide();
    this.disableBtn = false;
  }

  downloadReport() {
    var from = (this.from) ? new Date(this.from).toJSON() : '',
      to = (this.to) ? new Date(this.to).toJSON() : '';
    var param = {
      'CreatedDateFrom': from,
      'CreatedDateTo': to,
      'UserID': this.Currentuser.id,
      'SmartSearch': this.smartSearch
    }
    this.api.dowloadExcel('VehicleCarCompany','Rent Car Report',param);
    this.closemodal();
  }



  languageSupport() {

    if (this.common.language == 'English') {
      this.lang = 'en';
      this.common.topBanner(true, 'Rent a Car Company', '+ RENT A CAR COMPANY', 'rent-car-create');
      this.common.breadscrumChange("Vehicle Management", "Rent a Car Company Management", '');
      this.gridLabel = {
        companyname: "Company Name",
        date: "Created Date",
        conatactname: "Contact Name",
        contactnumber: "Contact Number",
        action: 'Action',
        carcompanyid: 'Car CompanyID',
      };
    } else {
      this.lang = 'ar';
      this.common.topBanner(true, this.arabic('rentacarcompany'), '+ '+this.arabic('rentacarcompany'), 'rent-car-create');
      this.common.breadscrumChange(this.arabic('vehiclemgmt'), this.arabic('rentacarcompanymgmt'), '');
      this.gridLabel = {
        companyname: this.arabic('companyname'),
        date: this.arabic('createddate'),
        conatactname: this.arabic('contactname'),
        contactnumber: this.arabic('contactnumber'),
        action: this.arabic('action'),
        carcompanyid: 'Car CompanyID',
      };
    }

    this.maxSize = 10;
    this.page = 1;
    this.formLabels = {
      title: (this.common.language == 'English') ? 'FILTER BY' : this.arabic('filterby'),
      from: (this.common.language == 'English') ? 'Created Date From' : this.arabic('datefrom'),
      to: (this.common.language == 'English') ? 'Created Date To' : this.arabic('dateto'),
      search: (this.common.language == 'English') ? 'Search' : this.arabic('search'),
      smartSearch: (this.common.language == 'English') ? 'Smart Search' : this.arabic('smartsearch'),
      rentcarcompany: (this.common.language == 'English') ? 'RENT A CAR COMPANY' : this.arabic('search'),
      report: (this.common.language == 'English') ? 'Show Available Reports' : this.arabic('showavailablereports'),
      reportTitle: (this.common.language == 'English') ? "Report" : this.arabic('report')
    };
    this.tableMessages = {
      emptyMessage: (this.common.language == 'English') ? 'No data to display' : this.arabic('nodatatodisplay')
    };

    this.columns = [
      { name: this.gridLabel.companyname, prop: 'CompanyName' },
      { name: this.gridLabel.date, prop: 'CreatedDateTime' },
      { name: this.gridLabel.conatactname, prop: 'ContactName' },
      { name: this.gridLabel.contactnumber, prop: 'ContactNumber' },
      { name: this.gridLabel.action, prop: '', cellTemplate: this.actionTemplate }
    ];
  }
  arabic(word) {
    return this.common.arabic.words[word];
  }

  ngOnDestroy() {
    this.saveEvent.unsubscribe();
    this.createEvent.unsubscribe();
  }

}
