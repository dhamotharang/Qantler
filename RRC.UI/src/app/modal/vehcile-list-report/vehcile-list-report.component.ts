import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { Router } from '@angular/router';
import { VehicleMgmtServiceService } from 'src/app/vehicle-mgmt/service/vehicle-mgmt-service.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { MaintenanceService } from 'src/app/maintenance/service/maintenance.service';
import { ReportsService } from 'src/app/shared/service/reports.service';
import { UtilsService } from 'src/app/shared/service/utils.service';

@Component({
  selector: 'app-vehcile-list-report',
  templateUrl: './vehcile-list-report.component.html',
  styleUrls: ['./vehcile-list-report.component.scss']
})
export class VehcileListReportComponent implements OnInit {
  reportFilter:any = {
    PlateNumber:this.common.currentLang == 'en' && "All" || this.arabic('all'),
    UserID:'',
    PlateColor:this.common.currentLang == 'en' && "All" || this.arabic('all'),
    DepartmentOffice:this.common.currentLang == 'en' && "All" || this.arabic('all'),
    Destination:'',
    RequestorDepartment:'',
    AlternativeVehicle: this.common.currentLang == 'en' && "No" || this.arabic('no')
  };
  currentUser:any;
  lang:any;
  departmentList:any = [];
  plateColourList:any = [];
  plateNumberList:any = [];
  alternativeVehicleList: any=[];
  constructor(public common: CommonService,
    public router:Router,
    public vehicleservice: VehicleMgmtServiceService,
    public modalService: BsModalService,
    private maintenanceService:MaintenanceService,
    private reportService:ReportsService,
    private utilsService:UtilsService,
    public bsModalRef: BsModalRef) { }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('User'));
    this.lang = this.common.currentLang;
    if(this.lang != 'en'){
      this.alternativeVehicleList = [this.arabic('yes'), this.arabic('no'), this.arabic('both')];
    }else{
      this.alternativeVehicleList = ['Yes','No','Both'];
    }
    this.getRequestorDepartment();
    this.getPlateNumberAndColorList();
  }

  arabic(word) {
    return this.common.arabic.words[word];
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

  downloadExcel() {
    let ReferenceNumber = '';
    let EventType = '';
    let EventRequestor = '';
    let EventDetails = '';
    let DateFrom = '';
    let DateTo = '';
    let Status = '';
    let Location = '';

    let toSendReportOptions:any = {
      SmartSearch:this.reportFilter.SmartSearch,
      DepartmentOffice:this.reportFilter.DepartmentOffice,
      RequestorDepartment: this.reportFilter.RequestorDepartment,
      PlateNumber:this.reportFilter.PlateNumber,
      PlateColor:this.reportFilter.PlateColor,
      Destination:this.reportFilter.Destination
    };

      if(!this.reportFilter.SmartSearch){
        toSendReportOptions.SmartSearch = '';
      }

      if(!this.reportFilter.DepartmentOffice){
        toSendReportOptions.DepartmentOffice = '';
      }

      if(!this.reportFilter.RequestorDepartment){
        toSendReportOptions.RequestorDepartment = '';
      }

      if(!this.reportFilter.PlateNumber){
        toSendReportOptions.PlateNumber = '';
      }

      if(!this.reportFilter.PlateColor){
        toSendReportOptions.PlateColor = '';
      }

      if(!this.reportFilter.Destination){
        toSendReportOptions.Destination = '';
      }

      if(!this.reportFilter.AlternativeVehicle){
        toSendReportOptions.AlternativeVehicle = '';
      }

      // if(!this.reportFilter.SmartSearch){
      //   toSendReportOptions.DepartmentOffice = '';
      // }

      if(toSendReportOptions.PlateNumber == "All" || toSendReportOptions.PlateNumber == this.arabic('all'))
      {
        toSendReportOptions.PlateNumber = '';
      }
      if(toSendReportOptions.PlateColor == "All" || toSendReportOptions.PlateColor == this.arabic('all'))
      {
        toSendReportOptions.PlateColor = '';
      }
      if(toSendReportOptions.DepartmentOffice == "All" || toSendReportOptions.DepartmentOffice == this.arabic('all'))
      {
        toSendReportOptions.DepartmentOffice = '';
      }

      if(toSendReportOptions.AlternativeVehicle == "Both" || toSendReportOptions.AlternativeVehicle == this.arabic('both'))
      {
        toSendReportOptions.AlternativeVehicle = '';
      }
      if(toSendReportOptions.AlternativeVehicle == "Yes" || toSendReportOptions.AlternativeVehicle == this.arabic('yes'))
      {
        toSendReportOptions.AlternativeVehicle = true;
      }
      if(toSendReportOptions.AlternativeVehicle == "No" || toSendReportOptions.AlternativeVehicle == this.arabic('no'))
      {
        toSendReportOptions.AlternativeVehicle = false;
      }
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
        this.bsModalRef.hide();
    });
  }

}
