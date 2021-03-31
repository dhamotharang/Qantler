import { BsModalService } from 'ngx-bootstrap/modal';
import { Component, OnInit, Renderer2, Inject, Input } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { BsModalRef, BsDatepickerConfig } from 'ngx-bootstrap';
import { DOCUMENT } from '@angular/common';
import { VehicleMgmtServiceService } from './../../vehicle-mgmt/service/vehicle-mgmt-service.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-driver-management-modal',
  templateUrl: './driver-management-modal.component.html',
  styleUrls: ['./driver-management-modal.component.scss']
})
export class DriverManagementModalComponent implements OnInit {
  @Input() id: any;
  @Input() driverName: any;
  lang: any;
  bsModalRef: BsModalRef;
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'DD/MM/YYYY'
  };
  rows: Array<any> = [];
  columns: Array<any> = [];
  tableMessages: { emptyMessage: any; };
  currentUser:any;
  ExtraCompensateModel:any = {
    StartDate:'',
    EndDate: ''
  };
  constructor(
    public common: CommonService,
    public modalService: BsModalService,
    private renderer: Renderer2,
    public datepipe: DatePipe,
    private VehicleMgmtService:VehicleMgmtServiceService,
    @Inject(DOCUMENT) private document: Document) { }
  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    this.lang = this.common.currentLang;
    if(this.common.currentLang != 'en'){
      this.columns = [
        { name: this.arabicfn('date'), prop: 'LogDate' },
        { name: this.arabicfn('extrahours'), prop: 'ExtraHours' },
        { name: this.arabicfn('compensate'), prop: 'CompensateHours' },
        { name: this.arabicfn('extrahoursreport'), prop: 'BalanceExtraHours'}
      ];
    }else{
      this.columns = [
        { name: 'Date', prop: 'LogDate' },
        { name: 'Extra Hours', prop: 'ExtraHours' },
        { name: 'Compensate', prop: 'CompensateHours' },
        { name: 'Extra Hours Report', prop: 'BalanceExtraHours'}
      ];
    }
    this.tableMessages = {
      emptyMessage: (this.lang == 'en') ? 'No data to display' : this.arabicfn('nodatatodisplay')
    };

    this.getDriverView(this.id);
  }

  validateDateFilter() {
    let flag = true;
    if(this.ExtraCompensateModel.StartDate <= this.ExtraCompensateModel.EndDate) {
      flag = false;
    }
    return flag;
  }

  getDriverView(id: any) {
    this.id = id;
    let StartDate = this.datepipe.transform(this.ExtraCompensateModel.StartDate, 'yyyy-MM-dd');
    let EndDate = this.datepipe.transform(this.ExtraCompensateModel.EndDate, 'yyyy-MM-dd');
    
    let dateFilter: any = {
      StartDate: StartDate ? StartDate : '',
      EndDate: EndDate ? EndDate : ''
    };
    this.VehicleMgmtService.getDriverModalView(id,this.currentUser.id, dateFilter).subscribe((userList:any)=>{
      this.rows = userList.CompensateExtra; 
      this.rows.forEach(val =>{
        val.LogDate = this.datepipe.transform(val.LogDate, "dd/MM/yyyy");
      })
    });
  }
  
  arabicfn(word) {
    return this.common.arabic.words[word];
  }

  closemodal() {
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
  }

  downloadExcel(){

    let StartDate = this.datepipe.transform(this.ExtraCompensateModel.StartDate, 'yyyy-MM-dd');
    let EndDate = this.datepipe.transform(this.ExtraCompensateModel.EndDate, 'yyyy-MM-dd');
    
    let dateFilter: any = {
      DateRangeFrom: StartDate ? StartDate : '',
      DateRangeTo: EndDate ? EndDate : ''
    };

    dateFilter.UserID = this.currentUser.id;
    let dateVal = new Date(), cur_date = dateVal.getDate() +'-'+(dateVal.getMonth()+1)+'-'+dateVal.getFullYear();
    // this.reportService.downloadModuleReport(toSendReportOptions,'vehicle_management').subscribe((data) => {
      this.VehicleMgmtService.driverListExcelDownload(this.id,dateFilter).subscribe((data) => {
      var url = window.URL.createObjectURL(data);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = 'DriverTripsReport.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
        // this.bsModalRef.hide();
    });
  }
}
